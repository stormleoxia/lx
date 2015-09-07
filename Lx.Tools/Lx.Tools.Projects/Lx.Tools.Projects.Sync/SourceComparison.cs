using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lx.Tools.Projects.Sync
{
    public class SourceComparison
    {
        private readonly List<ComparisonResult> _results = new List<ComparisonResult>();

        public IEnumerable<MissingFileInProject> MissingFilesInProject
        {
            get { return _results.Where(x => x is MissingFileInProject).Cast<MissingFileInProject>().ToArray(); }
        }

        public IEnumerable<MissingFileInSource> MissingFilesInSource
        {
            get { return _results.Where(x => x is MissingFileInSource).Cast<MissingFileInSource>().ToArray(); }
        }

        public void Add(ComparisonResult missing)
        {
            _results.Add(missing);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var result in _results)
            {
                builder.AppendLine(result.ToString());
            }
            return builder.ToString();
        }
    }

    public class ComparisonResult
    {
        private readonly string _item;

        public ComparisonResult(string item)
        {
            _item = item;
        }

        public string Path
        {
            get { return _item; }
        }
    }
}