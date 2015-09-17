using Lx.Tools.Projects.Reference;
using Lx.Tools.Tests.MockUnity;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.References
{
    [TestFixture]
    public class ReferenceManagerTest : MockUnitTestFixture
    {
        [Test]
        public void UsageTest()
        {
            var manager = Container.Resolve<ReferenceManager>();
            Assert.IsNotNull(manager);
            manager.Run(new string[] { ReferenceOptions.AddReference.Name, "mycsprog.csproj", "System.Data" });
        }
    }

}
