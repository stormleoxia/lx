using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    public sealed class ProjectFactory : IProjectFactory
    {
        private readonly IConsole _console;
        private readonly IFileSystem _fileSystem;
        private readonly IDictionary<string, IProject> _projects = new Dictionary<string, IProject>();

        public ProjectFactory(IConsole console, IFileSystem fileSystem)
        {
            _console = console;
            _fileSystem = fileSystem;
        }

        public bool IsValidProject(string projectPath)
        {
            try
            {
                CreateProject(projectPath);
                var directory = Path.GetDirectoryName(projectPath);
                var files = _fileSystem.GetFiles(directory, "*.sources", SearchOption.TopDirectoryOnly);
                return files.Length > 0;
            }
            catch (Exception e)
            {
                _console.WriteLine(e);
            }
            return false;
        }

        public IProjectItemsProvider CreateProjectItemsProvider(string projectPath)
        {
            return new ProjectItemsProvider(CreateProject(projectPath));
        }

        public IProjectUpdater CreateProjectUpdater(string projectPath)
        {
            return new ProjectUpdater(CreateProject(projectPath), _fileSystem);
        }

        public ISourcesProvider CreateSourcesProvider(string projectPath)
        {
            return new SourcesProvider(projectPath, _fileSystem, _console);
        }

        public ISourceComparer CreateSourceComparer()
        {
            return new SourceComparer();
        }

        internal IProject CreateProject(string projectPath)
        {
            IProject project;
            if (!_projects.TryGetValue(projectPath, out project))
            {
                project = new ProjectWrapper(projectPath);
                _projects[projectPath] = project;
            }
            return project;
        }
    }
}