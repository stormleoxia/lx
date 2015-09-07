using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Lx.Tools.Common.Program;

namespace Lx.Tools.Projects.Sync
{
    public class ProjectSyncConfiguration : IProjectSyncConfiguration
    {
        public ProjectSyncConfiguration(ISettingsProvider provider)
        {
            var ignored = provider.GetSettings("IgnoreDirectories");
            IgnoredDirectories = string.IsNullOrEmpty(ignored) ? new string[]{} : ignored.Split(',').Select(x => x.Trim()).ToArray();
        }

        /// <summary>
        /// Gets the ignored directories pattern.
        /// </summary>
        /// <value>
        /// The ignored directories pattern.
        /// </value>
        public string[] IgnoredDirectories { get; private set; }

        /// <summary>
        /// Gets or sets the options activated for the run of this program.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public HashSet<Option> Options { get; set; }
    }
}
