using System.Collections.Generic;
using Microsoft.Build.Evaluation;

namespace Lx.Tools.Projects.Sync
{
    public interface IProject
    {
        string FullPath { get; }
        void AddItem(string itemType, string itemInclude);
        ICollection<ISyncProjectItem> GetItems(string itemType);
        void RemoveItem(ISyncProjectItem item);
        void Save();
    }

    public interface ISyncProjectItem
    {
        ProjectItem InnerItem { get; }
        string EvaluatedInclude { get; }
    }
}