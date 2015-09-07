using System.Configuration;

namespace Lx.Tools.Projects.Sync
{
    public class ProjectSyncConfiguration : IProjectSyncConfiguration
    {
        public ProjectSyncConfiguration()
        {
            var ignored = ConfigurationManager.AppSettings["IgnoreDirectories"];
            IgnoredDirectories = string.IsNullOrEmpty(ignored) ? new string[]{} : ignored.Split(',');
        }

        /// <summary>
        /// Gets the ignored directories pattern.
        /// </summary>
        /// <value>
        /// The ignored directories pattern.
        /// </value>
        public string[] IgnoredDirectories { get; private set; }
    }
}
