using System;

namespace Lx.Tools.Common
{
    public sealed class ConsoleWriter : IWriter
    {
        public void WriteLine(string text)
        {
            Console.Out.WriteLine(text);
        }

        public void Dispose()
        {
        }
    }
}