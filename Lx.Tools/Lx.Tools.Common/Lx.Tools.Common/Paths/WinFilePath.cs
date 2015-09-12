namespace Lx.Tools.Common.Paths
{
    internal class WinFilePath : WinPath, IFilePath
    {
        private readonly string[] _components;
        private IFilePath _file;
        private bool _fileUninitialized = true;


        public WinFilePath(PathFactory factory, PlatformPathTypes platformPathType, PathTypes pathType, string path, string[] components) 
            : base(factory, platformPathType, pathType, path, components)
        {           
        }

        public override IFilePath File
        {
            get
            {
                if (_fileUninitialized)
                {
                    _fileUninitialized = false;
                    string path = PathUtility.GetFile(Components);
                    if (path != null)
                    {
                        _file = (IFilePath)Factory.Create(path, PlatformPathType, PathTypes.File, new string[] {path});
                    }
                }
                return _file;
            }
            
        }
    }
}