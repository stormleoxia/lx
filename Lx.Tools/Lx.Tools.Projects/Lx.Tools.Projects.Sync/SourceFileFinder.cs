using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.Sync.FilterSteps;

namespace Lx.Tools.Projects.Sync
{
    public static class ScopesEx
    {
        public static HashSet<string> AllValues { get; private set; }

        static ScopesEx()
        {
            AllValues = new HashSet<string>(Enum.GetNames(typeof(Scopes)));
        }
    }

    internal class SourceFileFinder
    {
        private readonly string _directory;
        private readonly IFileSystem _fileSystem;
        private readonly string _projectFilePath;
        private static readonly List<FilterStep> _steps = new List<FilterStep>
        {
            new TestFilterStep(),
            new TargetFilterStep(),
            new ScopeFilterStep(),
            new NameFilterStep(),
            new NotSupportedTargetStep()
        };

        public SourceFileFinder(string projectFilePath, IFileSystem fileSystem)
        {            
            _projectFilePath = projectFilePath;
            _directory = Path.GetDirectoryName(_projectFilePath);
            _fileSystem = fileSystem;

        }

        public string FindSourcesFile()
        {
            var files = _fileSystem.GetFiles(_directory, "*.sources", SearchOption.TopDirectoryOnly).Select(Path.GetFileName).ToArray();
            if (files.Length == 0)
            {
                throw new InvalidOperationException("No sources found for " + _projectFilePath);
            }
            var attributes = new ProjectAttributes(_projectFilePath);
            int currentStep = 0;
            while (files.Length != 1 && currentStep < _steps.Count)
            {
                var res = _steps[currentStep].Filter(files, attributes);
                if (res.Length > 0) // Discard any filter that removed all values
                {
                    files = res;
                }
                ++currentStep;
            }
            if (files.Length != 1)
            {
                throw new InvalidOperationException("Unable to find unique source for " + _projectFilePath);
            }
            return files.First();
        }
    }
}