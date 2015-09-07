using System.IO;
using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.Sync;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.Sync
{
    [TestFixture]
    public class SyncFactoryTest
    {
        private Mock<IProjectFactory> _projectFactory;
        private Mock<IFileSystem> _fileSystem;
        private Mock<IConsole> _console;
        private Mock<IDirectoryValidator> _validator;

        [SetUp]
        public void Setup()
        {            
            _projectFactory = new Mock<IProjectFactory>(MockBehavior.Strict);
            _console = new Mock<IConsole>(MockBehavior.Strict);
            _fileSystem = new Mock<IFileSystem>(MockBehavior.Strict);
            _validator = new Mock<IDirectoryValidator>(MockBehavior.Strict);
        }

        [TearDown]
        public void TearDown()
        {
            _validator.VerifyAll();            
            _projectFactory.VerifyAll();
            _console.VerifyAll();
            _fileSystem.VerifyAll();
        }

        [Test]
        public void CreateDirectorySyncTest()
        {

            _fileSystem.Setup(x => x.GetFiles("directoryPath", "*.csproj", SearchOption.AllDirectories)).Returns(new string[]{"file1.csproj", "file2.csproj"});
            _validator.Setup(x => x.IsDirectoryValid("file1.csproj")).Returns(true);
            _validator.Setup(x => x.IsDirectoryValid("file2.csproj")).Returns(true);
            _projectFactory.Setup(x => x.IsValidProject("file1.csproj")).Returns(true);
            _projectFactory.Setup(x => x.IsValidProject("file2.csproj")).Returns(true);
            SyncFactory factory = new SyncFactory(_projectFactory.Object, _console.Object, _fileSystem.Object, _validator.Object);
            var sync = factory.CreateDirectorySynchronizer("directoryPath");
            Assert.IsNotNull(sync);
        }

        [Test]
        public void CreateProjecSyncTest()
        {
            SyncFactory factory = new SyncFactory(_projectFactory.Object, _console.Object, _fileSystem.Object, _validator.Object);
            var sync = factory.CreateProjectSynchronizer("filePath");
            Assert.IsNotNull(sync);
        }
    }
}
