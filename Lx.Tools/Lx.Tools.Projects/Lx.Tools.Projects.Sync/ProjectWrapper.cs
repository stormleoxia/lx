using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Evaluation;

namespace Lx.Tools.Projects.Sync
{
    public class ProjectWrapper : IProject
    {
        private readonly Project _project;

        public ProjectWrapper(string path)
        {
            _project = new Project(path, null, null, ProjectCollection.GlobalProjectCollection, ProjectLoadSettings.IgnoreMissingImports);
        }

        public void AddItem(string itemType, string itemInclude)
        {
            _project.AddItem(itemType, itemInclude);
        }

        public ICollection<ISyncProjectItem> GetItems(string itemType)
        {
            return _project.GetItems(itemType).Select(x => new SyncProjectItem(x)).ToArray();
        }

        public void RemoveItem(ISyncProjectItem item)
        {
            _project.RemoveItem(item.InnerItem);
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

    public class SyncProjectItem : ISyncProjectItem
    {
        public SyncProjectItem(ProjectItem projectItem)
        {
            InnerItem = projectItem;
        }

        public ProjectItem InnerItem { get; private set; }

        public string EvaluatedInclude
        {
            get { return InnerItem.EvaluatedInclude; }
        }
    }
}