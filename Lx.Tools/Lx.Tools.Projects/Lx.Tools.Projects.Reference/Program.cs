using System.Diagnostics;
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;
using Microsoft.Practices.Unity;

namespace Lx.Tools.Projects.Reference
{
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
            var usage = Container.Resolve<ReferenceManagerDefinition>();
            Container.RegisterInstance(usage, new ContainerControlledLifetimeManager());
            var program = Container.Resolve<ReferenceManager>();
            program.Run(args);
        }

        public static void ContainerConfigure(IUnityContainer container)
        {
            container.RegisterInstance(container, new ContainerControlledLifetimeManager());
            container.RegisterType<IWriterFactory, WriterFactory>();
            container.RegisterType<IVersion, VersionGetter>();
            container.RegisterType<IEnvironment, SystemEnvironment>();
            container.RegisterType<IDebugger, SystemDebugger>();
            container.RegisterType<IConsole, SystemConsole>();
            container.RegisterType<IFileSystem, FileSystem>();
            container.RegisterType<IProject, ProjectWrapper>(new ExternallyControlledLifetimeManager());
            container.RegisterType<IReferenceAdder, ReferenceAdder>(new ContainerControlledLifetimeManager());
        }
    }
}
