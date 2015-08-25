using System;

namespace Lx.Tools.Projects.SourceDump
{
    internal sealed class ConsoleWriter : IWriter
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