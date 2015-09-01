using System.Collections.Generic;

namespace Lx.Tools.Projects.Sync
{
    public class ProjectItemsProvider : IProjectItemsProvider
    {
        private readonly IProject _project;

        public ProjectItemsProvider(IProject project)
        {
            _project = project;
        }

        public HashSet<string> GetItems()
        {
            var hashSet = new HashSet<string>();
            var items = _project.GetItems("Compile");
            foreach (var item in items)
            {
                hashSet.Add(item.EvaluatedInclude.Replace('\\', '/'));
            }
            return hashSet;
        }
    }
}