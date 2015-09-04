using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lx.Tools.Common.Assemblies
{
    public sealed class ApiExtractor : MarshalByRefObject, IApiExtractor
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiExtractor" /> class.
        ///     Keep it because it is used by AppDomain create instance;
        /// </summary>
        public ApiExtractor()
        {
            Loader = new AssemblyLoader();
        }

        internal IAssemblyLoader Loader { get; set; }

        public AssemblyApi ExtractApi(string assemblyPath)
        {
            var assembly = Loader.LoadFrom(assemblyPath);
            var types = assembly.GetTypes();
            var namespaces = new Dictionary<string, NamespaceDefinition>();
            foreach (var type in types)
            {
                NamespaceDefinition nspace;
                var space = type.Namespace;
                if (string.IsNullOrEmpty(space))
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

    public interface IAssemblyLoader
    {
        Assembly LoadFrom(string assemblyPath);
    }

    public class AssemblyLoader : IAssemblyLoader
    {
        public Assembly LoadFrom(string assemblyPath)
        {
            return Assembly.LoadFrom(assemblyPath);
        }
    }
}