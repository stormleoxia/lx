using System.Collections.Generic;
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;
using Microsoft.Practices.Unity;

namespace Lx.Tools.Projects.Reference
{
    public class ReferenceManager : ProgramDefinition
    {
        private readonly IUnityContainer _container;

        public ReferenceManager(IUnityContainer container, ReferenceOptions options, UsageDefinition definition, IEnvironment environment, IDebugger debugger, IConsole console, IVersion versionGetter) 
            : base(options, definition, environment, debugger, console, versionGetter)
        {
            _container = container;
        }

        protected override HashSet<Option> InnerManageOptions(HashSet<Option> activatedOptions)
        {
            return activatedOptions;            
        }

        protected override void InnerRun(HashSet<Option> options, string[] args)
        {
            if (options.Contains(ReferenceOptions.AddReference))
            {
                IReferenceAdder adder = _container.Resolve<IReferenceAdder>();
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