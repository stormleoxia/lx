using System;
using System.Linq;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Common.Paths
{
    public class PathFactory
    {
        private readonly IFileSystem _system;
        private readonly PathConfiguration _pathConfiguration;
        private readonly PathPartFactory _partFactory;

        public PathFactory(IFileSystem system, PathConfiguration pathConfiguration)
        {
            _system = system;
            _pathConfiguration = pathConfiguration;
            _partFactory = new PathPartFactory(pathConfiguration);
        }

        public IPath Create(string path)
        {
            return Create(path, _pathConfiguration.DefaultPlatformPathType);
        }

        public IPath Create(string path, PlatformPathTypes platformPathType)
        {
            switch (platformPathType)
            {
                case PlatformPathTypes.Infer:
                    return Create(path, InferPlatformPathType(path));
                default: // TODO manage UNC and URI
                {
                    var components = path.Split('/', '\\');
                    var parts = _partFactory.MakeParts(path);
                    components = PathUtility.CleanUp(components);
                    return Create(path, platformPathType, InferPathType(path, components), components);
                }
            }

        }

        internal IPath Create(string path, PlatformPathTypes platformPathType, PathTypes pathType, string[] components)
        {
            switch (platformPathType)
            {
                case PlatformPathTypes.Infer:
                    return Create(path, InferPlatformPathType(path), pathType, components);
                case PlatformPathTypes.Windows:
                    switch (pathType)
                    {
                        case PathTypes.Root:
                            return new WinRootPath(this, platformPathType, pathType, path, components);
                        case PathTypes.Directory:
                            return new WinDirectoryPath(this, platformPathType, pathType, path, components);
                        case PathTypes.File:
                            return new WinFilePath(this, platformPathType, pathType, path, components);
                        default:
                            throw new ArgumentOutOfRangeException("pathType", pathType, null);
                    }
                case PlatformPathTypes.Unix:
                    switch (pathType)
                    {
                        case PathTypes.Root:
                            return new UnixRootPath(this, platformPathType, pathType, path, components);
                        case PathTypes.Directory:
                            return new UnixDirectoryPath(this, platformPathType, pathType, path, components);
                        case PathTypes.File:
                            return new UnixFilePath(this, platformPathType, pathType, path, components);
                        default:
                            throw new ArgumentOutOfRangeException("pathType", pathType, null);
                    }
                    ;
                default:
                    throw new ArgumentOutOfRangeException("platformPathType", platformPathType, null);
            }
        }

        private PathTypes InferPathType(string path, string[] components)
        {
            if (_system.FileExists(path))
            {
                return PathTypes.File;
            }
            if (_system.DirectoryExists(path))
            {
                if (_system.IsRoot(path))
                {
                    return PathTypes.Root;                    
                }
                return PathTypes.Directory;
            }
            return PathUtility.InferPathType(path, components);
        }

        private PlatformPathTypes InferPlatformPathType(string path)
        {
            var slashCount = path.Count(x => x == '/');
            var backslashCount = path.Count(x => x == '\\');
            if (backslashCount > slashCount || path.Count(x => x == ':') == 1)
            {
                return PlatformPathTypes.Windows;
            }
            return PlatformPathTypes.Unix;
        }
    }


    public class PathPart
    {
        public PathComponentKind Kind { get; private set; }
        public string RawValue { get; private set; }

        internal PathPart(string rawValue, PathComponentKind kind)
        {
            Kind = kind;
            RawValue = rawValue;
        }
    }
}
