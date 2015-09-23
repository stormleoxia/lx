using System.Collections.Generic;
using System.Linq;
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Files.Grep
{
    public class GrepMain : ProgramDefinition
    {
        private readonly IDirectoryExplorer _explorer;

        public GrepMain(IDirectoryExplorer explorer,
            Options options, UsageDefinition definition, IEnvironment environment, IDebugger debugger, IConsole console, IVersion versionGetter) 
            : base(options, definition, environment, debugger, console, versionGetter)
        {
            _explorer = explorer;
        }

        protected override HashSet<Option> InnerManageOptions(HashSet<Option> activatedOptions)
        {
            return activatedOptions;            
        }

        protected override void InnerRun(HashSet<Option> options, string[] args)
        {
            if (args.Count(x => !string.IsNullOrEmpty(x)) < 2)
            {
                _console.Error.WriteLine("Not enough arguments.");
                DisplayUsage();
            }
            var regex = args.First(x => !string.IsNullOrEmpty(x));
            foreach (var argument in args)
            {
                if (string.IsNullOrEmpty(argument) || argument == regex)
                {
                    continue;
                }
                _explorer.Explore(argument, regex);
            }
        }
    }
}