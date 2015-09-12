using System.IO;
using System.Linq;

namespace Lx.Tools.Common.Paths
{
    internal abstract class WinPath : BasePath
    {

        protected WinPath(PathFactory factory, PlatformPathTypes platformPathType, PathTypes pathType, string path, string[] components)
            : base(factory, platformPathType, pathType, path, components)
        {
            var firstComponent = components.FirstOrDefault();
            Drive = PathUtility.GetDrive(firstComponent);
        }

        public string Drive { get; private set; }
    }

    internal abstract class BasePath : IPath
    {
        private BasePath _parent = null;
        private bool _parentUninitialized = true;
        private bool _rootUninitialized = true;
        private IPath _root;

        protected BasePath(PathFactory factory, PlatformPathTypes platformPathType, PathTypes pathType, string path, string[] components)
        {
            Factory = factory;
            PlatformPathType = platformPathType;
            PathType = pathType;
            Components = components;
            Path = path;
        }

        protected internal PlatformPathTypes PlatformPathType { get; private set; }
        protected internal PathTypes PathType { get; private set; }
        protected internal PathFactory Factory { get; private set; }
        protected internal string[] Components { get; private set; }

        public abstract IFilePath File { get;  }
        
        public string Path { get; private set; }

        public IPath Root
        {
            get
            {
                if (_rootUninitialized)
                {
                    _rootUninitialized = false;
                    var rootPath = PathUtility.GetRootPath(Path, Components);
                    if (rootPath != null)
                    {
                        _root = Factory.Create(rootPath, PlatformPathType, PathTypes.Root,
                            new string[] {Components.First()});
                    }
                }
                return _root;
            }
        }
        
        public IPath Child { get; private set; }

        public IPath Parent
        {
            get
            {
                if (_parentUninitialized)
                {
                    _parentUninitialized = false;
                    if (Components.Length > 1)
                    {
                        var path = PathUtility.GetParent(Path, Components);
                        var components = Components.Take(Components.Length - 1).ToArray();
                        // TODO specialize creation to avoid cast
                        _parent = (BasePath)Factory.Create(path, PlatformPathType, PathTypes.Root, components);
                        _parent.Child = this;
                    }
                }
                return _parent;
            }
        }


        public IPath Intersect(IPath path, PathIntersections bottomUp)
        {
            return path;
        }
    }
}