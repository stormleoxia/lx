using System.Collections.Generic;
using Microsoft.Build.Evaluation;

namespace Lx.Tools.Projects.Sync
{
    public interface IProject
    {
        string FullPath { get; }
        void AddItem(string itemType, string itemInclude);
        ICollection<ProjectItem> GetItems(string itemType);
        void RemoveItem(ProjectItem item);
        void Save();
    }
}