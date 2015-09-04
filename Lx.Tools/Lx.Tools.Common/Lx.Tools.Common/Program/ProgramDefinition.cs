using System.Collections.Generic;
using System.Text;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Common.Program
{
    public abstract class ProgramDefinition
    {
        protected readonly IConsole _console;
        private readonly IDebugger _debugger;
        private readonly UsageDefinition _definition;
        private readonly IEnvironment _environment;
        private readonly Options _options;
        private readonly IVersion _versionGetter;

        protected ProgramDefinition(Options options, UsageDefinition definition, IEnvironment environment,
            IDebugger debugger, IConsole console, IVersion versionGetter)
        {
            _options = options;
            _definition = definition;
            _environment = environment;
            _debugger = debugger;
            _console = console;
            _versionGetter = versionGetter;
        }

        public void Run(string[] args)
        {
            if (args.Length == 0)
            {
                DisplayUsage();
            }
            var options = ManageOptions(args);
            InnerRun(options, args);
            Exit(0);
        }

        private HashSet<Option> ManageOptions(string[] args)
        {
            var activatedOptions = _options.ParseOptions(args);
            if (activatedOptions.Contains(Options.Help))
            {
                DisplayUsage();
                _options.DisplayHelp();
                Exit(0);
            }
            return InnerManageOptions(activatedOptions);
        }

        protected abstract HashSet<Option> InnerManageOptions(HashSet<Option> activatedOptions);
        protected abstract void InnerRun(HashSet<Option> options, string[] args);

        public void DisplayUsage()
        {
            var builder = new StringBuilder();
            builder.Append("Usage: ");
            builder.Append(_definition.ExeName);
            if (_options.AvailableOptions.Count > 0)
            {
                builder.Append("[options] ");
            }
            foreach (var arg in _definition.Arguments)
            {
                builder.Append(arg.Name);
            }
            _console.WriteLine(builder.ToString());
            var version = _versionGetter.Version;
            _console.WriteLine("src-dump " + version);
            _console.WriteLine("Copyright (C) 2015 Leoxia Ltd");
        }

        public void Exit(int exitCode)
        {
            if (_debugger.IsAttached)
            {
                _console.ReadLine();
            }
            _environment.Exit(exitCode);
        }
    }
}