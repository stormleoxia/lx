using System.Collections;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace Lx.Tools.Tests.MockUnity.Tests
{
    [TestFixture]
    public class MockUnitExtensionTest : MockUnitTestFixture
    {
        [Test]
        public void AddExtensionTest()
        {
            var container = new UnityContainer();
            container.AddExtension(MockUnity.MockUnit.Extension);
            var list = container.Resolve<IList>();
            Assert.IsNotNull(list);
        }

        [Test, ExpectedException(typeof(ResolutionFailedException))]
        public void RemoveExtensionTest()
        {
            var container = new UnityContainer();
            container.AddExtension(MockUnity.MockUnit.Extension);
            container.RemoveAllExtensions();
            // Re add all extensions present before remove all. 
            // Beware though because it could change in future versions of Unity.
            container.AddExtension(new InjectedMembers());
            container.AddExtension(new UnityDefaultBehaviorExtension());
            container.AddExtension(new UnityDefaultStrategiesExtension());
            var resolved = container.Resolve<IList>();
            Assert.IsNull(resolved);
        }
    }

}
