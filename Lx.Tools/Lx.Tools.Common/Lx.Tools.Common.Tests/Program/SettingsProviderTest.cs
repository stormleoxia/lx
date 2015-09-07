using Lx.Tools.Common.Program;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests.Program
{
    [TestFixture]
    public class SettingsProviderTest
    {
        [Test]
        public void UsageTest()
        {
            ISettingsProvider provider = new SettingsProvider();
            var value = provider.GetSettings("MySetting");
            Assert.AreEqual("MyValue", value);
        }
    }
}
