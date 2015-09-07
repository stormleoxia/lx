using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using IUnityContainer = Microsoft.Practices.Unity.IUnityContainer;
using UnityContainer = Microsoft.Practices.Unity.UnityContainer;
using UnityContainerExtensions = Microsoft.Practices.Unity.UnityContainerExtensions;

namespace Lx.Tools.Common.Tests.Program
{
    [TestFixture]
    public class ProgramTest
    {
        [SetUp]
        public void Setup()
        {
            _unityContainer = new UnityContainer();
            ConfigureContainer(_unityContainer);
            _processName = Process.GetCurrentProcess().ProcessName;
        }

        private IUnityContainer _unityContainer;
        private Mock<IEnvironment> _environment;
        private Mock<IConsole> _console;
        private Mock<IFileSystem> _fileSystem;
        private string _processName;

        private void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterMoqInstance<IWriterFactory>();
            container.RegisterMoqInstance<IVersion>();
            _environment = container.RegisterMoqInstance<IEnvironment>();
            container.RegisterMoqInstance<IDebugger>();
            _console = container.RegisterMoqInstance<IConsole>(MockBehavior.Strict);
            _fileSystem = container.RegisterMoqInstance<IFileSystem>();
        }

        [Test]
        public void ProgramDefinitionUsageTest()
        {
            var definition = UnityContainerExtensions.Resolve<MyDefinition>(_unityContainer);
            
            _console.Setup(x => x.WriteLine("Usage: " + _processName + " [options]"));
            _console.Setup(x => x.WriteLine(_processName + " "));
            _console.Setup(x => x.WriteLine("Copyright (C) 2015 Leoxia Ltd"));
            _console.Setup(x => x.Write(@"
  AvailableOptions:
  --help  Display this text"));
            definition.Run(new[] {"arg1", "--help", "arg2"});
            Assert.AreEqual(3, definition.ReceivedArguments.Length);
            Assert.AreEqual("arg1", definition.ReceivedArguments[0]);
            Assert.IsNull(definition.ReceivedArguments[1]);
            Assert.AreEqual("arg2", definition.ReceivedArguments[2]);
            Assert.AreEqual(1, definition.ReceivedOptions.Count);
            Assert.AreEqual(Options.Help, definition.ReceivedOptions.FirstOrDefault());
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void ExceptionTest()
        {
            var definition = UnityContainerExtensions.Resolve<MyExceptionDefinition>(_unityContainer);
            _console.Setup(x => x.WriteLine("Usage: " + _processName + " [options]"));
            _console.Setup(x => x.WriteLine(_processName + " "));
            _console.Setup(x => x.WriteLine("Copyright (C) 2015 Leoxia Ltd"));
            _console.Setup(x => x.Write(@"
  AvailableOptions:
  --help  Display this text"));
            definition.Run(new[] { "arg1", "--help", "arg2" });
            
        }
    }

    public class MyExceptionDefinition : ProgramDefinition
    {
        public MyExceptionDefinition(Options options, IEnvironment environment, IDebugger debugger, IConsole console, IVersion versionGetter) : base(options, new UsageDefinition(), environment, debugger, console, versionGetter)
        {
        }

        protected override HashSet<Option> InnerManageOptions(HashSet<Option> activatedOptions)
        {
            return new HashSet<Option>();
        }

        protected override void InnerRun(HashSet<Option> options, string[] args)
        {
            throw new InvalidOperationException();
        }


    }

    public class MyDefinition : ProgramDefinition
    {
        public MyDefinition(Options options, UsageDefinition definition, IEnvironment environment, IDebugger debugger,
            IConsole console, IVersion versionGetter)
            : base(options, definition, environment, debugger, console, versionGetter)
        {
        }

        public string[] ReceivedArguments { get; private set; }
        public HashSet<Option> ReceivedOptions { get; private set; }

        protected override HashSet<Option> InnerManageOptions(HashSet<Option> activatedOptions)
        {
            Assert.IsNotNull(activatedOptions);
            return activatedOptions;
        }

        protected override void InnerRun(HashSet<Option> options, string[] args)
        {
            ReceivedOptions = options;
            ReceivedArguments = args;
        }
    }


    public static class MoqContainerEx
    {
        public static Mock<TInterface> RegisterMoqInstance<TInterface>(this IUnityContainer container)
            where TInterface : class
        {
            var mock = new Mock<TInterface>();
            return RegisterInstance(container, mock);
        }

        private static Mock<TInterface> RegisterInstance<TInterface>(IUnityContainer container, Mock<TInterface> mock) where TInterface : class
        {
            container.RegisterInstance(typeof (TInterface), mock.Object);
            return mock;
        }

        public static Mock<TInterface> RegisterMoqInstance<TInterface>(this IUnityContainer container, MockBehavior behavior)
            where TInterface : class
        {
            var mock = new Mock<TInterface>(behavior);
            return RegisterInstance(container, mock); 
        }

    }
}