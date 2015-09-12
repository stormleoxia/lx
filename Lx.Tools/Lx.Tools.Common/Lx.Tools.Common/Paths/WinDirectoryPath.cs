using System;

namespace Lx.Tools.Common.Paths
{
    internal class WinDirectoryPath : WinPath
    {
        public WinDirectoryPath(PathFactory factory, PlatformPathTypes platformPathType, PathTypes pathType, string path, string[] components) :
            base(factory, platformPathType, pathType, path, components)
        {
        }

        public override IFilePath File { get { return null; }}
    }
}