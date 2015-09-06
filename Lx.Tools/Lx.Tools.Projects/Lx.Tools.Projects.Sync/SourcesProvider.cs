using System.Collections.Generic;
using System.IO;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    internal class SourcesProvider : ISourcesProvider
    {
        private readonly string _directory;
        private readonly IFileSystem _fileSystem;
        private readonly IConsole _console;
        private readonly string _sourceFile;

        public SourcesProvider(string projectFilePath, Targets target, IFileSystem fileSystem, IConsole console)
        {
            _fileSystem = fileSystem;
            _console = console;
            _directory = Path.GetDirectoryName(projectFilePath);
            var sourceFileFinder = new SourceFileFinder(projectFilePath, target, console, fileSystem);
            _sourceFile = sourceFileFinder.FindSourcesFile();
        }

        public HashSet<string> GetFiles()
        {
            var res = new HashSet<string>();
            ReadSourceFile(res, _sourceFile);
            return res;
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
            var results = result.Split('\n', '\r');
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
                        var filePath = Path.Combine(_directory, line);
                        if (_fileSystem.FileExists(filePath))
                        {
                            res.Add(line);
                        }
                        else
                        {
                            _console.WriteLine("Source Not Found: " + filePath);
                        }
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

        private static bool IsInclude(string line)
        {
            if (line.StartsWith(" "))
            {
                return true;
            }
            return line.TrimStart(' ', '\t').StartsWith("#include");
        }
    }
}