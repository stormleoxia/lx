namespace Lx.Tools.Projects.Sync
{
    public interface IProjectFactory
    {
        IProjectItemsProvider CreateProjectItemsProvider(string projectPath);
        IProjectUpdater CreateProjectUpdater(string projectPath);
        ISourcesProvider CreateSourcesProvider(string projectPath);
        ISourceComparer CreateSourceComparer();
    }
}