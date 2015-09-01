using System.IO;
using Lx.Tools.Common.Wrappers;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests.Wrappers
{
    [TestFixture]
    public class FileSystemTest
    {
        private string fileName = @"Lx.Tools.Common.Tests.csproj";
        private string notExisting = @"Xldkfjsldkfj.dsmflkdmsf";

        [Test]
        public void FileInfoVerifyAssertions()
        {
            var curDirectory = Directory.GetCurrentDirectory();
            var existingFile = Path.Combine(curDirectory, "../../" + fileName);
            var fileInfo = new FileInfo(existingFile);
            Assert.IsFalse(fileInfo.FullName.Contains(".."));
            Assert.AreEqual(fileInfo.FullName, new FileSystem().ResolvePath(existingFile));
        }

        [Test]
        public void FileExists()
        {
            var curDirectory = Directory.GetCurrentDirectory();
            var existingFile = Path.Combine(curDirectory, "../../" + fileName);
            var fileSystem = new FileSystem();
            Assert.IsTrue(fileSystem.FileExists(existingFile));
            var notExistingFile = Path.Combine(curDirectory, "../../" + notExisting);
            Assert.IsFalse(fileSystem.FileExists(notExistingFile));
        }

        [Test]
        public void DirectoryExists()
        {
            var curDirectory = Directory.GetCurrentDirectory();
            var existingFile = Path.Combine(curDirectory, "../../");
            var fileSystem = new FileSystem();
            Assert.IsTrue(fileSystem.DirectoryExists(existingFile));
            var notExistingFile = Path.Combine(curDirectory, "../../" + notExisting + "/");
            Assert.IsFalse(fileSystem.DirectoryExists(notExistingFile));
        }
    }
}
