using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    public class SyncFactory : ISyncFactory
    {
        private readonly IConsole _console;
        private readonly IProjectFactory _factory;
        private readonly IFileSystem _fileSystem;
        private readonly IDirectoryValidator _validator;

        public SyncFactory(IProjectFactory factory, IConsole console, IFileSystem fileSystem, IDirectoryValidator validator)
        {
            _factory = factory;
            _console = console;
            _fileSystem = fileSystem;
            _validator = validator;
        }

        public ISynchronizer CreateProjectSynchronizer(string projectFilePath)
        {
            var target = TargetsEx.ExtractTarget(projectFilePath);
            return new ProjectSync(projectFilePath, target, _console, _factory);
        }

        public ISynchronizer CreateDirectorySynchronizer(string directoryPath)
        {
            return new DirectorySync(directoryPath, this, _fileSystem, _validator, _factory);
        }
    }
}