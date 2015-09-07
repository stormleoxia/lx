using System.IO;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests
{
    [TestFixture]
    public class StringExTest
    {
        [Test]
        public void Test()
        {
            Assert.AreEqual("/usr/lib/myfile.lib".Replace('/', Path.DirectorySeparatorChar), "/usr/lib/myfile.lib".ToPlatformPath());
            Assert.AreEqual(@"\usr\lib\myfile.lib".Replace('\\', Path.DirectorySeparatorChar), @"\usr\lib\myfile.lib".ToPlatformPath());
        }
    }
}
