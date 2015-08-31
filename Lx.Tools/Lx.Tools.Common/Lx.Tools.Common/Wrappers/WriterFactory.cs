namespace Lx.Tools.Common.Wrappers
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