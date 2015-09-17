using System;
using Moq;

namespace Lx.Tools.Tests.MockUnity
{
    public class MockUnit
    {
        private readonly MockFactory _factory;

        public static void Setup()
        {
            Instance = new MockUnit();
        }

        private MockUnit()
        {
            _factory = new MockFactory();
            InnerExtension = new MockUnitExtension();
        }

        private static MockUnit Instance { get; set; }

        public static MockUnitExtension Extension
        {
            get
            {
                if (Instance == null)
                {
                    throw new InvalidOperationException("Call MoqInject.Setup() or Use " + typeof(MockUnitTestFixture));
                }
                return Instance.InnerExtension;
            }
        }

        private MockUnitExtension InnerExtension { get; set; }

        public static void TearDown()
        {
            Instance.VerifyAll();
            Instance = null;
        }

        private void VerifyAll()
        {
            foreach (var instance in _factory.Mocks)
            {
                instance.VerifyAll();
            }
        }

        public static Mock<T> Get<T>() where T : class
        {
            return (Mock<T>)Get(typeof(T));
        }

        public static Mock Get(Type type)
        {
            if (Instance == null)
            {
                throw new InvalidOperationException("Call MoqInject.Setup() or Use " + typeof (MockUnitTestFixture));
            }
            return Instance.InnerGet(type);
        }

        private Mock InnerGet(Type type)
        {
            return _factory.Get(type, InnerExtension.Behavior);
        }
    }
}