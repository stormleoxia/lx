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

using Lx.Tools.Common;
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.Sync;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using IUnityContainer = Microsoft.Practices.Unity.IUnityContainer;
using UnityContainer = Microsoft.Practices.Unity.UnityContainer;
using UnityContainerExtensions = Microsoft.Practices.Unity.UnityContainerExtensions;

namespace Lx.Tools.Projects.Tests.Sync
{
    [TestFixture]
    public class ProgramTest
    {
        private Mock<ISourcesProvider> _sourceFinder;
        private Mock<IProjectFactory> _projectFactory;
        private Mock<ISourceComparer> _sourceComparer;
        private Mock<ISyncFactory> _syncFactory;
        private Mock<IFileSystem> _fileSystem;
        private Mock<IEnvironment> _environment;
        private Mock<IConsole> _console;
        private Mock<IDirectoryValidator> _validator;
        private Mock<IProjectSyncConfiguration> _configuration;

        private void ConfigureContainer(UnityContainer container)
        {
            _validator = container.RegisterMoqInstance<IDirectoryValidator>();
            _configuration = container.RegisterMoqInstance<IProjectSyncConfiguration>();
            _sourceFinder = container.RegisterMoqInstance<ISourcesProvider>();
            _projectFactory = container.RegisterMoqInstance<IProjectFactory>();
            _sourceComparer = container.RegisterMoqInstance<ISourceComparer>();
            _syncFactory = container.RegisterMoqInstance<ISyncFactory>();
            container.RegisterMoqInstance<IWriterFactory>();
            container.RegisterMoqInstance<IVersion>();
            _environment = container.RegisterMoqInstance<IEnvironment>();
            container.RegisterMoqInstance<IDebugger>();
            _console = container.RegisterMoqInstance<IConsole>();
            _fileSystem = container.RegisterMoqInstance<IFileSystem>();
        }

        [Test]
        public void MainTest()
        {
            var mockContainer = new Mock<IUnityContainer>();
            var unityContainer = new UnityContainer();
            var usageDefinition = unityContainer.Resolve<UsageDefinition>();
            mockContainer.Setup(x => x.Resolve(typeof (UsageDefinition), null)).Returns(usageDefinition);

            ConfigureContainer(unityContainer);

            var programSync = unityContainer.Resolve<ProgramSync>();
            mockContainer.Setup(x => x.Resolve(typeof (ProgramSync), null)).Returns(programSync);

            Program.Container = mockContainer.Object;
            Program.Main(new[] {"directory", "csproj"});
        }
    }

    public static class MoqContainerEx
    {
        public static Mock<TInterface> RegisterMoqInstance<TInterface>(this IUnityContainer container)
            where TInterface : class
        {
            var mock = new Mock<TInterface>();
            container.RegisterInstance(typeof (TInterface), mock.Object);
            return mock;
        }
    }
}