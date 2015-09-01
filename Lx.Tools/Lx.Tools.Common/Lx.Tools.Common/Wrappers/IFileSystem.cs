using System.IO;

namespace Lx.Tools.Common.Wrappers
{
    public interface IFileSystem
    {
        bool DirectoryExists(string path);
        bool FileExists(string path);
        string[] GetFiles(string path, string filter, SearchOption searchOption);
        TextReader OpenText(string filePath);
        string ResolvePath(string path);
    }
}