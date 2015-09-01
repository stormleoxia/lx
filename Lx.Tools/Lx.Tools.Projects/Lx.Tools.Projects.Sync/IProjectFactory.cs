using System.Collections.Generic;

namespace Lx.Tools.Projects.Sync
{
    public interface IProjectFactory
    {
        IProjectItemsProvider CreateProjectItemsProvider(string projectPath, Targets target);
        ProjectUpdater CreateProjectUpdater(string projectPath);
        ISourcesProvider CreateSourcesProvider(string projectPath, Targets target);
        ISourceComparer CreateSourceComparer();
    }

    public interface IProjectItemsProvider
    {
        HashSet<string> GetItems();
    }
}