using System.Collections.Generic;
using System.IO;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    public class DirectorySync : ISynchronizer
    {
        private readonly List<ISynchronizer> _projects = new List<ISynchronizer>();

        public DirectorySync(string directoryPath, ISyncFactory factory, IFileSystem fileSystem, IDirectoryValidator validator, IProjectFactory projectFactory)
        {
            var files = fileSystem.GetFiles(directoryPath, "*.csproj", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (validator.IsDirectoryValid(file) && projectFactory.IsValidProject(file))
                {
                    var project = factory.CreateProjectSynchronizer(file);
                    _projects.Add(project);
                }
            }
        }

        public void Synchronize()
        {
            foreach (var project in _projects)
            {
                project.Synchronize();
            }
        }
    }
}