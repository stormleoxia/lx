using Lx.Tools.Common.Wrappers;
using Microsoft.Practices.Unity;

namespace Lx.Tools.Projects.Sync
{
    public class ProjectFactory : IProjectFactory
    {
        private readonly IUnityContainer _container;
        private readonly IFileSystem _fileSystem;

        public ProjectFactory(IUnityContainer container, IFileSystem fileSystem)
        {
            _container = container;
            _fileSystem = fileSystem;
        }

        public IProject CreateProject(string projectPath)
        {
            return new ProjectWrapper(projectPath);
        }

        public ProjectUpdater CreateProjectUpdater(IProject project)
        {
            return new ProjectUpdater(project, _fileSystem);
        }

        public ISourceFinder CreateSourceFileFinder(string projectPath, Targets target)
        {
            var sourceFinder = _container.Resolve<ISourceFinder>(
                new ParameterOverride("projectFilePath", projectPath),
                new ParameterOverride("target", target));
            return sourceFinder;
        }

        public ISourceComparer CreateSourceComparer()
        {
            return new SourceComparer();
        }
    }
}