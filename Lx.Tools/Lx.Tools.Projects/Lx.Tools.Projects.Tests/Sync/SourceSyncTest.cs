using System;
using System.Threading;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.Sync
{
    [TestFixture]
    public class SourceSyncTest
    {
        [Test]
        public void MainTest()
        {
            int iocp;
            int wt;
            ThreadPool.GetMinThreads(out wt, out iocp);
            var pc = Environment.ProcessorCount;
            Console.WriteLine("MIN NBT:{0} / WT:{1} / IOCP:{2}", pc, wt/pc, iocp/pc);
            ThreadPool.GetMaxThreads(out wt, out iocp);
            Console.WriteLine("MAX NBT:{0} / WT:{1} / IOCP:{2}", pc, wt/pc, iocp/pc);
            ThreadPool.GetAvailableThreads(out wt, out iocp);
            Console.WriteLine("AVB NBT:{0} / WT:{1} / IOCP:{2}", pc, wt/pc, iocp/pc);
        }
    }
}