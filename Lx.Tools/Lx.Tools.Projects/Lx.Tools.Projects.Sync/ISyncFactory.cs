namespace Lx.Tools.Projects.Sync
{
    public interface ISyncFactory
    {
        ISynchronizer CreateProjectSynchronizer(string file);
        ISynchronizer CreateDirectorySynchronizer(string file);
    }
}