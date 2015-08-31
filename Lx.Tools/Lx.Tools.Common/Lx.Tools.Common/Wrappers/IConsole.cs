using System;

namespace Lx.Tools.Common.Wrappers
{
    public interface IConsole
    {
        string ReadLine();
        void WriteLine(string text);
        void Write(string text);
        void WriteLine(Exception exception);
        IWriter Error { get; }
    }
}