using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using ProtoBuf;
using ProtoBuf.Meta;

namespace Lx.Tools.Db.Proto
{
    internal class DatabaseContainer<TKey>
    {
        private readonly Dictionary<TKey, TypeEnvelop<TKey>> _envelops = new Dictionary<TKey, TypeEnvelop<TKey>>();
        private readonly Dictionary<Type, TypeModel> _models = new Dictionary<Type, TypeModel>();
        private readonly Dictionary<Type, bool> _isProtoContracts = new Dictionary<Type, bool>();  

        public DatabaseContainer()
        {
        }

        public DatabaseContainer(List<TypeEnvelop<TKey>> values)
        {
            foreach (var value in values)
            {
                _envelops[value.Key] = value;
            }
        }

        public List<TypeEnvelop<TKey>> Values
        {
            get { return _envelops.Values.ToList(); }
        }

        public void Add<TInstance>(TKey key, TInstance data)
        {
            MemoryStream ms = new MemoryStream();
            Serialize(ms, data);
            var typeEnvelop = new TypeEnvelop<TKey>(key, typeof (TInstance), ms.ToArray()) {Instance = data};
            _envelops[key] = typeEnvelop;
        }

        private void Serialize<TInstance>(MemoryStream ms, TInstance data)
        {
            var type = typeof(TInstance);
            if (IsProtoContractType(type))
            {
                Serializer.Serialize(ms, data);                
            }
            else
            {
                TypeModel model;
                if (!_models.TryGetValue(type, out model))
                {
                    model = InferModel(type);
                    _models[type] = model;
                }
                model.Serialize(ms, data);
            }
        }

        private TypeModel InferModel(Type type)
        {
            var model = TypeModel.Create();
            AddProperties(model, type);
            return model;
        }

        
        private static void AddProperties(RuntimeTypeModel model, Type type)
        {
            var metaType = model.Add(type, true);
            // TODO: Do not add XmlIgnore/DataContractIgnore/ProtoIgnore properties
            foreach (var property in type.GetProperties().OrderBy(x => x.Name))
            {
                metaType.Add(property.Name);
                var propertyType = property.PropertyType;
                if (!IsBuiltinType(propertyType) &&
                    !model.IsDefined(propertyType) && 
                    propertyType.GetProperties().Length > 0)
                {                    
                    AddProperties(model, propertyType);
                }
            }
        }

        private static bool IsBuiltinType(Type type)
        {
            return type.IsValueType || type == typeof (string);
        }

        private bool IsProtoContractType(Type type)
        {
            bool flag;
            if (!_isProtoContracts.TryGetValue(type, out flag))
            {
                flag = type.GetCustomAttributes(typeof (ProtoContractAttribute), true).Length > 0;
                _isProtoContracts[type] = flag;
            }
            return flag;
        }

        public void Remove(TKey key)
        {
            _envelops.Remove(key);
        }

        public TInstance Get<TInstance>(TKey key)
        {
            TypeEnvelop<TKey> envelop;
            if (!_envelops.TryGetValue(key, out envelop))
            {
                return default(TInstance);
            }
            var typeSignature = envelop.TypeSignature;
            if (TypeEnvelop<TKey>.GetSignature(typeof (TInstance)) != typeSignature)
            {
                throw new SerializationException("Stored data for key " + key + " is with a different type signature " +
                                                 typeSignature);
            }
            return OpenEnvelop<TInstance>(envelop);
        }

        private TInstance OpenEnvelop<TInstance>(TypeEnvelop<TKey> envelop)
        {
            if (envelop.Instance == null)
            {
                var ms = new MemoryStream(envelop.SerializedData);
                var deserialized = Deserialize<TInstance>(ms);
                envelop.Instance = deserialized;
                return deserialized;
            }
            return (TInstance) envelop.Instance; // Note there could be unboxing but deserialization is worse.
        }

        private T Deserialize<T>(MemoryStream ms)
        {
            var type = typeof (T);
            if (IsProtoContractType(type))
            {
                return Serializer.Deserialize<T>(ms);                
            }
            else
            {
                TypeModel model;
                if (!_models.TryGetValue(type, out model))
                {
                    model = InferModel(type);
                    _models[type] = model;
                }
                return (T)model.Deserialize(ms, null, type);
            }
        }

        public IEnumerable<T> GetAll<T>()
        {
            var signature = TypeEnvelop<TKey>.GetSignature(typeof (T));
            return _envelops.Values.Where(x => x.TypeSignature == signature).Select(OpenEnvelop<T>).ToArray();
        }
    }
}