using System.IO;

namespace Lx.Tools.Common.Wrappers
{
    public sealed class FileWriter : IWriter
    {
        private readonly StreamWriter _writer;

        public FileWriter(string file)
        {
            _writer = File.CreateText(file);
        }

        public void WriteLine(string text)
        {
            _writer.WriteLine(text);
        }

        public void Dispose()
        {
            _writer.Close();
            _writer.Dispose();
        }
    }
}