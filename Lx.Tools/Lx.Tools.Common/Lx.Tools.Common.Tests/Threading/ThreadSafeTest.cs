using Lx.Tools.Common.Threading;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests.Threading
{
    [TestFixture]
    public class ThreadSafeTest
    {
        [Test]
        public void UsageTest()
        {
            var tsf = new ThreadSafe<MyClass>();
            tsf.Execute(x => x.Increment());
            Assert.IsNotNull(tsf.InnerInstance);
            Assert.AreEqual(1, tsf.InnerInstance.Counter);
            tsf.Execute(x => x.Increment());
            Assert.IsNotNull(tsf.InnerInstance);
            Assert.AreEqual(2, tsf.InnerInstance.Counter);
        }
    }

    public class MyClass
    {
        public int Counter { get; private set; }

        public void Increment()
        {
            Counter++;
        }
    }
}
