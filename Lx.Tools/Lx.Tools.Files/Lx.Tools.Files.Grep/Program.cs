using Autofac;
using Lx.Tools.Common.Program;

namespace Lx.Tools.Files.Grep
{
        /// <summary>
    ///     Dump on Console the items compiled based on what's in the csproj passed in argument
    /// </summary>
    public class Program
    {
        static Program()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<UsageDefinition>().SingleInstance();
            builder.RegisterType<GrepMain>().SingleInstance();
            builder.RegisterType<Options>().SingleInstance();
            builder.RegisterAssemblyTypes(typeof (UsageDefinition).Assembly)
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .AsImplementedInterfaces().SingleInstance();
            Container = builder.Build();
        }

        public static IContainer Container { get; set; }

        public static void Main(string[] args)
        {
            var usage = Container.Resolve<UsageDefinition>();
            usage.Arguments.Add(new Argument {Name = "regular_expression"});
            usage.Arguments.Add(new Argument {Name = "directory"});
            usage.Arguments.Add(new Argument {Name = "[ other_directory ]"});
            var program = Container.Resolve<GrepMain>();
            program.Run(args);
        }

    }
}

