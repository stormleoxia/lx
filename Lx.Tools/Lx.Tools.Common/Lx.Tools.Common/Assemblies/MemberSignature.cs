using System;
using System.Reflection;
using System.Text;

namespace Lx.Tools.Common.Assemblies
{
    [Serializable]
    public class MemberSignature
    {
        public MemberSignature()
        {
        }

        public MemberSignature(MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Constructor:
                    var constructor = (ConstructorInfo) member;
                    Signature = constructor.Name + "(" + SerializeParameters(constructor.GetParameters()) + ")";
                    break;
                case MemberTypes.Event:
                    var eventInfo = (EventInfo) member;
                    Signature = "Event " + GetGenericSignature(eventInfo.EventHandlerType) + " " + member.Name;
                    break;
                case MemberTypes.Field:
                    var fieldInfo = (FieldInfo) member;
                    Signature = "Field " + GetGenericSignature(fieldInfo.FieldType) + " " + fieldInfo.Name;
                    break;
                case MemberTypes.Property:
                    var property = (PropertyInfo) member;
                    var getMethod = property.GetGetMethod(false);
                    var setMethod = property.GetSetMethod(false);
                    var getter = (getMethod != null) ? "get;" : string.Empty;
                    var setter = (setMethod != null) ? "set;" : string.Empty;
                    Signature = "Property " + GetGenericSignature(property.PropertyType) + " " + property.Name + "{" +
                                getter + " " + setter + "}";
                    break;
                case MemberTypes.Method:
                    var method = (MethodInfo) member;
                    var genericParameters = GetGenericSignatures(method.GetGenericArguments());
                    Signature = "Method " + GetGenericSignature(method.ReturnType) + " " + method.Name +
                                genericParameters + "(" + SerializeParameters(method.GetParameters()) + ")";
                    break;
                case MemberTypes.TypeInfo:
                    Signature = "Type " + member.Name + "{}";
                    break;
                case MemberTypes.Custom:
                    Signature = "Custom " + member.Name;
                    break;
                case MemberTypes.NestedType:
                    Signature = "Nested " + member.Name + "{}";
                    break;
                case MemberTypes.All:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string Signature { get; set; }

        private string GetGenericSignatures(Type[] types)
        {
            if (types.Length > 0)
            {
                var builder = new StringBuilder();
                builder.Append("<");
                builder.Append(GetGenericSignature(types[0]));
                var separator = ",";
                for (var index = 1; index < types.Length; ++index)
                {
                    builder.AppendFormat("{0}{1}", separator, GetGenericSignature(types[index]));
                }
                builder.Append(">");
                return builder.ToString();
            }
            return string.Empty;
        }

        private string GetGenericSignature(Type type)
        {
            var res = TypeToString(type);
            var genericArguments = type.GenericTypeArguments;
            if (genericArguments.Length > 0)
            {
                res += GetGenericSignatures(genericArguments);
            }
            return res;
        }

        private static string TypeToString(Type type)
        {
            return type.FullName ?? type.Name;
        }

        private string SerializeParameters(ParameterInfo[] parameters)
        {
            if (parameters.Length > 0)
            {
                var builder = new StringBuilder();
                builder.AppendFormat("{0} {1}", GetGenericSignature(parameters[0].ParameterType), parameters[0].Name);
                var separator = ", ";
                for (var index = 1; index < parameters.Length; index++)
                {
                    var parameter = parameters[index];
                    builder.AppendFormat("{1}{0} {2}", GetGenericSignature(parameter.ParameterType), separator,
                        parameter.Name);
                }
                return builder.ToString();
            }
            return string.Empty;
        }

        public override string ToString()
        {
            return Signature;
        }
    }
}