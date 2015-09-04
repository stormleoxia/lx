using System.Linq;
using System.Reflection;
using Lx.Tools.Common.Assemblies;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests.Assemblies
{
    [TestFixture]
    public class ApiExtractorTest
    {
        [Test]
        public void ApiExtractionShouldExtractSomethingTest()
        {
            var loader = new Mock<IAssemblyLoader>();
            loader.Setup(x => x.LoadFrom(It.IsAny<string>())).Returns(Assembly.GetExecutingAssembly());
            var extractor = new ApiExtractor();
            extractor.Loader = loader.Object;
            var api = extractor.ExtractApi("");
            Assert.IsNotNull(api);
            Assert.IsNotNull(api.GetNamespaces());
            var namespaces = api.GetNamespaces().ToArray();
            Assert.Greater(namespaces.Length, 2);
            extractor.Dispose();
        }
    }
}