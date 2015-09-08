#region Copyright (c) 2015 Leoxia Ltd

// #region Copyright (c) 2015 Leoxia Ltd
// 
// // Copyright © 2015 Leoxia Ltd.
// // 
// // This file is part of Lx.
// //
// // Lx is released under GNU General Public License unless stated otherwise.
// // You may not use this file except in compliance with the License.
// // You can redistribute it and/or modify it under the terms of the GNU General Public License 
// // as published by the Free Software Foundation, either version 3 of the License, 
// // or any later version.
// // 
// // In case GNU General Public License is not applicable for your use of Lx, 
// // you can subscribe to commercial license on 
// // http://www.leoxia.com 
// // by contacting us through the form page or send us a mail
// // mailto:contact@leoxia.com
// //  
// // Unless required by applicable law or agreed to in writing, 
// // Lx is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES 
// // OR CONDITIONS OF ANY KIND, either express or implied. 
// // See the GNU General Public License for more details.
// //
// // You should have received a copy of the GNU General Public License along with Lx.
// // It is present in the Lx root folder SolutionItems/GPL.txt
// // If not, see http://www.gnu.org/licenses/.
// //
// 
// #endregion

#endregion

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