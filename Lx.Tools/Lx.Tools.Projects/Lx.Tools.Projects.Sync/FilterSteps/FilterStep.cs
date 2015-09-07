namespace Lx.Tools.Projects.Sync
{
    public abstract class FilterStep
    {
        public abstract string[] Filter(string[] files, ProjectAttributes attributes);
    }
}