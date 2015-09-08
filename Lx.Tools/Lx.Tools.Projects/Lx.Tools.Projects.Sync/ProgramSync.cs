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
using System.Linq;
using Lx.Tools.Common;
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    public sealed class ProgramSync : ProgramDefinition
    {
        private readonly IProjectSyncConfiguration _configuration;
        private readonly ISyncFactory _factory;
        private readonly IFileSystem _fileSystem;
        private readonly IProjectFactory _projectFactory;
        private readonly IDirectoryValidator _validator;

        public ProgramSync(ISyncFactory factory, IFileSystem fileSystem, ProgramOptions options,
            IProjectSyncConfiguration configuration,
            UsageDefinition definition, IDirectoryValidator validator, IProjectFactory projectFactory,
            IEnvironment environment, IDebugger debugger, IConsole console, IVersion versionGetter) :
                base(options, definition, environment, debugger, console, versionGetter)
        {
            _factory = factory;
            _fileSystem = fileSystem;
            _configuration = configuration;
            _validator = validator;
            _projectFactory = projectFactory;
        }

        protected override HashSet<Option> InnerManageOptions(HashSet<Option> activatedOptions)
        {
            _configuration.Options = activatedOptions;
            return activatedOptions;
        }

        protected override void InnerRun(HashSet<Option> options, string[] args)
        {
            foreach (var arg in args)
            {
                if (_validator.IsDirectoryValid(arg))
                {
                    if (IsCsProj(arg) && _projectFactory.IsValidProject(arg))
                    {
                        var sync = _factory.CreateProjectSynchronizer(arg);
                        sync.Synchronize();
                    }
                    else if (IsDirectory(arg))
                    {
                        var sync = _factory.CreateDirectorySynchronizer(arg);
                        sync.Synchronize();
                    }
                }
            }
        }

        private bool IsDirectory(string directoryPath)
        {
            return _fileSystem.DirectoryExists(directoryPath);
        }

        private bool IsCsProj(string path)
        {
            if (_fileSystem.FileExists(path) && Path.GetExtension(path) == ".csproj")
            {
                return true;
            }
            return false;
        }
    }

    public interface IDirectoryValidator
    {
        bool IsDirectoryValid(string path);
    }

    public sealed class DirectoryValidator : IDirectoryValidator
    {
        private readonly IProjectSyncConfiguration _configuration;

        public DirectoryValidator(IProjectSyncConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool IsDirectoryValid(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            return _configuration.IgnoredDirectories.All(
                dir => !path.ToLower().ToPlatformPath().Contains(dir.ToLower().ToPlatformPath()));
        }
    }
}