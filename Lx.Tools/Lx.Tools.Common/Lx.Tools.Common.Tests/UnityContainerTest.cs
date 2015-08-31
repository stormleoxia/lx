using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests
{
    [TestFixture]
    public class UnityContainerTest
    {
        [Test]
        public void RegisterTypeWithStrongCheckedInheritance()
        {
            var realContainer = new UnityContainer();
            Mock<IUnityContainer> container = new Mock<IUnityContainer>();
            container.Setup(
                x => x.RegisterType(typeof (IMyInterface), typeof (MyClass), (string) null, (LifetimeManager) null,
                    new InjectionMember[] {}))
                .Callback(() => realContainer.RegisterType(typeof (IMyInterface), typeof (MyClass)));
            container.Object.Register<IMyInterface, MyClass>();
            var resolved = realContainer.Resolve<IMyInterface>();
            container.Setup(x => x.Resolve(typeof (IMyInterface), (string) null, new ResolverOverride[] {}))
                .Returns(resolved);

            var instance = container.Object.Resolve<IMyInterface>();
            Assert.IsNotNull(instance);
            Assert.IsInstanceOf<MyClass>(instance);
        }
    }

    public class MyClass : IMyInterface
    {
        
    }

    public interface IMyInterface
    {

    }
}
