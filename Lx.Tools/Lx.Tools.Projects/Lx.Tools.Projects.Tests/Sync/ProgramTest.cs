﻿using Lx.Tools.Common;
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

        private void ConfigureContainer(UnityContainer container)
        {
            _validator = container.RegisterMoqInstance<IDirectoryValidator>();
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
            var usageDefinition = UnityContainerExtensions.Resolve<UsageDefinition>(unityContainer);
            mockContainer.Setup(x => x.Resolve(typeof (UsageDefinition), null)).Returns(usageDefinition);

            ConfigureContainer(unityContainer);

            var programSync = UnityContainerExtensions.Resolve<ProgramSync>(unityContainer);
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