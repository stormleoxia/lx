using System;
using System.Collections.Generic;
using System.Linq;

namespace Lx.Tools.Common.Assembly
{
    [Serializable]
    public class AssemblyApi
    {
        private readonly List<NamespaceDefinition> _namespaces;

        public AssemblyApi(IEnumerable<NamespaceDefinition> namespaces)
        {
            _namespaces = namespaces.ToList();
        }

        public IEnumerable<NamespaceDefinition> GetNamespaces()
        {
            return _namespaces;
        }
    }
}