#region Copyright (c) 2015 Leoxia Ltd

//  Copyright © 2015 Leoxia Ltd
//  
//  This file is part of Lx.
// 
//  Lx is released under GNU General Public License unless stated otherwise.
//  You may not use this file except in compliance with the License.
//  You can redistribute it and/or modify it under the terms of the GNU General Public License 
//  as published by the Free Software Foundation, either version 3 of the License, 
//  or any later version.
//  
//  In case GNU General Public License is not applicable for your use of Lx, 
//  you can subscribe to commercial license on 
//  http://www.leoxia.com 
//  by contacting us through the form page or send us a mail
//  mailto:contact@leoxia.com
//   
//  Unless required by applicable law or agreed to in writing, 
//  Lx is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES 
//  OR CONDITIONS OF ANY KIND, either express or implied. 
//  See the GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License along with Lx.
//  It is present in the Lx root folder SolutionItems/GPL.txt
//  If not, see http://www.gnu.org/licenses/.

#endregion

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