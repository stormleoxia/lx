namespace Lx.Tools.Projects.Sync
{
    public class MissingFileInSource : ComparisonResult
    {
        public MissingFileInSource(string item) : base(item)
        {
        }

        public override string ToString()
        {
            return "Source File Missing: " + Path;
        }
    }
}