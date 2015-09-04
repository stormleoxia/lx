using System.IO;
using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.Sync;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.Sync
{
    [TestFixture]
    public class DirectorySyncTest
    {
        [Test]
        public void SynchronizeTest()
        {
            var fileSystem = new Mock<IFileSystem>(MockBehavior.Strict);
            var fileNet45 = "file1-net_4_5";
            var fileAll = "file2";
            string[] array = {fileNet45, fileAll};
            fileSystem.Setup(x => x.GetFiles("path", "*.csproj", SearchOption.AllDirectories)).Returns(array);
            var factory = new Mock<ISyncFactory>(MockBehavior.Strict);
            var synchronizer45 = new Mock<ISynchronizer>(MockBehavior.Strict);
            synchronizer45.Setup(x => x.Synchronize());
            var synchronizerAll = new Mock<ISynchronizer>(MockBehavior.Strict);
            synchronizerAll.Setup(x => x.Synchronize());
            factory.Setup(x => x.CreateProjectSynchronizer(fileNet45, Targets.Net4Dot5)).Returns(synchronizer45.Object);
            factory.Setup(x => x.CreateProjectSynchronizer(fileAll, Targets.All)).Returns(synchronizerAll.Object);

            var directorySync = new DirectorySync("path", factory.Object, fileSystem.Object);
            directorySync.Synchronize();
        }
    }
}