using System;
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
            mockContainer.Setup(x => x.Resolve(typeof (UsageDefinition), null)).Returns(usageDefinition);
            mockContainer.Setup(x => x.RegisterType(It.IsAny<Type>(), It.IsAny<Type>(), It.IsAny<string>(), null, new InjectionMember[0]))
                .Callback<Type, Type, string, LifetimeManager, InjectionMember[]>((x, y, v, z, w) =>
                {
                    RegisterType(mockContainer, unityContainer, x);
                }).Returns(mockContainer.Object);
            ConfigureContainer(unityContainer);

            var programRef = unityContainer.Resolve<ReferenceManager>();
            Assert.IsNotNull(programRef);
            mockContainer.Setup(x => x.Resolve(typeof (ReferenceManager), null, new ResolverOverride[0])).Returns(programRef);

            var definition = unityContainer.Resolve<ReferenceManagerDefinition>();
            Assert.IsNotNull(definition);
            mockContainer.Setup(x => x.RegisterInstance(definition.GetType(), null, definition, It.IsAny<ContainerControlledLifetimeManager>())).Returns(mockContainer.Object);
            mockContainer.Setup(x => x.Resolve(typeof (ReferenceManagerDefinition), null, new ResolverOverride[0])).Returns(definition);


            _console.Setup(x => x.Error).Returns(_writer.Object);

            Program.Container = mockContainer.Object;
            Program.Main(new[] {"directory", "csproj"});
        }

        private void RegisterType(Mock<IUnityContainer> mockContainer, UnityContainer unityContainer, Type interfaceType)
        {
            var mock = unityContainer.RegisterMoqInstance(interfaceType);
            mockContainer.Setup(x => x.Resolve(interfaceType, null, new ResolverOverride[0])).Returns(mock);
        }
    }
}

