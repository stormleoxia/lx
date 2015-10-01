using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Lx.Tools.Common.Program;

namespace Lx.Tools.Files.Src
{
    class Program
    {
        static Program()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<UsageDefinition>().SingleInstance();
            builder.RegisterType<SrcMain>().SingleInstance();
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
            usage.Arguments.Add(new Argument {Name = "reference_source"});
            usage.Arguments.Add(new Argument {Name = "compared_source"});
            var program = Container.Resolve<SrcMain>();
            program.Run(args);
        }
    }
}
