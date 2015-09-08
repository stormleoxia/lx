#region Copyright (c) 2015 Leoxia Ltd

//  Copyright © 2015 Leoxia Ltd
//  
//  This file is part of Lx.
// 
//  Lx is released under GNU General Public License unless stated otherwise.
//  You may not use this file except in compliance with the License.
//  You can redistribute it and/or modify it under the terms of the GNU General Public License 
//  as published by the Free Software Foundation, either version 3 of the License, 
//  or any later version.
//  
//  In case GNU General Public License is not applicable for your use of Lx, 
//  you can subscribe to commercial license on 
//  http://www.leoxia.com 
//  by contacting us through the form page or send us a mail
//  mailto:contact@leoxia.com
//   
//  Unless required by applicable law or agreed to in writing, 
//  Lx is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES 
//  OR CONDITIONS OF ANY KIND, either express or implied. 
//  See the GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License along with Lx.
//  It is present in the Lx root folder SolutionItems/GPL.txt
//  If not, see http://www.gnu.org/licenses/.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Lx.Tools.Common;
using Lx.Tools.Common.Program;

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
                if (_options.Contains(SourceDumperOptions.UnixPaths))
                {
                    res = res.Replace('\\', '/');
                }
                if (_options.Contains(SourceDumperOptions.RelativePaths))
                {
                    UPath file;
                    if (IsPathRooted(res))
                    {
                        if (res.Contains(':') && _referenceDirectory.Contains(':'))
                        {
                            if (_referenceDirectory[0] != res[0])
                            {
                                throw new InvalidOperationException("csproj in " + _referenceDirectory +
                                                                    " is not on the same drive that " + res);
                            }
                        }
                        file = new UPath(res);
                    }
                    else
                    {
                        file = new UPath(_referenceDirectory, res);
                    }
                    res = _directoryPath.MakeRelativeUPath(file).ToString();
                }
                if (_options.Contains(SourceDumperOptions.WindowsPaths))
                {
                    res = res.Replace('/', '\\');
                }
                if (_options.Contains(SourceDumperOptions.AbsolutePaths))
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
            return res.Contains(':') || res[0] == '/' || res[0] == '\\';
        }
    }
}