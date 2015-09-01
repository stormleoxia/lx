namespace Lx.Tools.Projects.Sync
{
    public interface IProjectFactory
    {
        IProjectItemsProvider CreateProjectItemsProvider(string projectPath, Targets target);
        IProjectUpdater CreateProjectUpdater(string projectPath);
        ISourcesProvider CreateSourcesProvider(string projectPath, Targets target);
        ISourceComparer CreateSourceComparer();
    }
}