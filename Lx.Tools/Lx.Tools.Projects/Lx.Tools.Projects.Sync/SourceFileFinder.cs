using System;
using System.IO;
using System.Linq;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    internal class SourceFileFinder
    {
        private readonly string _projectFilePath;
        private readonly Targets _target;
        private readonly IConsole _console;
        private readonly IFileSystem _fileSystem;
        private readonly Targets[] _notSupportedTargets;
        private readonly string _directory;

        public SourceFileFinder(string projectFilePath, Targets target, IConsole console, IFileSystem fileSystem)
        {
            _notSupportedTargets = TargetsEx.GetValuesButAll().Where(x => x != target).ToArray();
            _projectFilePath = projectFilePath;
            _directory = Path.GetDirectoryName(_projectFilePath);
            _target = target;
            _console = console;
            _fileSystem = fileSystem;
        }

        public string FindSourcesFile()
        {
            try
            {
                return InnerFindSourcesFile();
            }
            catch (Exception e)
            {
                _console.WriteLine("ERROR: " + e.Message);
            }
            return null;
        }

        private string InnerFindSourcesFile()
        {
            var fileName = Path.GetFileName(_projectFilePath);
            var name = fileName.Split('.')[0]; // remove extension
            var splits = name.Split('-');
            var mainName = splits[0];
            var subName = string.Empty;
            if (splits.Length > 1)
            {
                subName = splits[1];
            }
            var files = _fileSystem.GetFiles(_directory, "*.sources", SearchOption.TopDirectoryOnly);
            if (files.Length == 0)
            {
                throw new InvalidOperationException("No sources found for " + _projectFilePath);
            }
            if (files.Length != 1)
            {
                var targeted = files.Where(x => x.Contains(_target.Convert())).ToArray();
                if (targeted.Length == 1)
                {
                    return targeted.First();
                }
                if (targeted.Length > 1)
                {
                    throw new InvalidOperationException("Several targeting sources found for the same target on " +
                                                        _projectFilePath);
                }
            }
            var filtered = files.Where(x =>
            {
                var xName = Path.GetFileName(x);
                return !_notSupportedTargets.Any(y => xName.Contains(y.Convert()));
            }).ToArray();
            if (filtered.Length != 1)
            {
                throw new InvalidOperationException("Unable to find unique source for " + _projectFilePath);
            }
            return filtered.First();
        }
    }
}