using System;
using System.Reflection;
using System.Text;

namespace Lx.Tools.Common.Assembly
{
    [Serializable]
    public class MemberSignature
    {
        public string Signature { get; set; }

        public MemberSignature()
        {
        }

        public MemberSignature(MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Constructor:
                    Signature = member.Name + "()";
                    break;
                case MemberTypes.Event:
                    Signature = "Event " + member.Name;
                    break;
                case MemberTypes.Field:
                    Signature = "Field " + member.Name;
                    break;
                case MemberTypes.Method:
                    var method = (MethodInfo) member;
                    Signature = "Method " + method.ReturnType.Name + " " + method.Name + "(" + SerializeParameters(method.GetParameters())  + ")";
                    break;
                case MemberTypes.Property:
                    var property = (PropertyInfo) member;
                    var getMethod = property.GetGetMethod(false);
                    var setMethod = property.GetSetMethod(false);
                    var getter = (getMethod != null) ? "get;" : string.Empty;
                    var setter = (setMethod != null) ? "set;" : String.Empty;
                    Signature = "Property " + property.PropertyType + property.Name + "{" + getter + " " + setter + "}";
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

        private string SerializeParameters(ParameterInfo[] parameters)
        {
            StringBuilder builder = new StringBuilder();
            string separator = string.Empty;
            foreach (var parameter in parameters)
            {
                builder.AppendFormat("{1}{0}", parameter, separator);
                separator = ", ";
            }
            return builder.ToString();
        }

        public override string ToString()
        {
            return Signature;
        }
    }
}