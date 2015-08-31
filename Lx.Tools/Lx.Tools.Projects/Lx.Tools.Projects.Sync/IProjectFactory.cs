namespace Lx.Tools.Projects.Sync
{
    public interface IProjectFactory
    {
        IProject CreateProject(string projectPath);
        ProjectUpdater CreateProjectUpdater(IProject project);
        ISourceFinder CreateSourceFileFinder(string projectPath, Targets target);
        ISourceComparer CreateSourceComparer();
    }
}