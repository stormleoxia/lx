using System.Collections.Generic;

namespace Lx.Tools.Projects.Sync
{
    public interface ISourceFinder
    {
        void FindSourcesFile();
        HashSet<string> GetFiles();
    }
}