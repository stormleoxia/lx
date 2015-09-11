namespace Lx.Tools.Common.Paths
{
    public class WinFilePath : WinPath, IFilePath
    {
        public WinFilePath(string path, string[] components)
            : base(path, components)
        {

        }
    }
}