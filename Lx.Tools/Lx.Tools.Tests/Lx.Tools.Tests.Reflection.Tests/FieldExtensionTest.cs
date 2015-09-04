using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lx.Tools.Tests.Reflection.Tests
{
    [TestClass]
    public class FieldExtensionTest
    {
        [TestMethod]
        public void SetFieldWithStringTest()
        {
            var container = new DummyContainer();
            container.SetField("_myField", "FieldValue2");
            Assert.AreEqual("FieldValue2", container.MyField);
        }

        [TestMethod]
        public void SetFieldWithClassTest()
        {
            var container = new DummyContainer();
            var newInstance = new Implementation();
            container.SetField("_instance", newInstance);
            Assert.AreEqual(newInstance, container.Instance);
        }
    }
}