using System.IO;
using System.Linq;
using Lx.Tools.Common;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    public class ProjectUpdater : IProjectUpdater
    {
        private readonly IFileSystem _fileSystem;
        private readonly IProject _project;

        public ProjectUpdater(IProject project, IFileSystem fileSystem)
        {
            _project = project;
            _fileSystem = fileSystem;
        }

        public void Update(SourceComparison comparison)
        {
            var directory = Path.GetDirectoryName(_project.FullPath);

            foreach (var item in comparison.MissingFilesInProject)
            {
                var fileName = Path.Combine(directory, item.Path);
                var filePath = _fileSystem.ResolvePath(fileName);
                if (filePath != null)
                {
                    var path = new UPath(directory);
                    var fileUPath = new UPath(filePath);
                    var res = path.MakeRelativeUPath(fileUPath);
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