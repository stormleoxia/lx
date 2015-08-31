using System.Collections.Generic;
using System.IO;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    public class DirectorySync
    {
        private readonly List<ProjectSync> _projects = new List<ProjectSync>();

        public DirectorySync(string directoryPath, ISyncFactory factory, IFileSystem fileSystem)
        {
            var files = fileSystem.GetFiles(directoryPath, "*.csproj", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var target = TargetsEx.ExtractTarget(file);
                var project = factory.CreateProjectSynchronizer(file, target);
                _projects.Add(project);
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