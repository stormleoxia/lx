using Lx.Tools.Projects.Reference;
using Lx.Tools.Tests.MockUnity;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.References
{
    [TestFixture]
    public class ReferenceManagerTest : MockUnitTestFixture
    {
        private UnityContainer _container;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _container = new UnityContainer();
        }

        [Test]
        public void UsageTest()
        {
            _container.AddExtension(MockUnit.Extension);
            var manager = _container.Resolve<ReferenceManager>();
            Assert.IsNotNull(manager);
            manager.Run(new string[] { ReferenceOptions.AddReference.Name, "mycsprog.csproj", "System.Data" });
        }
    }

}
