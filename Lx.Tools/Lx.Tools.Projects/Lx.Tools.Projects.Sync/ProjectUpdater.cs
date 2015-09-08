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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Lx.Tools.Common;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    public class ProjectUpdater : IProjectUpdater
    {
        private readonly IProjectSyncConfiguration _configuration;
        private readonly IFileSystem _fileSystem;
        private readonly IProject _project;

        public ProjectUpdater(IProject project, IFileSystem fileSystem, IProjectSyncConfiguration configuration)
        {
            _project = project;
            _fileSystem = fileSystem;
            _configuration = configuration;
        }

        /// <summary>
        ///     Add missing items in the project and removes items not found in source file.
        ///     Note that there are some files which are generated, so file existence should not be checked.
        /// </summary>
        /// <param name="comparison">The comparison.</param>
        public void Update(SourceComparison comparison)
        {
            Cleanup();
            if (!_configuration.Options.Contains(ProgramOptions.NoDelete))
            {
                var hash = comparison.MissingFilesInSource.Select(x => x.Path.ToLower()).ToHashSet();
                var items = _project.GetItems("Compile").ToArray();
                foreach (var item in items)
                {
                    if (hash.Contains(item.EvaluatedInclude.Replace('\\', '/').ToLower()))
                    {
                        _project.RemoveItem(item);
                    }
                }
            }
            var directory = Path.GetDirectoryName(_project.FullPath);
            foreach (var item in comparison.MissingFilesInProject)
            {
                var fileName = Path.Combine(directory, item.Path);
                var path = new UPath(directory);
                var fileUPath = new UPath(fileName);
                var res = path.MakeRelativeUPath(fileUPath);
                _project.AddItem("Compile", res.ToString());
            }

            _project.Save();
        }

        private void Cleanup()
        {
            var items = _project.GetItems("Compile").ToArray();
            var duplicates = items.Select(x => x.EvaluatedInclude.ToLower().ToPlatformPath().Trim().RemoveDotPath())
                .GroupBy(x => x)
                .Where(g => g.Count() > 1).Select(x => x.Key).ToHashSet();
            foreach (var item in items)
            {
                var comparable = item.EvaluatedInclude.ToLower().ToPlatformPath().Trim().RemoveDotPath();
                if (duplicates.Contains(comparable))
                {
                    _project.RemoveItem(item);
                    duplicates.Remove(comparable);
                }
            }
        }
    }
}