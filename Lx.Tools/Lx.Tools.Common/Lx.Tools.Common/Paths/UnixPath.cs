using System;

namespace Lx.Tools.Common.Paths
{
    internal abstract class UnixPath : IPath
    {
        public IFilePath File { get; private set; }
        public IPath Parent { get; private set; }
        public IPath Intersect(IPath path, PathIntersections bottomUp)
        {
            return path;
        }

        public string Path { get; private set; }
        public IPath Root { get; private set; }
        public IPath Child { get; private set; }
    }
}