#region Copyright (c) 2015 Leoxia Ltd

// #region Copyright (c) 2015 Leoxia Ltd
// 
// // Copyright © 2015 Leoxia Ltd.
// // 
// // This file is part of Lx.
// //
// // Lx is released under GNU General Public License unless stated otherwise.
// // You may not use this file except in compliance with the License.
// // You can redistribute it and/or modify it under the terms of the GNU General Public License 
// // as published by the Free Software Foundation, either version 3 of the License, 
// // or any later version.
// // 
// // In case GNU General Public License is not applicable for your use of Lx, 
// // you can subscribe to commercial license on 
// // http://www.leoxia.com 
// // by contacting us through the form page or send us a mail
// // mailto:contact@leoxia.com
// //  
// // Unless required by applicable law or agreed to in writing, 
// // Lx is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES 
// // OR CONDITIONS OF ANY KIND, either express or implied. 
// // See the GNU General Public License for more details.
// //
// // You should have received a copy of the GNU General Public License along with Lx.
// // It is present in the Lx root folder SolutionItems/GPL.txt
// // If not, see http://www.gnu.org/licenses/.
// //
// 
// #endregion

#endregion

using System.Collections.Generic;
using System.IO;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    internal class SourcesProvider : ISourcesProvider
    {
        private readonly IConsole _console;
        private readonly string _directory;
        private readonly IFileSystem _fileSystem;
        private readonly string _sourceFile;

        public SourcesProvider(string projectFilePath, IFileSystem fileSystem, IConsole console)
        {
            _fileSystem = fileSystem;
            _console = console;
            _directory = Path.GetDirectoryName(projectFilePath);
            var sourceFileFinder = new SourceFileFinder(projectFilePath, fileSystem);
            _sourceFile = sourceFileFinder.FindSourcesFile();
        }

        public HashSet<string> GetFiles()
        {
            var res = new HashSet<string>();
            ReadSourceFile(res, _sourceFile);
            return res;
        }

        private void ReadSourceFile(HashSet<string> res, string file)
        {
            var result = string.Empty;
            if (!string.IsNullOrEmpty(file))
            {
                var filePath = Path.Combine(_directory, file);
                using (var reader = _fileSystem.OpenText(filePath))
                {
                    result = reader.ReadToEnd();
                }
            }
            var results = result.Split('\n', '\r');
            var includes = new List<string>();
            foreach (var line in results)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    if (IsInclude(line))
                    {
                        includes.Add(GetInclude(line));
                    }
                    else
                    {
                        // Cannot check for existence as there is no distinction between normal sources and generated                        
                        res.Add(line);
                    }
                }
            }
            foreach (var include in includes)
            {
                ReadSourceFile(res, include);
            }
        }

        private string GetInclude(string line)
        {
            return line.Replace("#include", string.Empty).Trim();
        }

        private static bool IsInclude(string line)
        {
            if (line.StartsWith(" "))
            {
                return true;
            }
            return line.TrimStart(' ', '\t').StartsWith("#include");
        }
    }
}