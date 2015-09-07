using System.IO;
using System.Linq;
using Lx.Tools.Common;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    public class ProjectUpdater : IProjectUpdater
    {
        private readonly IFileSystem _fileSystem;
        private readonly IProjectSyncConfiguration _configuration;
        private readonly IProject _project;

        public ProjectUpdater(IProject project, IFileSystem fileSystem, IProjectSyncConfiguration configuration)
        {
            _project = project;
            _fileSystem = fileSystem;
            _configuration = configuration;
        }

        /// <summary>
        /// Add missing items in the project and removes items not found in source file.
        /// Note that there are some files which are generated, so file existence should not be checked.
        /// </summary>
        /// <param name="comparison">The comparison.</param>
        public void Update(SourceComparison comparison)
        {
            var directory = Path.GetDirectoryName(_project.FullPath);

            foreach (var item in comparison.MissingFilesInProject)
            {
                var fileName = Path.Combine(directory, item.Path);
                var path = new UPath(directory);
                var fileUPath = new UPath(fileName);
                var res = path.MakeRelativeUPath(fileUPath);
                _project.AddItem("Compile", res.ToString());
            }
            if (!_configuration.Options.Contains(ProgramOptions.NoDelete))
            {
                var hash = comparison.MissingFilesInSource.Select(x => x.Path.ToLower()).ToHashSet();
                var items = _project.GetItems("Compile").ToArray();
                foreach (var item in items)
                {
                    if (hash.Contains(item.EvaluatedInclude.Replace('\\', '/').ToLower()))
                    {
                        _project.RemoveItem(item);
                    }
                }
            }
            _project.Save();
        }
    }
}