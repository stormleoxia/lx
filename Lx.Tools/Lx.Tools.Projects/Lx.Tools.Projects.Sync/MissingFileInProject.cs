namespace Lx.Tools.Projects.Sync
{
    public class MissingFileInProject : ComparisonResult
    {
        public MissingFileInProject(string item) : base(item)
        {
        }

        public override string ToString()
        {
            return Path + " is missing in project file";
        }
    }
}