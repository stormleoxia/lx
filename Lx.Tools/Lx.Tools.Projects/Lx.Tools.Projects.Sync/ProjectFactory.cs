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

namespace Lx.Tools.Projects.Sync
{
    public sealed class ProjectFactory : IProjectFactory
    {
        private readonly IProjectSyncConfiguration _configuration;
        private readonly IConsole _console;
        private readonly IFileSystem _fileSystem;
        private readonly IDictionary<string, IProject> _projects = new Dictionary<string, IProject>();

        public ProjectFactory(IConsole console, IFileSystem fileSystem, IProjectSyncConfiguration configuration)
        {
            _console = console;
            _fileSystem = fileSystem;
            _configuration = configuration;
        }

        public bool IsValidProject(string projectPath)
        {
            try
            {
                CreateProject(projectPath);
                var directory = Path.GetDirectoryName(projectPath);
                var files = _fileSystem.GetFiles(directory, "*.sources", SearchOption.TopDirectoryOnly);
                return files.Length > 0;
            }
            catch (Exception e)
            {
                _console.WriteLine(e.Message);
            }
            return false;
        }

        public IProjectItemsProvider CreateProjectItemsProvider(string projectPath)
        {
            return new ProjectItemsProvider(CreateProject(projectPath));
        }

        public IProjectUpdater CreateProjectUpdater(string projectPath)
        {
            return new ProjectUpdater(CreateProject(projectPath), _fileSystem, _configuration);
        }

        public ISourcesProvider CreateSourcesProvider(string projectPath)
        {
            return new SourcesProvider(projectPath, _fileSystem, _console);
        }

        public ISourceComparer CreateSourceComparer()
        {
            return new SourceComparer();
        }

        internal IProject CreateProject(string projectPath)
        {
            IProject project;
            if (!_projects.TryGetValue(projectPath, out project))
            {
                project = new ProjectWrapper(projectPath);
                _projects[projectPath] = project;
            }
            return project;
        }
    }
}