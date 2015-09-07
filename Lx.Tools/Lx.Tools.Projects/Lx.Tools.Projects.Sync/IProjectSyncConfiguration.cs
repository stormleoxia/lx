using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
