using System.Collections.Generic;
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Reference
{
    public class ReferenceManager : ProgramDefinition
    {
        public ReferenceManager(ReferenceOptions options, UsageDefinition definition, IEnvironment environment, IDebugger debugger, IConsole console, IVersion versionGetter) 
            : base(options, definition, environment, debugger, console, versionGetter)
        {
        }

        protected override HashSet<Option> InnerManageOptions(HashSet<Option> activatedOptions)
        {
            return activatedOptions;            
        }

        protected override void InnerRun(HashSet<Option> options, string[] args)
        {
            if (options.Contains(ReferenceOptions.AddReference))
            {
                ReferenceAdder adder = new ReferenceAdder();
                adder.AddReference(args);
            }
            else
            {
                _console.Error.WriteLine("No managed option");
                DisplayUsage();
            }
        }
    }
}