using System.IO;
using System.Linq;
using Lx.Tools.Common;

namespace Lx.Tools.Projects.Sync
{
    public class ProjectUpdater
    {
        private readonly IProject _project;

        public ProjectUpdater(IProject project)
        {
            _project = project;
        }

        public void Update(SourceComparison comparison)
        {
            var directory = Path.GetDirectoryName(_project.FullPath);

            foreach (var item in comparison.MissingFilesInProject)
            {
                var fileName = Path.Combine(directory, item.Path);
                if (File.Exists(fileName))
                {
                    var fileInfo = new FileInfo(fileName);
                    var path = new UPath(directory);
                    var filePath = new UPath(fileInfo.FullName);
                    var res = path.MakeRelativeUPath(filePath);
                    _project.AddItem("Compile", res.ToString());
                }
            }
            var hash = comparison.MissingFilesInSource.Select(x => x.Path.ToLower()).ToHashSet();
            var items = _project.GetItems("Compile").ToArray();
            foreach (var item in items)
            {
                if (hash.Contains(item.EvaluatedInclude.Replace('\\', '/').ToLower()))
                {
                    _project.RemoveItem(item);
                }
            }
            //_project.Save();
        }
    }
}