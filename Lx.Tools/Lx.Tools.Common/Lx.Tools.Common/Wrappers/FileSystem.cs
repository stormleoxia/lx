using System.IO;

namespace Lx.Tools.Common.Wrappers
{
    public class FileSystem : IFileSystem
    {
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public string[] GetFiles(string path, string filter, SearchOption searchOption)
        {
            return Directory.GetFiles(path, filter, searchOption);
        }

        public TextReader OpenText(string filePath)
        {
            return File.OpenText(filePath);
        }

    }
}
