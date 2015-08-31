using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lx.Tools.Common;
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;
using Microsoft.Build.Evaluation;

namespace Lx.Tools.Projects.SourceDump
{
    public class SourceDump : ProgramDefinition
    {
        private readonly IWriterFactory _factory;

        public SourceDump(Options options, UsageDefinition definition, IEnvironment environment, IDebugger debugger, IConsole console, IVersion versionGetter, 
            IWriterFactory factory) :
            base(options, definition, environment, debugger, console, versionGetter)
        {
            _factory = factory;
        }


        protected override HashSet<Option> InnerManageOptions(HashSet<Option> activatedOptions)
        {
            RemoveIncompatibleOptions(activatedOptions, SourceDumperOptions.RelativePaths,
                SourceDumperOptions.AbsolutePaths);
            RemoveIncompatibleOptions(activatedOptions, SourceDumperOptions.UnixPaths, SourceDumperOptions.WindowsPaths);
            return activatedOptions;
        }


        /// <summary>
        ///     Remove the incompatible option and keep the superseding one.
        /// </summary>
        /// <param name="managedOptions">The managed options.</param>
        /// <param name="supersedeOption">The supersede option.</param>
        /// <param name="incompatibleOption">The incompatible option.</param>
        private void RemoveIncompatibleOptions(HashSet<Option> managedOptions, Option supersedeOption,
            Option incompatibleOption)
        {
            if (managedOptions.Contains(supersedeOption) && managedOptions.Contains(incompatibleOption))
            {
                _console.Error.WriteLine(incompatibleOption + " cannot be specified along with " + supersedeOption);
                _console.Error.WriteLine(supersedeOption + " take precedence");
                managedOptions.Remove(incompatibleOption);
            }
        }

        protected override void InnerRun(HashSet<Option> options, string[] args)
        {
            foreach (var file in args)
            {
                if (file == null)
                    continue;
                Project project;
                if (IsNotCsproj(file, out project))
                {
                    _console.Error.WriteLine(file + " is not a valid csproj. Ignored");
                    continue;
                }
                var directory = Path.GetDirectoryName(file);
                var sourceDumper = new SourceDumper(directory, options);
                var projectItems = project.GetItems("Compile").Select(x => x.EvaluatedInclude);
                var items = sourceDumper.Dump(projectItems);
                var fileName = Path.GetFileNameWithoutExtension(file);
                var newFile = fileName + ".dll.sources";
                using (
                    var writer = (options.Contains(SourceDumperOptions.OutputFile))
                        ? _factory.CreateFileWriter(Path.Combine(directory, newFile))
                        : _factory.CreateConsoleWriter())
                {
                    foreach (var item in items)
                    {
                        writer.WriteLine(item);
                    }
                }
            }
            Exit(0);
        }

        /// <summary>
        ///     Determines whether the specified file is not a valid csproj.
        ///     Files and imports should exists.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="project">The project.</param>
        /// <returns>true if it's not valid and false otherwise</returns>
        private bool IsNotCsproj(string file, out Project project)
        {
            try
            {
                project = new Project(file);
                return false;
            }
            catch (Exception e)
            {
                _console.Error.WriteLine(e.ToString());
                project = null;
                return true;

            }
        }


    }
}
