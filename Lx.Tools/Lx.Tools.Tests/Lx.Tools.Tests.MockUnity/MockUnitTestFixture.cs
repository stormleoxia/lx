using NUnit.Framework;

namespace Lx.Tools.Tests.MockUnity
{
    public class MockUnitTestFixture
    {
        [SetUp]
        public virtual void Setup()
        {
            MockUnit.Setup();
        }

        [TearDown]
        public virtual void TearDown()
        {
            MockUnit.TearDown();
        }
    }

}
