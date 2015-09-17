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
            var list = Container.Resolve<IList>();
            Assert.IsNotNull(list);
        }

        [Test, ExpectedException(typeof(ResolutionFailedException))]
        public void RemoveExtensionTest()
        {
            Container.RemoveAllExtensions();
            // Re add all extensions present before remove all. 
            // Beware though because it could change in future versions of Unity.
            Container.AddExtension(new InjectedMembers());
            Container.AddExtension(new UnityDefaultBehaviorExtension());
            Container.AddExtension(new UnityDefaultStrategiesExtension());
            var resolved = Container.Resolve<IList>();
            Assert.IsNull(resolved);
        }
    }

}
