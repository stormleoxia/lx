using Lx.Tools.Common;
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;
using Microsoft.Practices.Unity;

namespace Lx.Tools.Projects.SourceDump
{
    /// <summary>
    ///     Dump on Console the items compiled based on what's in the csproj passed in argument
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
            var program = Container.Resolve<SourceDump>();
            program.Run(args);
        }

        public static void ContainerConfigure(IUnityContainer container)
        {
            container.RegisterType<IWriterFactory, WriterFactory>();
            container.RegisterType<IVersion, VersionGetter>();
            container.RegisterType<IEnvironment, SystemEnvironment>();
            container.RegisterType<IDebugger, SystemDebugger>();
            container.RegisterType<IConsole, SystemConsole>();
            container.RegisterType<IFileSystem, FileSystem>();
        }
    }
}