using System.Collections.Generic;
using Microsoft.Build.Evaluation;

namespace Lx.Tools.Projects.Sync
{
    public class ProjectWrapper : IProject
    {
        private readonly Project _project;

        public ProjectWrapper(string path)
        {
            _project = new Project(path);
        }

        public void AddItem(string itemType, string itemInclude)
        {
            _project.AddItem(itemType, itemInclude);
        }

        public ICollection<ProjectItem> GetItems(string itemType)
        {
            return _project.GetItems(itemType);
        }

        public void RemoveItem(ProjectItem item)
        {
            _project.RemoveItem(item);
        }

        public string FullPath
        {
            get { return _project.FullPath; }
        }

        public void Save()
        {
            _project.Save();
        }
    }
}