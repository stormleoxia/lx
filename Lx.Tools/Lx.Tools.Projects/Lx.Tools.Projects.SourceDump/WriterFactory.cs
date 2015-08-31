using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.SourceDump
{
    public class WriterFactory : IWriterFactory
    {
        public IWriter CreateFileWriter(string file)
        {
            return new FileWriter(file);
        }

        public IWriter CreateConsoleWriter()
        {
            return new ConsoleWriter();
        }
    }
}