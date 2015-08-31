using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lx.Tools.Common.Wrappers
{
    public class SystemConsole : IConsole
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public void Write(string text)
        {
            Console.Write(text);
        }

        public void WriteLine(Exception exception)
        {
            Console.WriteLine(exception);
        }

        public IWriter Error
        {
            get { return new ConsoleWriter(Console.Error); }
        }
    }
}
