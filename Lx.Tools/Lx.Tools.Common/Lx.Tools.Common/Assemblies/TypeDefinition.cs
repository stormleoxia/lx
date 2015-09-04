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