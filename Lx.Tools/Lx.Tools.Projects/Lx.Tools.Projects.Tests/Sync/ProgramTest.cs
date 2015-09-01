using Lx.Tools.Common;
using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.Sync;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;

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

        private void ConfigureContainer(UnityContainer container)
        {
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