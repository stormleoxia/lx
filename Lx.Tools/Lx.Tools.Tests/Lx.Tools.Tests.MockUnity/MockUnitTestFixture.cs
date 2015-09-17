using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace Lx.Tools.Tests.MockUnity
{
    public class MockUnitTestFixture
    {
        public IUnityContainer Container { get; private set; }

        [SetUp]
        public virtual void Setup()
        {
            Container = new UnityContainer();
            MockUnit.Setup();
            Container.AddExtension(MockUnit.Extension);
        }

        [TearDown]
        public virtual void TearDown()
        {
            MockUnit.TearDown();
        }
    }

}
