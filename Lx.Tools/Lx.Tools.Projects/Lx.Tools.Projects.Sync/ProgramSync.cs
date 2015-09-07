using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lx.Tools.Common;
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    public sealed class ProgramSync : ProgramDefinition
    {
        private readonly ISyncFactory _factory;
        private readonly IFileSystem _fileSystem;
        private readonly IProjectSyncConfiguration _configuration;
        private readonly IDirectoryValidator _validator;
        private readonly IProjectFactory _projectFactory;

        public ProgramSync(ISyncFactory factory, IFileSystem fileSystem, ProgramOptions options,
            IProjectSyncConfiguration configuration,
            UsageDefinition definition, IDirectoryValidator validator, IProjectFactory projectFactory,
            IEnvironment environment, IDebugger debugger, IConsole console, IVersion versionGetter) :
                base(options, definition, environment, debugger, console, versionGetter)
        {
            _factory = factory;
            _fileSystem = fileSystem;
            _configuration = configuration;
            _validator = validator;
            _projectFactory = projectFactory;
        }

        protected override HashSet<Option> InnerManageOptions(HashSet<Option> activatedOptions)
        {
            _configuration.Options = activatedOptions;
            return activatedOptions;
        }

        protected override void InnerRun(HashSet<Option> options, string[] args)
        {
            foreach (var arg in args)
            {
                if (_validator.IsDirectoryValid(arg))
                {
                    if (IsCsProj(arg) && _projectFactory.IsValidProject(arg))
                    {
                        var sync = _factory.CreateProjectSynchronizer(arg);
                        sync.Synchronize();
                    }
                    else if (IsDirectory(arg))
                    {
                        var sync = _factory.CreateDirectorySynchronizer(arg);
                        sync.Synchronize();
                    }
                }
            }
        }      

        private bool IsDirectory(string directoryPath)
        {
            return _fileSystem.DirectoryExists(directoryPath);
        }

        private bool IsCsProj(string path)
        {
            if (_fileSystem.FileExists(path) && Path.GetExtension(path) == ".csproj")
            {
                return true;
            }
            return false;
        }
    }

    public interface IDirectoryValidator
    {
        bool IsDirectoryValid(string path);
    }

    public sealed class DirectoryValidator : IDirectoryValidator
    {
        private readonly IProjectSyncConfiguration _configuration;

        public DirectoryValidator(IProjectSyncConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool IsDirectoryValid(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            return _configuration.IgnoredDirectories.All(
                dir => !path.ToLower().ToPlatformPath().Contains(dir.ToLower().ToPlatformPath()));
        }
    }
}