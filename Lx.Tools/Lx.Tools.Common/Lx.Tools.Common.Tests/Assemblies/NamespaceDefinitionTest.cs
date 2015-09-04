using System.Linq;
using Lx.Tools.Common.Assemblies;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests.Assemblies
{
    [TestFixture]
    public class NamespaceDefinitionTest
    {
        [Test]
        public void NamespaceUsageTest()
        {
            var type = GetType();
            var namespaceDefinition = new NamespaceDefinition();
            namespaceDefinition.Name = "MyName";
            Assert.AreEqual("MyName", namespaceDefinition.Name);
            namespaceDefinition.AddType(type);
            Assert.AreEqual(1, namespaceDefinition.Types.Count);
            Assert.AreEqual(1, namespaceDefinition.GetTypes().Count());
            Assert.IsNotNull(namespaceDefinition.GetTypes().First());
            Assert.IsNotNull(namespaceDefinition.Types.First());
            var textSerialization = namespaceDefinition.ToString();
            Assert.IsNotNull(textSerialization);
            Assert.IsTrue(textSerialization.Contains(type.FullName));
        }
    }
}