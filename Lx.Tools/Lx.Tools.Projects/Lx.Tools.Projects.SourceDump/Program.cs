using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Lx.Tools.Common;
using Microsoft.Build.Evaluation;

namespace Lx.Tools.Projects.SourceDump
{
    /// <summary>
    ///     Dump on Console the items compiled based on what's in the csproj passed in argument
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                DisplayUsage();
            }
            var options = ManageOptions(args);
            foreach (var file in args)
            {
                if (file == null)
                    continue;
                Project project;
                if (IsNotCsproj(file, out project))
                {
                    Console.Error.WriteLine(file + " is not a valid csproj. Ignored");
                    continue;
                }
                var directory = Path.GetDirectoryName(file);
                var sourceDumper = new SourceDumper(directory, options);
                var projectItems = project.GetItems("Compile").Select(x => x.EvaluatedInclude);
                var items = sourceDumper.Dump(projectItems);
                var fileName = Path.GetFileNameWithoutExtension(file);
                var newFile = fileName + ".dll.sources";
                using (var writer = (options.Contains(Options.OutputFile)) ? (IWriter)new FileWriter(Path.Combine(directory, newFile)) : new ConsoleWriter())
                {
                    foreach (var item in items)
                    {
                        writer.WriteLine(item);                       
                    }
                }
            }
            Exit(0);
        }

        private static HashSet<Option> ManageOptions(string[] args)
        {
            var activatedOptions = Options.GetOptions(args);
            if (activatedOptions.Contains(Options.Help))
            {
                DisplayUsage();
                DisplayHelp();
                Exit(0);
            }
            RemoveIncompatibleOptions(activatedOptions, Options.RelativePaths, Options.AbsolutePaths);
            RemoveIncompatibleOptions(activatedOptions, Options.UnixPaths, Options.WindowsPaths);
            return activatedOptions;
        }

        /// <summary>
        ///     Remove the incompatible option and keep the superseding one.
        /// </summary>
        /// <param name="managedOptions">The managed options.</param>
        /// <param name="supersedeOption">The supersede option.</param>
        /// <param name="incompatibleOption">The incompatible option.</param>
        private static void RemoveIncompatibleOptions(HashSet<Option> managedOptions, Option supersedeOption,
            Option incompatibleOption)
        {
            if (managedOptions.Contains(supersedeOption) && managedOptions.Contains(incompatibleOption))
            {
                Console.Error.WriteLine(incompatibleOption + " cannot be specified along with " + supersedeOption);
                Console.Error.WriteLine(supersedeOption + " take precedence");
                managedOptions.Remove(incompatibleOption);
            }
        }

        public static void Exit(int exitCode)
        {
            if (Debugger.IsAttached)
            {
                Console.ReadLine();
            }
            Environment.Exit(exitCode);
        }

        private static void DisplayHelp()
        {
            var nl = Environment.NewLine;
            var help = new StringBuilder();
            var maxLength = Options.AvailableOptions.Max(x => x.Name.Length);
            foreach (var option in Options.AvailableOptions)
            {
                help.AppendFormat("  {0}  {1}{2}", option.Name.PadRight(maxLength), option.Explanation,
                    Environment.NewLine);
            }
            Console.Write(help);
        }

        /// <summary>
        ///     Determines whether the specified file is not a valid csproj.
        ///     Files and imports should exists.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="project">The project.</param>
        /// <returns>true if it's not valid and false otherwise</returns>
        private static bool IsNotCsproj(string file, out Project project)
        {
            try
            {
                project = new Project(file);
                return false;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                project = null;
                return true;
            }
        }

        private static void DisplayUsage()
        {
            Console.WriteLine("Usage: source-dump [options] file.csproj [other csproj]");
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine("src-dump " + version);
            Console.WriteLine("Copyright (C) 2015 Leoxia Ltd");
        }
    }
}