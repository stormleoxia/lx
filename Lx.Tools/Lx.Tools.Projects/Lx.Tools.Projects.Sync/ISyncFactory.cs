using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    public interface ISyncFactory
    {
        ProjectSync CreateProjectSynchronizer(string s, Targets target);
        DirectorySync CreateDirectorySynchronizer(string s);
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

        public ProjectSync CreateProjectSynchronizer(string projectFilePath, Targets target)
        {
            return new ProjectSync(projectFilePath, target, _console, _factory);
        }

        public DirectorySync CreateDirectorySynchronizer(string directoryPath)
        {
            return new DirectorySync(directoryPath, this, _fileSystem);
        }
    }
}