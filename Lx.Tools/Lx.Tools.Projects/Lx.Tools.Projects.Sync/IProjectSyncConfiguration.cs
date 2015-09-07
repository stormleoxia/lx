using System.Collections.Generic;
using Lx.Tools.Common.Program;

namespace Lx.Tools.Projects.Sync
{
    public interface IProjectSyncConfiguration
    {
        /// <summary>
        /// Gets the ignored directories pattern.
        /// </summary>
        /// <value>
        /// The ignored directories pattern.
        /// </value>
        string[] IgnoredDirectories { get; }

        /// <summary>
        /// Gets or sets the options activated for the run of this program.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        HashSet<Option> Options { get; set; }

    }
}
