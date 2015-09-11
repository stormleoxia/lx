namespace Lx.Tools.Common.Paths
{
    public interface IPath
    {
        IFilePath File { get; }
        IPath Parent { get; }
        IPath Intersect(IPath path, PathIntersections bottomUp);
        string Path { get; }
        IPath Root { get; }
        IPath Child { get; }
    }

    public interface IFilePath : IPath
    {
    }
}