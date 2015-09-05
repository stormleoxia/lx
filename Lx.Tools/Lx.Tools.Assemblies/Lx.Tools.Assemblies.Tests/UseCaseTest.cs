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
			myNamespace.AddType (typeof(MyClass));
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