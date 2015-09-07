using Lx.Tools.Common.Program;
using Lx.Tools.Projects.Sync;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.Sync
{
    [TestFixture]
    public class ProjectSyncConfigurationTest
    {
        [Test]
        public void UsageTest()
        {
            var mock = new Mock<ISettingsProvider>(MockBehavior.Strict);
            mock.Setup(x => x.GetSettings("IgnoreDirectories")).Returns("ignored");
            IProjectSyncConfiguration projectSyncConfiguration = new ProjectSyncConfiguration(mock.Object);
            string[] ignored = projectSyncConfiguration.IgnoredDirectories;
            Assert.IsNotNull(ignored);
            Assert.IsNotEmpty(ignored);
            Assert.AreEqual(1, ignored.Length);
            Assert.AreEqual("ignored", ignored[0]);
        }

        [Test]
        public void ListOfCommaSeparatedDirectoriesTest()
        {
            var mock = new Mock<ISettingsProvider>(MockBehavior.Strict);
            mock.Setup(x => x.GetSettings("IgnoreDirectories")).Returns("ignored, another");
            IProjectSyncConfiguration projectSyncConfiguration = new ProjectSyncConfiguration(mock.Object);
            string[] ignored = projectSyncConfiguration.IgnoredDirectories;
            Assert.IsNotNull(ignored);
            Assert.IsNotEmpty(ignored);
            Assert.AreEqual(2, ignored.Length);
            Assert.AreEqual("ignored", ignored[0]);
            Assert.AreEqual("another", ignored[1]);
        }
    }
}
