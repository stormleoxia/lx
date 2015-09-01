using System.Collections.Generic;

namespace Lx.Tools.Projects.Sync
{
    public interface ISourcesProvider
    {
        HashSet<string> GetFiles();
    }
}