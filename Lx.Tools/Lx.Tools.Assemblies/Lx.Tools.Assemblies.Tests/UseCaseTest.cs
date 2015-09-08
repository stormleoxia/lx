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

using System.Collections.Generic;
using System.Linq;
using Lx.Tools.Assemblies.Compare;
using Lx.Tools.Common.Assemblies;
using Moq;
using NUnit.Framework;
using AssemblyLoader = Lx.Tools.Assemblies.Compare.AssemblyLoader;

namespace Lx.Tools.Assemblies.Tests
{
    [TestFixture]
    public class UseCaseTest
    {
        [Test]
        public void ApiTest()
        {
            var myNamespace = new NamespaceDefinition();
            myNamespace.AddType(typeof (MyClass));
            var api = new AssemblyApi(new List<NamespaceDefinition> {myNamespace});
            var namespaces = api.GetNamespaces().ToList();
            Assert.AreEqual(1, namespaces.Count);
            foreach (var nspace in namespaces)
            {
                var classes = nspace.Types.ToList();
                Assert.AreEqual(1, classes.Count);
                foreach (var c in classes)
                {
                    var members = c.GetPublicMembersSignatures().ToList();
                    Assert.AreEqual(5, members.Count);
                }
            }
        }

        [Test]
        public void FullApiTest()
        {
            var mock = new Mock<IApiExtractorFactory>();
            var extractor = new Mock<IApiExtractor>();
            mock.Setup(x => x.BuildExtractor()).Returns(extractor.Object);
            var loader = new AssemblyLoader(mock.Object);
            var api = loader.ExtractApi("myAssembly.dll");
            var api2 = loader.ExtractApi("myAssembly2.dll");
            var comparer = new AssemblyApiComparer();
            var comparison = comparer.CompareApi(api, api2);
            Assert.IsNotNull(comparison.ToString());
        }
    }

    public class MyClass
    {
    }
}