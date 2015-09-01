namespace Lx.Tools.Projects.Sync
{
    public interface IProjectUpdater
    {
        void Update(SourceComparison comparison);
    }
}