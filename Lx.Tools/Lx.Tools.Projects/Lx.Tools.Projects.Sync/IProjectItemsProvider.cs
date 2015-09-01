using System.Collections.Generic;

namespace Lx.Tools.Projects.Sync
{
    public interface IProjectItemsProvider
    {
        HashSet<string> GetItems();
    }
}