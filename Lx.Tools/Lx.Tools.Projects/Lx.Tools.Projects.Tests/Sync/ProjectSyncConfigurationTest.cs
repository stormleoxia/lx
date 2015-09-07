using Lx.Tools.Projects.Sync;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.Sync
{
    [TestFixture]
    public class ProjectSyncConfigurationTest
    {
        [Test]
        public void UsageTest()
        {
            IProjectSyncConfiguration projectSyncConfiguration = new ProjectSyncConfiguration();
            string[] ignored = projectSyncConfiguration.IgnoredDirectories;
            Assert.IsNotNull(ignored);
            Assert.IsNotEmpty(ignored);
            Assert.AreEqual(1, ignored.Length);
            Assert.AreEqual("ignored", ignored[0]);
        }
    }
}
