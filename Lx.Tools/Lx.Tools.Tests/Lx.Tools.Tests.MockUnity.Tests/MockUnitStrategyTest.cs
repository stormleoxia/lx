using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Tests.MockUnity.Tests
{
    [TestFixture]
    public class MockUnitStrategyTest : MockUnitTestFixture
    {
        [Test]
        public void ResolveLooseInterfaceTest()
        {
            var container = new UnityContainer();
            container.AddExtension(MockUnity.MockUnit.Extension);
            var list = container.Resolve<IList>();
            Assert.IsFalse(list.Contains(string.Empty));
        }

        [Test, ExpectedException(typeof(MockException))]
        public void ResolveStrictInterfaceTest()
        {
            var container = new UnityContainer();
            MockUnity.MockUnit.Extension.Behavior = MockBehavior.Strict;
            container.AddExtension(MockUnity.MockUnit.Extension);
            var list = container.Resolve<IList>();
            var res = list.Contains(string.Empty);
        }

        [Test]
        public void ResolveLooseInterfaceWithSetupTest()
        {
            var container = new UnityContainer();
            container.AddExtension(MockUnity.MockUnit.Extension);
            Mock<IList> mock = MockUnit.Get<IList>();
            mock.Setup(x => x.Contains(It.IsAny<string>())).Returns(true);
            var list = container.Resolve<IList>();
            Assert.IsTrue(list.Contains(string.Empty));
        }

        [Test, Ignore("Feature to add")]
        public void ResolveOnRegisteredTypeShouldNotBeMocked()
        {
            var container = new UnityContainer();
            container.AddExtension(MockUnit.Extension);
            container.RegisterType<IMyInterface, MyClass>(new ContainerControlledLifetimeManager());
            var resolved = container.Resolve<IMyInterface>();
            Assert.IsNotNull(resolved);
            Assert.AreEqual(typeof(MyClass), resolved.GetType());
        }
    }

    public class MyClass : IMyInterface
    {
    }

    public interface IMyInterface
    {
    }
}
