using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lx.Tools.Common.Assembly
{
    [Serializable]
    public class TypeDefinition
    {
        private Dictionary<string, MemberSignature> _signatures = new Dictionary<string, MemberSignature>(); 
        public List<MemberSignature> Members { get; set; }

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
            AddMembers(type.GetMembers(BindingFlags.Public | BindingFlags.GetProperty));
            AddMembers(type.GetMembers(BindingFlags.Public | BindingFlags.CreateInstance));
        }

        private void AddMembers(MemberInfo[] members)
        {
            foreach (var member in members)
            {
                var signature = new MemberSignature(member);
                MemberSignature sign;
                if (!_signatures.TryGetValue(signature.Signature, out sign))
                {
                    _signatures[signature.Signature] = signature;
                    Members.Add(signature);
                }
            }
        }

        public string Name { get; set; }

        public string Namespace { get; set; }

        public IEnumerable<MemberSignature> GetPublicMembersSignatures()
        {
            return Members;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var member in Members)
            {
                builder.AppendFormat("{0}.{1}.{2}{3}", Namespace, Name, member.Signature, Environment.NewLine);
            }
            return builder.ToString();
        }
    }
}