using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Common.Program
{
    public class Options
    {
        public static readonly Option Help;
        private readonly IEnvironment _environment;
        private readonly IConsole _console;

        static Options()
        {
            Help = new Option { Name = "--help", Explanation = "Display this text" };
        }


        public Options(IEnvironment environment, IConsole console)
        {
            _environment = environment;
            _console = console;
            AvailableOptions = new List<Option>();
            AvailableOptions.Add(Options.Help);
        }

        public List<Option> AvailableOptions { get; private set; }

        public HashSet<Option> ParseOptions(string[] arguments)
        {
            var list = new HashSet<Option>();
            for (var index = 0; index < arguments.Length; index++)
            {
                var arg = arguments[index];
                foreach (var option in AvailableOptions)
                {
                    if (arg == option.Name)
                    {
                        list.Add(option);
                        arguments[index] = null;
                    }
                }
            }
            return list;
        }

        public void DisplayHelp()
        {
            var nl = _environment.NewLine;
            var help = new StringBuilder();
            var maxLength = AvailableOptions.Max(x => x.Name.Length);
            foreach (var option in AvailableOptions)
            {
                help.AppendFormat("  {0}  {1}{2}", option.Name.PadRight(maxLength), option.Explanation,
                    nl);
            }
         
            _console.Write(help.ToString());
        }
    }
}
