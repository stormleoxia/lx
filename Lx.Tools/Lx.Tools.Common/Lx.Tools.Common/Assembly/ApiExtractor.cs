using System;
using System.Collections.Generic;

namespace Lx.Tools.Common.Assembly
{
    public sealed class ApiExtractor : MarshalByRefObject, IApiExtractor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiExtractor"/> class.
        /// Keep it because it is used by AppDomain create instance;
        /// </summary>
        public ApiExtractor()
        {
        }

        public AssemblyApi ExtractApi(string assemblyPath)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(assemblyPath);
            var types = assembly.GetTypes();
            Dictionary<string, NamespaceDefinition> namespaces = new Dictionary<string, NamespaceDefinition>();
            foreach (var type in types)
            {
                NamespaceDefinition nspace;
                var space = type.Namespace;
                if (String.IsNullOrEmpty(space))
                {
                    space = "NULL";
                }
                if (!namespaces.TryGetValue(space, out nspace))
                {
                    nspace = new NamespaceDefinition(space);
                    namespaces[space] = nspace;
                }
                nspace.AddType(type);
            }
            return new AssemblyApi(namespaces.Values);
        }

        public void Dispose()
        {
            
        }
    }
}
