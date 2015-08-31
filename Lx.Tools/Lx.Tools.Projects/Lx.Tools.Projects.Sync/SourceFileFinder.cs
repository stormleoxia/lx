using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    internal class SourceFileFinder : ISourceFinder
    {
        private readonly IConsole _console;
        private readonly string _directory;
        private readonly IFileSystem _fileSystem;
        private readonly string _projectFilePath;
        private readonly Targets _target;
        private readonly Targets[] NotSupportedTargets;
        private string sourceFile;

        public SourceFileFinder(string projectFilePath, Targets target, IFileSystem fileSystem, IConsole console)
        {
            _projectFilePath = projectFilePath;
            _target = target;
            _fileSystem = fileSystem;
            _console = console;
            _directory = Path.GetDirectoryName(_projectFilePath);

            NotSupportedTargets = TargetsEx.GetValuesButAll().Where(x => x != _target).ToArray();
        }

        public HashSet<string> GetFiles()
        {
            var res = new HashSet<string>();
            ReadSourceFile(res, sourceFile);
            return res;
        }

        public void FindSourcesFile()
        {
            try
            {
                sourceFile = InnerFindSourcesFile();
            }
            catch (Exception e)
            {
                _console.WriteLine("ERROR: " + e.Message);
            }
        }

        private void ReadSourceFile(HashSet<string> res, string file)
        {
            var result = string.Empty;
            if (!string.IsNullOrEmpty(file))
            {
                var filePath = Path.Combine(_directory, file);
                using (var reader = _fileSystem.OpenText(filePath))
                {
                    result = reader.ReadToEnd();
                }
            }
            var results = result.Split('\n');
            var includes = new List<string>();
            foreach (var line in results)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    if (IsInclude(line))
                    {
                        includes.Add(GetInclude(line));
                    }
                    else
                    {
                        res.Add(line);
                    }
                }
            }
            foreach (var include in includes)
            {
                ReadSourceFile(res, include);
            }
        }

        private string GetInclude(string line)
        {
            return line.Replace("#include", string.Empty).Trim();
        }

        public static bool IsInclude(string line)
        {
            if (line.StartsWith(" "))
            {
                return true;
            }
            return line.TrimStart(' ', '\t').StartsWith("#include");
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
                return !NotSupportedTargets.Any(y => xName.Contains(y.Convert()));
            }).ToArray();
            if (filtered.Length != 1)
            {
                throw new InvalidOperationException("Unable to find unique source for " + _projectFilePath);
            }
            return filtered.First();
        }
    }
}