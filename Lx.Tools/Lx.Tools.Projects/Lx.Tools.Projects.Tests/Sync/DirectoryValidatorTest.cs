using Lx.Tools.Projects.Sync;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.Sync
{
    [TestFixture]
    public class DirectoryValidatorTest
    {
        private readonly Mock<IProjectSyncConfiguration> _configuration = new Mock<IProjectSyncConfiguration>(MockBehavior.Strict);

        [Test]
        public void UsageTest()
        {
            _configuration.Setup(x => x.IgnoredDirectories).Returns(new string[] { "ignored", @"a\b", "/b/d/"});
            DirectoryValidator validator = new DirectoryValidator(_configuration.Object);
            Assert.IsTrue(validator.IsDirectoryValid("my/valid/Directory"));
            Assert.IsFalse(validator.IsDirectoryValid("my/a/b/Directory"));
            Assert.IsFalse(validator.IsDirectoryValid(@"my/a\b/Directory"));
            Assert.IsFalse(validator.IsDirectoryValid(@"my/ignored/Directory"));
            Assert.IsFalse(validator.IsDirectoryValid(@"myignoredDirectory"));
            Assert.IsTrue(validator.IsDirectoryValid(@"myb/dDirectory"));
        }
    }
}
