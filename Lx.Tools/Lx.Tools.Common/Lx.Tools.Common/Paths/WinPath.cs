namespace Lx.Tools.Common.Paths
{
    public abstract class WinPath : GenericPath
    {
        protected WinPath(string path, string[] components)
            : base(path, components)
        {
            
        }

        public string Drive { get; protected set; }
    }

    public abstract class GenericPath : IPath
    {
        protected GenericPath(string path, string[] components)
        {
            Path = path;
        }
        
        public IFilePath File { get; set; }
        
        public string Path { get; private set; }
        
        public IPath Root { get; private set; }
        
        public IPath Child { get; private set; }

        public IPath Parent { get; private set; }


        public IPath Intersect(IPath path, PathIntersections bottomUp)
        {
            return path;
        }
    }
}