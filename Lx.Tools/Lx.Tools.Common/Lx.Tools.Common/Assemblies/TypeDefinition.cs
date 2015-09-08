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
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lx.Tools.Common.Assemblies
{
    [Serializable]
    public class TypeDefinition
    {
        private readonly Dictionary<string, MemberSignature> _signatures = new Dictionary<string, MemberSignature>();

        public TypeDefinition()
        {
            Members = new List<MemberSignature>();
        }

        public TypeDefinition(Type type) : this()
        {
            Namespace = type.Namespace;
            Name = type.Name;
            AddMembers(type.GetMembers(BindingFlags.Public | BindingFlags.Instance));
            AddMembers(type.GetMembers(BindingFlags.Public | BindingFlags.Static));
            AddMembers(type.GetMembers(BindingFlags.Public | BindingFlags.CreateInstance));
        }

        public List<MemberSignature> Members { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }

        private void AddMembers(MemberInfo[] members)
        {
            foreach (var member in members)
            {
                if (member.MemberType == MemberTypes.Method &&
                    (member.Name.StartsWith("get_") || member.Name.StartsWith("set_")))
                {
                    continue;
                }
                var signature = new MemberSignature(member);
                MemberSignature sign;
                if (!_signatures.TryGetValue(signature.Signature, out sign))
                {
                    _signatures[signature.Signature] = signature;
                    Members.Add(signature);
                }
            }
        }

        public IEnumerable<MemberSignature> GetPublicMembersSignatures()
        {
            return Members;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var member in Members)
            {
                builder.AppendFormat("{0}.{1}.{2}{3}", Namespace, Name, member.Signature, Environment.NewLine);
            }
            return builder.ToString();
        }
    }
}