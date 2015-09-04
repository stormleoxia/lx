using System.Collections.Generic;
using System.Linq;
using Lx.Tools.Common.Assemblies;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests.Assemblies
{
    [TestFixture]
    public class AssemblyApiTest
    {
        [Test]
        public void AssemblyApiWithEmptyList()
        {
            var api = new AssemblyApi(new List<NamespaceDefinition>());
            Assert.IsNotNull(api.GetNamespaces());
            Assert.AreEqual(0, api.GetNamespaces().Count());
        }

        [Test]
        public void AssemblyApiWithOneNamespace()
        {
            var api = new AssemblyApi(new List<NamespaceDefinition> {new NamespaceDefinition("MyNamespace")});
            Assert.IsNotNull(api.GetNamespaces());
            Assert.AreEqual(1, api.GetNamespaces().Count());
            Assert.AreEqual("MyNamespace", api.GetNamespaces().First().Name);
        }

        [Test]
        public void AssemblyApiWithTwoNamespaces()
        {
            var api =
                new AssemblyApi(new List<NamespaceDefinition>
                {
                    new NamespaceDefinition("MyNamespace"),
                    new NamespaceDefinition("MyOtherNameSpace")
                });
            Assert.IsNotNull(api.GetNamespaces());
            Assert.AreEqual(2, api.GetNamespaces().Count());
            Assert.AreEqual("MyNamespace", api.GetNamespaces().First().Name);
        }
    }
}