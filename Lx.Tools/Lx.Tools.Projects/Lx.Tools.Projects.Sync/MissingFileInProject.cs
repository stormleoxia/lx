namespace Lx.Tools.Projects.Sync
{
    public class MissingFileInProject : ComparisonResult
    {
        public MissingFileInProject(string item) : base(item)
        {
        }

        public override string ToString()
        {
            return "Project File Missing: " + Path;
        }
    }
}