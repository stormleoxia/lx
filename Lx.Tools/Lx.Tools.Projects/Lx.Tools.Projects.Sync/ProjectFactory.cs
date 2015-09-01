using System.Collections.Generic;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    public class ProjectFactory : IProjectFactory
    {
        private readonly IConsole _console;
        private readonly IFileSystem _fileSystem;
        private readonly IDictionary<string, IProject> _projects = new Dictionary<string, IProject>(); 

        public ProjectFactory(IConsole console, IFileSystem fileSystem)
        {
            _console = console;
            _fileSystem = fileSystem;
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

        public IProjectItemsProvider CreateProjectItemsProvider(string projectPath, Targets target)
        {
            return new ProjectItemsProvider(CreateProject(projectPath));
        }

        public IProjectUpdater CreateProjectUpdater(string projectPath)
        {
            return new ProjectUpdater(CreateProject(projectPath), _fileSystem);
        }

        public ISourcesProvider CreateSourcesProvider(string projectPath, Targets target)
        {
            return new SourcesProvider(projectPath, target, _fileSystem, _console);
        }

        public ISourceComparer CreateSourceComparer()
        {
            return new SourceComparer();
        }
    }
}