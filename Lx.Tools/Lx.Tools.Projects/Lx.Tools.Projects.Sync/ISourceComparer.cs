using System.Collections.Generic;

namespace Lx.Tools.Projects.Sync
{
    public interface ISourceComparer
    {
        SourceComparison Compare(HashSet<string> items, HashSet<string> sources);
    }
}