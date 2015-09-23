namespace Lx.Tools.Files.Grep
{
    public interface IDirectoryExplorer
    {
        void Explore(string directory, string pattern);
    }
}