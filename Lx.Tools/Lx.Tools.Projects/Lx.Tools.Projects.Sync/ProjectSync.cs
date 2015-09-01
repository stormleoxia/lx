using System;
using System.Collections.Generic;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    public class ProjectSync : ISynchronizer
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
                var project = _factory.CreateProjectItemsProvider(_projectPath, _target);
                var items = project.GetItems();
                var finder = _factory.CreateSourcesProvider(_projectPath, _target);
                var sources = finder.GetFiles();
                var comparer = _factory.CreateSourceComparer();
                var comparison = comparer.Compare(items, sources);
                _console.WriteLine(comparison.ToString());
                var updater = _factory.CreateProjectUpdater(_projectPath);
                updater.Update(comparison);
            }
            catch (Exception e)
            {
                _console.WriteLine(e);
            }
        }


    }
}