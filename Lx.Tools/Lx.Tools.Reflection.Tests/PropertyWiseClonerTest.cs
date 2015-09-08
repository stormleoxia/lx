using NUnit.Framework;

namespace Lx.Tools.Reflection.Unit.Tests
{
    [TestFixture]
    public class PropertyWiseClonerTest
    {
        [Test]
        public void UsageTest()
        {
            MyPropertyContainer container = new MyPropertyContainer
            {
                IntegerProperty = 10,
                StringProperty = "MyValue"
            };
            var clone = container.PropertyWiseClone();
            Assert.IsNotNull(clone);
            Assert.AreEqual(container.IntegerProperty, clone.IntegerProperty);
            Assert.AreEqual(container.StringProperty, clone.StringProperty);
            var clone2 = container.PropertyWiseClone();
            Assert.IsNotNull(clone);
            Assert.AreEqual(container.IntegerProperty, clone.IntegerProperty);
            Assert.AreEqual(container.StringProperty, clone.StringProperty);
            Assert.IsFalse(object.ReferenceEquals(clone, clone2));
        }

    }

    public class MyPropertyContainer
    {
        public string StringProperty { get; set; }
        public int IntegerProperty { get; set; }
    }
}

