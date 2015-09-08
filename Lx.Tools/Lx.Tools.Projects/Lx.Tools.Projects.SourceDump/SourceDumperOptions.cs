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