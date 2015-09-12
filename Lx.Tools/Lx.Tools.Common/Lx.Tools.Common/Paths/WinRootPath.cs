using System;

namespace Lx.Tools.Common.Paths
{
    internal class WinRootPath : WinDirectoryPath
    {
        public WinRootPath(PathFactory factory, PlatformPathTypes platformPathType, PathTypes pathType, string path, string[] components) :
            base(factory, platformPathType, pathType, path, components)
        {
        }
    }
}