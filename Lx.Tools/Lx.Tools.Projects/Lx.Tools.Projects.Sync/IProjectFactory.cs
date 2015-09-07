namespace Lx.Tools.Projects.Sync
{
    public interface IProjectFactory
    {
        bool IsValidProject(string projectPath);
        IProjectItemsProvider CreateProjectItemsProvider(string projectPath);
        IProjectUpdater CreateProjectUpdater(string projectPath);
        ISourcesProvider CreateSourcesProvider(string projectPath);
        ISourceComparer CreateSourceComparer();
    }
}