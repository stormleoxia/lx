using System.IO;

namespace Lx.Tools.Common.Paths
{
    public class PathConfiguration
    {
        public PathConfiguration()
        {
            DefaultPlatformPathType = PlatformPathTypes.Infer;
            GoToParentPattern = "..";
            StayInCurrentPattern = ".";
            DirectorySeparatorPattern = Path.DirectorySeparatorChar.ToString();
        }

        public string DirectorySeparatorPattern { get; private set; }

        public string StayInCurrentPattern { get; private set; }
        public bool NormalizePath { get; set; }
        public bool IgnoreCase { get; set; }
        public PlatformPathTypes DefaultPlatformPathType { get; set; }
        public string GoToParentPattern { get; set; }
    }
}