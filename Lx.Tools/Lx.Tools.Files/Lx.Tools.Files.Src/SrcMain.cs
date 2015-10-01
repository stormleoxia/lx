using System;
using System.Collections.Generic;
using System.IO;
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Files.Src
{
    public class SrcMain : ProgramDefinition
    {
        public SrcMain(Options options, UsageDefinition definition, IEnvironment environment, IDebugger debugger, IConsole console, IVersion versionGetter) :
            base(options, definition, environment, debugger, console, versionGetter)
        {
        }

        protected override HashSet<Option> InnerManageOptions(HashSet<Option> activatedOptions)
        {
            return activatedOptions;
        }

        protected override void InnerRun(HashSet<Option> options, string[] args)
        {
            bool first = true;
            Dictionary<string, string> referenceFiles = new Dictionary<string, string>();
            foreach (var argument in args)
            {
                if (!string.IsNullOrEmpty(argument))
                {
                    if (first)
                    {
                        first = false;
                        referenceFiles = ExploreFile(argument);
                    }
                    else
                    {
                        Dictionary<string, string> currentFiles = ExploreFile(argument);
                        foreach (var file in currentFiles)
                        {
                            if (!referenceFiles.ContainsKey(file.Key))
                            {
                                referenceFiles.Add(file.Key, file.Value);
                            }
                        }
                    }
                }
            }
            foreach (var pair in referenceFiles)
            {
                Console.WriteLine(pair.Value);
            }
        }

        private Dictionary<string, string> ExploreFile(string argument)
        {
            Dictionary<string, string> result  = new Dictionary<string, string>();
            using (var stream = File.OpenText(argument))
            {
                var res = stream.ReadToEnd();
                var lines = res.Split('\n', '\r');
                foreach (var line in lines)
                {
                    var key = Path.GetFileName(line);
                    if (!string.IsNullOrEmpty(key))
                    {
                        var lower = key.ToLower();
                        if (!result.ContainsKey(lower))
                        {
                            result.Add(lower, line);
                        }
                    }
                }
            }
            return result;
        }
    }
}