using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester
{
    internal class InvokeMemberBenchmark : IBenchmark
    {
        private MethodContainer _container;
        private Type _type;
        private readonly object[] _emptyArgs = {};

        public void Init()
        {
            _container = new MethodContainer();
            _type = typeof(MethodContainer);
        }

        public void Call()
        {
            _type.InvokeMember("MethodCall", BindingFlags.InvokeMethod, null, _container, _emptyArgs);
        }

        public void Cleanup()
        {

        }

        public string Name { get { return "Invoke Member Benchmark";  } }
    }
}