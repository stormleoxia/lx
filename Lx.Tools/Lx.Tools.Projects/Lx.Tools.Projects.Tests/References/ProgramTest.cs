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
using System.Runtime.CompilerServices;
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.Reference;
using Lx.Tools.Tests.MockUnity;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.References
{
    [TestFixture]
    public class ProgramTest : MockUnitTestFixture
    {
        private Mock<IConsole> _console;
        private Mock<IEnvironment> _environment;
        private Mock<IFileSystem> _fileSystem;
        private Mock<IWriter> _writer;

        private void ConfigureContainer(UnityContainer container)
        {
            _writer = new Mock<IWriter>();
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
            var mockContainer = new Mock<IUnityContainer>(MockBehavior.Strict);
            var unityContainer = new UnityContainer();
            var usageDefinition = unityContainer.Resolve<UsageDefinition>();
            mockContainer.Setup(
                x =>
                    x.RegisterInstance(typeof (IUnityContainer), null, mockContainer.Object, It.IsAny<LifetimeManager>()))
                .Returns(mockContainer.Object);
            mockContainer.Setup(x => x.Resolve(typeof (UsageDefinition), null)).Returns(usageDefinition);
            mockContainer.Setup(x => x.RegisterType(It.IsAny<Type>(), It.IsAny<Type>(), It.IsAny<string>(), null))
                .Callback<Type, Type, string, LifetimeManager, InjectionMember[]>(
                    (x, y, v, z, w) => { RegisterType(mockContainer, unityContainer, x); })
                .Returns(mockContainer.Object);
            mockContainer.Setup(
                x =>
                    x.RegisterType(typeof (IProject), typeof (ProjectWrapper), null, It.IsAny<LifetimeManager>()))
                .Returns(mockContainer.Object);
            mockContainer.Setup(
                x =>
                    x.RegisterType(typeof (IReferenceAdder), typeof (ReferenceAdder), null, It.IsAny<LifetimeManager>()))
                .Returns(mockContainer.Object);

            ConfigureContainer(unityContainer);

            var programRef = unityContainer.Resolve<ReferenceManager>();
            Assert.IsNotNull(programRef);
            mockContainer.Setup(x => x.Resolve(typeof (ReferenceManager), null)).Returns(programRef);

            var definition = unityContainer.Resolve<ReferenceManagerDefinition>();
            Assert.IsNotNull(definition);
            mockContainer.Setup(
                x =>
                    x.RegisterInstance(definition.GetType(), null, definition,
                        It.IsAny<ContainerControlledLifetimeManager>())).Returns(mockContainer.Object);
            mockContainer.Setup(x => x.Resolve(typeof (ReferenceManagerDefinition), null)).Returns(definition);


            _console.Setup(x => x.Error).Returns(_writer.Object);

            Program.Container = mockContainer.Object;
            Program.Main(new[] {"directory", "csproj"});
        }

        private void RegisterType(Mock<IUnityContainer> mockContainer, UnityContainer unityContainer, Type interfaceType)
        {
            var mock = unityContainer.RegisterMoqInstance(interfaceType);
            mockContainer.Setup(x => x.Resolve(interfaceType, null)).Returns(mock);
        }
    }
}