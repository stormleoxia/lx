namespace Lx.Tools.Projects.Sync
{
    public class MissingFileInSource : ComparisonResult
    {
        public MissingFileInSource(string item) : base(item)
        {
        }

        public override string ToString()
        {
            return Path + " is missing in source file";
        }
    }
}