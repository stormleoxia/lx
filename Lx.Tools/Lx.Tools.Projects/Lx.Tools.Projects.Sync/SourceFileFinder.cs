using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    internal class SourceFileFinder
    {
        private readonly IConsole _console;
        private readonly string _directory;
        private readonly IFileSystem _fileSystem;
        private readonly Targets[] _notSupportedTargets;
        private readonly string _projectFilePath;
        private readonly Targets _target;

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
            return InnerFindSourcesFile();
        }

        private string InnerFindSourcesFile()
        {
            var files = _fileSystem.GetFiles(_directory, "*.sources", SearchOption.TopDirectoryOnly).Select(Path.GetFileName).ToArray();
            if (files.Length == 0)
            {
                throw new InvalidOperationException("No sources found for " + _projectFilePath);
            }
            var projectFileName = Path.GetFileName(_projectFilePath);
            var isTest = IsTest(projectFileName);
            files = files.Where(x => FilterTest(isTest, x)).ToArray();
            if (files.Length != 1)
            {                
                string[] targeted;
                if (_target != Targets.All)
                {
                   targeted = files.Where(x => x.Contains(_target.Convert())).ToArray();
                }
                else
                {
                    targeted = files.Where(x => !TargetsEx.GetValuesButAll().Any(y => x.Contains(y.Convert()))).ToArray();
                }
                if (targeted.Length > 1)
                {
                    var projectFileNameWithoutExtension = Path.GetFileNameWithoutExtension(projectFileName);
                    if (projectFileNameWithoutExtension != null)
                    {
                        targeted = files.Where(x => x.Contains(projectFileNameWithoutExtension)).ToArray();
                    }
                }
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
                return !_notSupportedTargets.Any(y => x.Contains(y.Convert()));
            }).ToArray();
            if (filtered.Length != 1)
            {
                throw new InvalidOperationException("Unable to find unique source for " + _projectFilePath);
            }
            return filtered.First();
        }

        private bool IsTest(string projectFilePath)
        {
            return projectFilePath.Contains("test") || projectFilePath.Contains("Test");
        }

        private bool FilterTest(bool isTest, string sourceFile)
        {
            var res = IsTest(sourceFile);
            return isTest ? res : !res;
        }
    }
}