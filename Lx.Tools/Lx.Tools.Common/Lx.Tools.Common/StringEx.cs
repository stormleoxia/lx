using System.IO;

namespace Lx.Tools.Common
{
    public static class StringEx
    {
        public static string ToPlatformPath(this string path)
        {
            return path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }
    }
}
