namespace Lx.Tools.Common.Paths
{
    public class PathConfiguration
    {
        public PathConfiguration()
        {
            DefaultPlatformPathType = PlatformPathTypes.Infer;            
        }

        public bool NormalizePath { get; set; }
        public bool IgnoreCase { get; set; }
        public PlatformPathTypes DefaultPlatformPathType { get; set; }
    }
}