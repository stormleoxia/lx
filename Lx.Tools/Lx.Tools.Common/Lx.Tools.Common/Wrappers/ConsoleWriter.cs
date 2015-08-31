using System;
using System.IO;

namespace Lx.Tools.Common.Wrappers
{
    public class ConsoleWriter : IWriter
    {
        private readonly TextWriter _textWriter;

        public ConsoleWriter() : this(Console.Out)
        {
        }

        public ConsoleWriter(TextWriter textWriter)
        {
            _textWriter = textWriter;
        }

        public void Dispose()
        {
            
        }

        public void WriteLine(string text)
        {
            _textWriter.WriteLine(text);
        }
    }
}