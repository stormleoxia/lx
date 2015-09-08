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
using System.IO;
using System.Linq;
using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.Sync.FilterSteps;

namespace Lx.Tools.Projects.Sync
{
    public static class ScopesEx
    {
        static ScopesEx()
        {
            AllValues = new HashSet<string>(Enum.GetNames(typeof (Scopes)));
        }

        public static HashSet<string> AllValues { get; private set; }
    }

    internal class SourceFileFinder
    {
        private static readonly List<FilterStep> _steps = new List<FilterStep>
        {
            new TestFilterStep(),
            new TargetFilterStep(),
            new ScopeFilterStep(),
            new NameFilterStep(),
            new NotSupportedTargetStep()
        };

        private readonly string _directory;
        private readonly IFileSystem _fileSystem;
        private readonly string _projectFilePath;

        public SourceFileFinder(string projectFilePath, IFileSystem fileSystem)
        {
            _projectFilePath = projectFilePath;
            _directory = Path.GetDirectoryName(_projectFilePath);
            _fileSystem = fileSystem;
        }

        public string FindSourcesFile()
        {
            var files =
                _fileSystem.GetFiles(_directory, "*.sources", SearchOption.TopDirectoryOnly)
                    .Select(Path.GetFileName)
                    .ToArray();
            if (files.Length == 0)
            {
                throw new InvalidOperationException("No sources found for " + _projectFilePath);
            }
            var attributes = new ProjectAttributes(_projectFilePath);
            var currentStep = 0;
            while (files.Length != 1 && currentStep < _steps.Count)
            {
                var res = _steps[currentStep].Filter(files, attributes);
                if (res.Length > 0) // Discard any filter that removed all values
                {
                    files = res;
                }
                ++currentStep;
            }
            if (files.Length != 1)
            {
                throw new InvalidOperationException("Unable to find unique source for " + _projectFilePath);
            }
            return files.First();
        }
    }
}