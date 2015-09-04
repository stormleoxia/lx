using System.Collections.Generic;
using System.IO;
using Lx.Tools.Common;
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    public class ProgramSync : ProgramDefinition
    {
        private readonly ISyncFactory _factory;
        private readonly IFileSystem _fileSystem;

        public ProgramSync(ISyncFactory factory, IFileSystem fileSystem, ProgramOptions options,
            UsageDefinition definition,
            IEnvironment environment, IDebugger debugger, IConsole console, IVersion versionGetter) :
                base(options, definition, environment, debugger, console, versionGetter)
        {
            _factory = factory;
            _fileSystem = fileSystem;
        }

        protected override HashSet<Option> InnerManageOptions(HashSet<Option> activatedOptions)
        {
            return activatedOptions;
        }

        protected override void InnerRun(HashSet<Option> options, string[] args)
        {
            foreach (var arg in args)
            {
                if (IsCsProj(arg))
                {
                    var target = TargetsEx.ExtractTarget(arg);
                    var sync = _factory.CreateProjectSynchronizer(arg, target);
                    sync.Synchronize();
                }
                else if (IsDirectory(arg))
                {
                    var sync = _factory.CreateDirectorySynchronizer(arg);
                    sync.Synchronize();
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
}