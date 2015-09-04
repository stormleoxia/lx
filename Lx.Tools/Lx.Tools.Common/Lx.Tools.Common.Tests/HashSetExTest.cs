using System.Collections.Generic;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests
{
    [TestFixture]
    public class HashSetExTest
    {
        [Test]
        public void UseCaseTest()
        {
            var list = new List<string> {"toto", "toto", "tutu"};
            var hash = list.ToHashSet();
            Assert.IsNotNull(hash);
            Assert.IsNotEmpty(hash);
            Assert.AreEqual(2, hash.Count);
            Assert.IsTrue(hash.Contains("tutu"));
            Assert.IsTrue(hash.Contains("toto"));
            Assert.IsFalse(hash.Contains("totu"));
        }
    }
}