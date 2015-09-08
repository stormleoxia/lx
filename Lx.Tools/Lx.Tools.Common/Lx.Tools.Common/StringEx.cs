using System.IO;
using System.Linq;

namespace Lx.Tools.Common
{
    public static class StringEx
    {
        public static string ToPlatformPath(this string path)
        {
            return path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }

        public static string RemoveDotPath(this string path)
        {
            var components = path.Split('/', '\\');
            return string.Join(Path.DirectorySeparatorChar.ToString(),
                components.Select(x => x.Trim()).Where(x => x != "."));
        }
    }
}
