using System.Collections.Generic;
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.SourceDump
{
    /// <summary>
    ///     Repository for all available options.
    /// </summary>
    public class SourceDumperOptions : Options
    {
        static SourceDumperOptions()
        {
            UnixPaths = new Option {Name = "--unix-paths", Explanation = "Convert source path to unix paths"};
            WindowsPaths = new Option {Name = "--windows-paths", Explanation = "Convert source path to windows paths"};
            RelativePaths = new Option
            {
                Name = "--relative-paths",
                Explanation = "Convert absolute path to relative ones"
            };
            AbsolutePaths = new Option
            {
                Name = "--absolute-paths",
                Explanation = "Convert relative paths to absolute ones"
            };
            OutputFile = new Option
            {
                Name = "--output-file",
                Explanation = "Writes output in a file <csproj>.dll.sources in the same directory of the project file"
            };
        }

        public SourceDumperOptions(IEnvironment environment, IConsole console)
            : base(environment, console)
        {
            AvailableOptions.AddRange(new List<Option>
            {
                UnixPaths,
                WindowsPaths,
                RelativePaths,
                AbsolutePaths,
                OutputFile
            });
        }

        public static Option UnixPaths { get; private set; }
        public static Option WindowsPaths { get; private set; }
        public static Option RelativePaths { get; private set; }
        public static Option AbsolutePaths { get; private set; }
        public static Option OutputFile { get; private set; }
    }
}