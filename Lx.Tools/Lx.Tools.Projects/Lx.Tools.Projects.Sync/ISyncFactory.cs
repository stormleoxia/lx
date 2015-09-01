using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    public interface ISyncFactory
    {
        ISynchronizer CreateProjectSynchronizer(string s, Targets target);
        ISynchronizer CreateDirectorySynchronizer(string s);
    }

    public class SyncFactory : ISyncFactory
    {
        private readonly IConsole _console;
        private readonly IProjectFactory _factory;
        private readonly IFileSystem _fileSystem;

        public SyncFactory(IProjectFactory factory, IConsole console, IFileSystem fileSystem)
        {
            _factory = factory;
            _console = console;
            _fileSystem = fileSystem;
        }

        public ISynchronizer CreateProjectSynchronizer(string projectFilePath, Targets target)
        {
            return new ProjectSync(projectFilePath, target, _console, _factory);
        }

        public ISynchronizer CreateDirectorySynchronizer(string directoryPath)
        {
            return new DirectorySync(directoryPath, this, _fileSystem);
        }
    }
}