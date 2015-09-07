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
        private Mock<IFileSystem> _fileSystem;
        private Mock<IDirectoryValidator> _validator;
        private Mock<ISyncFactory> _factory;
        private Mock<ISynchronizer> _synchronizer45;
        private Mock<ISynchronizer> _synchronizerAll;

        [SetUp]
        public void Setup()
        {
            _factory = new Mock<ISyncFactory>(MockBehavior.Strict);
            _fileSystem = new Mock<IFileSystem>(MockBehavior.Strict);
            _validator = new Mock<IDirectoryValidator>(MockBehavior.Strict);
            _synchronizer45 = new Mock<ISynchronizer>(MockBehavior.Strict);
            _synchronizerAll = new Mock<ISynchronizer>(MockBehavior.Strict);
            _validator = new Mock<IDirectoryValidator>(MockBehavior.Strict);
        }

        [TearDown]
        public void TearDown()
        {
            _factory.VerifyAll();
            _validator.VerifyAll();
            _fileSystem.VerifyAll();
            _validator.VerifyAll();
            _synchronizer45.VerifyAll();
            _synchronizerAll.VerifyAll();
        }

        [Test]
        public void SynchronizeTest()
        {
            var fileNet45 = "file1-net_4_5";
            var fileAll = "file2";
            string[] array = {fileNet45, fileAll};
            _fileSystem.Setup(x => x.GetFiles("path", "*.csproj", SearchOption.AllDirectories)).Returns(array);
            _synchronizer45.Setup(x => x.Synchronize());
            _synchronizerAll.Setup(x => x.Synchronize());
            _factory.Setup(x => x.CreateProjectSynchronizer(fileNet45)).Returns(_synchronizer45.Object);
            _factory.Setup(x => x.CreateProjectSynchronizer(fileAll)).Returns(_synchronizerAll.Object);
            _validator.Setup(x => x.IsDirectoryValid("file1-net_4_5")).Returns(true);
            _validator.Setup(x => x.IsDirectoryValid("file2")).Returns(true);
            var directorySync = new DirectorySync("path", _factory.Object, _fileSystem.Object, _validator.Object);
            directorySync.Synchronize();
        }
    }
}