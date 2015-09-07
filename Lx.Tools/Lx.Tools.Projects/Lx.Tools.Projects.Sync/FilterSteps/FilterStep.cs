namespace Lx.Tools.Projects.Sync.FilterSteps
{
    public abstract class FilterStep
    {
        public abstract string[] Filter(string[] files, ProjectAttributes attributes);
    }
}