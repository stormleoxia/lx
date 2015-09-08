using System;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester
{
    internal class DelegateCallBenchmark : IBenchmark
    {
        private MethodContainer _container;
        private Action<MethodContainer> _delegate;

        public void Init()
        {
            _container = new MethodContainer();
            _delegate = (Action<MethodContainer>)typeof(MethodContainer).GetMethod("MethodCall").CreateDelegate(typeof(Action<MethodContainer>));
        }

        public void Call()
        {
            _delegate.Invoke(_container);
        }

        public void Cleanup()
        {

        }

        public string Name { get { return "Delegate Call Benchmark "; } }
    }
}