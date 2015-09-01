using System.Collections.Generic;
using System.Linq;
using Lx.Tools.Common;

namespace Lx.Tools.Projects.Sync
{
    public class SourceComparer : ISourceComparer
    {
        public SourceComparison Compare(HashSet<string> itemsInCsProj, HashSet<string> sourcesInSourceFile)
        {
            var pathInCsProj = itemsInCsProj.Select(x => x.ToLower()).ToHashSet();
            var pathInSourceFile = sourcesInSourceFile.Select(x => x.ToLower()).ToHashSet();
            var comparison = new SourceComparison();
            foreach (var item in pathInCsProj)
            {
                if (!pathInSourceFile.Contains(item))
                {
                    comparison.Add(new MissingFileInSource(item));
                }
            }
            foreach (var source in pathInSourceFile)
            {
                if (!pathInCsProj.Contains(source))
                {
                    comparison.Add(new MissingFileInProject(source));
                }
            }
            return comparison;
        }
    }
}