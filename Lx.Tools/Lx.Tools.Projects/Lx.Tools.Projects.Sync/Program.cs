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

using Lx.Tools.Common;
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;
using Microsoft.Practices.Unity;

namespace Lx.Tools.Projects.Sync
{
    /// <summary>
    ///     Detects discrepencies between csproj and sources files.
    /// </summary>
    public class Program
    {
        static Program()
        {
            Container = new UnityContainer();
        }

        public static IUnityContainer Container { get; set; }

        public static void Main(string[] args)
        {
            ContainerConfigure(Container);
            var usage = Container.Resolve<UsageDefinition>();
            usage.Arguments.Add(new Argument {Name = "file.csproj"});
            usage.Arguments.Add(new Argument {Name = "[other.csproj]"});
            Container.RegisterInstance(usage, new ContainerControlledLifetimeManager());
            var program = Container.Resolve<ProgramSync>();
            program.Run(args);
        }

        public static void ContainerConfigure(IUnityContainer container)
        {
            container.RegisterType<ISettingsProvider, SettingsProvider>(new ContainerControlledLifetimeManager());
            container.RegisterType<IProjectSyncConfiguration, ProjectSyncConfiguration>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<IDirectoryValidator, DirectoryValidator>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISourcesProvider, SourcesProvider>(new ContainerControlledLifetimeManager());
            container.RegisterType<IProjectFactory, ProjectFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISourceComparer, SourceComparer>(new ExternallyControlledLifetimeManager());
            container.RegisterType<ISyncFactory, SyncFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<IProject, ProjectWrapper>(new ContainerControlledLifetimeManager());
            container.RegisterType<IWriterFactory, WriterFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<IVersion, VersionGetter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IEnvironment, SystemEnvironment>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDebugger, SystemDebugger>();
            container.RegisterType<IConsole, SystemConsole>();
            container.RegisterType<IFileSystem, FileSystem>();
        }
    }
}