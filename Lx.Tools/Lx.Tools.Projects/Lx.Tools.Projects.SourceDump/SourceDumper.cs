﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lx.Tools.Common;

namespace Lx.Tools.Projects.SourceDump
{
    public class SourceDumper
    {
        private readonly UPath _directoryPath;
        private readonly HashSet<Option> _options;
        private readonly string _referenceDirectory;

        public SourceDumper(string referenceDirectory, HashSet<Option> options)
        {
            _referenceDirectory = referenceDirectory;
            _directoryPath = new UPath(_referenceDirectory);

            _options = options;
        }

        public IEnumerable<string> Dump(IEnumerable<string> sources)
        {
            var list = new List<string>();
            foreach (var source in sources)
            {
                var res = source;
                if (_options.Contains(Options.UnixPaths))
                {
                    res = res.Replace('\\', '/');
                }
                if (_options.Contains(Options.RelativePaths))
                {
                    if (IsPathRooted(res))
                    {
                        if (res.Contains(':'))
                        {
                            if (_referenceDirectory[0] != res[0])
                            {
                                Console.Error.WriteLine("csproj in " + _referenceDirectory +
                                                        " is not on the same drive that " + res);
                                Console.Error.WriteLine("Abort.");
                                Program.Exit(0);
                            }
                        }
                    }
                    var file = new UPath(_referenceDirectory, res);
                    res = _directoryPath.MakeRelativeUPath(file).ToString();
                }
                if (_options.Contains(Options.WindowsPaths))
                {
                    res = res.Replace('/', '\\');
                }
                if (_options.Contains(Options.AbsolutePaths))
                {
                    if (!IsPathRooted(res)) // if path is rooted
                    {
                        var file = new UPath(res);
                        if (file.HasAbsolute)
                        {
                            res = file.Absolute.ToString();
                        }
                    }
                }
                list.Add(res);
            }
            return list;
        }

        private static bool IsPathRooted(string res)
        {
            return res.Contains(':') || res[0] == '/';
        }
    }
}