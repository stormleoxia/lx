using System.Collections.Generic;
using System.Linq;
using Lx.Tools.Common;

namespace Lx.Tools.Projects.Sync
{
    public class SourceComparer : ISourceComparer
    {
        public SourceComparison Compare(HashSet<string> itemsInCsProj, HashSet<string> sourcesInSourceFile)
        {
            var pathInCsProj = itemsInCsProj.ToDictionaryIgnoreDuplicates(x => x.ToLower().ToPlatformPath().Trim().RemoveDotPath(), x => x);
            var pathInSourceFile = sourcesInSourceFile.ToDictionaryIgnoreDuplicates(x => x.ToLower().ToPlatformPath().Trim().RemoveDotPath(), x => x);
            var comparison = new SourceComparison();
            foreach (var item in pathInCsProj)
            {
                if (!pathInSourceFile.ContainsKey(item.Key))
                {
                    comparison.Add(new MissingFileInSource(item.Value));
                }
            }
            foreach (var source in pathInSourceFile)
            {
                if (!pathInCsProj.ContainsKey(source.Key))
                {
                    comparison.Add(new MissingFileInProject(source.Value));
                }
            }
            return comparison;
        }
    }
}