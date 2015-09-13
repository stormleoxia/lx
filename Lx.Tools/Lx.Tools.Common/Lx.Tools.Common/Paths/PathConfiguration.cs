using System.IO;

namespace Lx.Tools.Common.Paths
{
    public class PathConfiguration
    {
        public PathConfiguration()
        {
            DefaultPlatformPathType = PlatformPathTypes.Infer;
            GoToParent = "..";
            StayInCurrent = ".";
            DirectorySeparator = Path.DirectorySeparatorChar.ToString();
            AltDirectorySeparator = Path.AltDirectorySeparatorChar.ToString();
        }

        public string DirectorySeparator { get; private set; }

        public string StayInCurrent { get; private set; }
        public bool NormalizePath { get; set; }
        public bool IgnoreCase { get; set; }
        public PlatformPathTypes DefaultPlatformPathType { get; set; }
        public string GoToParent { get; set; }
        public string AltDirectorySeparator { get; set; }
    }
}