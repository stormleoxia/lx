using System.Reflection;
using System.Runtime.CompilerServices;
using Autofac;
using Lx.Tools.Common.Program;

namespace Lx.Tools.Common
{
    public class ProgramResolver
    {
        public static IContainer Container { get; set; }

        public static TProgram Resolve<TProgram>() where TProgram : ProgramDefinition
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<UsageDefinition>().SingleInstance();
            builder.RegisterType<TProgram>().SingleInstance();
            builder.RegisterType<Options>().SingleInstance();
            builder.RegisterAssemblyTypes(typeof (UsageDefinition).Assembly)
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterAssemblyTypes(Assembly.GetCallingAssembly())
                .AsImplementedInterfaces().SingleInstance();
            Container = builder.Build();
            return Container.Resolve<TProgram>();
        }
    }
}
