using System;
using System.Collections.Generic;
using System.Text;

namespace Lx.Tools.Common.Assemblies
{
    [Serializable]
    public class NamespaceDefinition
    {
        private Dictionary<string, TypeDefinition> typeDefinitions = new Dictionary<string, TypeDefinition>(); 
        
        public NamespaceDefinition(string space): this()
        {
            Name = space;
        }

        public NamespaceDefinition()
        {
            Types = new List<TypeDefinition>();
        }

        public List<TypeDefinition> Types { get; set; }

        public string Name { get; set; }

        public IEnumerable<TypeDefinition> GetTypes()
        {
            return Types;
        }

        public void AddType(Type type)
        {
            TypeDefinition typeDefinition;
            if (!typeDefinitions.TryGetValue(type.Name, out typeDefinition))
            {
                typeDefinition = new TypeDefinition(type);
                typeDefinitions[type.Name] = typeDefinition;
                Types.Add(typeDefinition);
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var type in Types)
            {
                builder.Append(type);
            }
            return builder.ToString();
        }
    }
}