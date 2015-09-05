using NUnit.Framework;

namespace Lx.Tools.Tests.Reflection.Tests
{
    [TestFixture]
    public class FieldExtensionTest
    {
        [Test]
        public void SetFieldWithStringTest()
        {
            var container = new DummyContainer();
            container.SetField("_myField", "FieldValue2");
            Assert.AreEqual("FieldValue2", container.MyField);
        }

        [Test]
        public void SetFieldWithClassTest()
        {
            var container = new DummyContainer();
            var newInstance = new Implementation();
            container.SetField("_instance", newInstance);
            Assert.AreEqual(newInstance, container.Instance);
        }
    }
}