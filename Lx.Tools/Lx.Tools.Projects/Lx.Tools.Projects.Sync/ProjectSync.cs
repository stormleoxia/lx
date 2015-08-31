using System;
using System.Collections.Generic;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    public class ProjectSync
    {
        private readonly IConsole _console;
        private readonly IProjectFactory _factory;
        private readonly string _projectPath;
        private readonly Targets _target;

        public ProjectSync(string projectPath, Targets target, IConsole console, IProjectFactory factory)
        {
            _projectPath = projectPath;
            _target = target;
            _console = console;
            _factory = factory;
        }

        public void Synchronize()
        {
            try
            {
                var project = _factory.CreateProject(_projectPath);
                var items = GetItems(project);
                var finder = _factory.CreateSourceFileFinder(_projectPath, _target);
                finder.FindSourcesFile();
                var sources = finder.GetFiles();
                var comparer = _factory.CreateSourceComparer();
                var comparison = comparer.Compare(items, sources);
                _console.WriteLine(comparison.ToString());
                var updater = _factory.CreateProjectUpdater(project);
                updater.Update(comparison);
            }
            catch (Exception e)
            {
                _console.WriteLine(e);
            }
        }

        private HashSet<string> GetItems(IProject project)
        {
            var hashSet = new HashSet<string>();
            var items = project.GetItems("Compile");
            foreach (var item in items)
            {
                hashSet.Add(item.EvaluatedInclude.Replace('\\', '/'));
            }
            return hashSet;
        }
    }
}