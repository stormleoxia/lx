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
            usage.Arguments.Add(new Arguments {Name = "file.csproj"});
            usage.Arguments.Add(new Arguments {Name = "[other.csproj]"});
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