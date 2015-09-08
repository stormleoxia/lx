using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lx.Tools.Reflection
{
    /// <summary>
    /// Extension for any class
    /// </summary>
    public static class ClassEx
    {
        private static readonly IDictionary<Type, object> dictionary = new Dictionary<Type, object>(); 

        /// <summary>
        /// Clone instance Property-wise.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static T PropertyWiseClone<T>(this T instance) where T : new()
        {
            var type = typeof (T);
            var clone = new T();
            IPropertyCopier<T> instanceCopier;
            object boxed;
            if (!dictionary.TryGetValue(type, out boxed))
            {
                instanceCopier = new InstanceCopier<T>();
                dictionary[type] = instanceCopier;
            }
            else
            {
                instanceCopier = (IPropertyCopier<T>) boxed;
            }
            instanceCopier.Copy(instance, clone);
            return clone;
        }
    }

    public sealed class InstanceCopier<T> : IPropertyCopier<T>
    {
        private readonly IPropertyCopier<T>[] _copiers;

        public InstanceCopier()
        {
            var properties = typeof (T).GetRuntimeProperties();
            var localCopiers = new List<IPropertyCopier<T>>();
            foreach (var property in properties)
            {
                localCopiers.Add(Properties.CreatePropertyCopier<T>(property));
            }
            _copiers = localCopiers.ToArray();
        }

        public void Copy(T source, T destination)
        {
            for (int index = 0; index < _copiers.Length; index++)
            {
                _copiers[index].Copy(source, destination);
            }
        }
    }

    public static class Properties
    {
        private static readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public static PropertyCopier<T> CreatePropertyCopier<T>(PropertyInfo property)
        {
            var key = typeof (T).FullName + "." + property.Name;
            object boxed;
            if (!_dictionary.TryGetValue(key, out boxed))
            {
                var copier = new PropertyCopier<T>(property.Name);
                _dictionary[key] = copier;
                return copier;
            }
            return (PropertyCopier<T>)boxed;
        }
    }
}
