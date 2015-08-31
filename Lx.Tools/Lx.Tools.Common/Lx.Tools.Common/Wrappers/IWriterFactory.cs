namespace Lx.Tools.Common.Wrappers
{
    public interface IWriterFactory
    {
        IWriter CreateFileWriter(string file);
        IWriter CreateConsoleWriter();
    }
}