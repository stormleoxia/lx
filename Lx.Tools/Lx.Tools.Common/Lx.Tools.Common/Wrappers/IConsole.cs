using System;

namespace Lx.Tools.Common.Wrappers
{
    public interface IConsole
    {
        IWriter Error { get; }
        string ReadLine();
        void WriteLine(string text);
        void Write(string text);
        void WriteLine(Exception exception);
    }
}