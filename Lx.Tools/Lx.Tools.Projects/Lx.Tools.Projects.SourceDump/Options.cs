using System.Collections.Generic;

namespace Lx.Tools.Projects.SourceDump
{
    /// <summary>
    ///     Repository for all available options.
    /// </summary>
    public class Options
    {
        static Options()
        {
            Help = new Option {Name = "--help", Explanation = "Display this text"};
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
            AvailableOptions = new List<Option>
            {
                Help,
                UnixPaths,
                WindowsPaths,
                RelativePaths,
                AbsolutePaths,
                OutputFile
            };
        }

        public static Option UnixPaths { get; private set; }
        public static Option WindowsPaths { get; private set; }
        public static Option RelativePaths { get; private set; }
        public static Option AbsolutePaths { get; private set; }
        public static Option Help { get; private set; }
        public static List<Option> AvailableOptions { get; private set; }
        public static Option OutputFile { get; private set; }

        public static HashSet<Option> GetOptions(string[] arguments)
        {
            var list = new HashSet<Option>();
            for (int index = 0; index < arguments.Length; index++)
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
    }
}