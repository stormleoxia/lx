using System.Reflection;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester
{
    internal class MethodInvokeBenchmark : IBenchmark
    {
        private MethodContainer _container;
        private readonly MethodInfo _methodInfo;
        private readonly object[] _emptyArgs = {};

        public MethodInvokeBenchmark()
        {
            _methodInfo = typeof (MethodContainer).GetMethod("MethodCall");
        }

        public void Init()
        {
            _container = new MethodContainer();
        }

        public void Call()
        {
            _methodInfo.Invoke(_container, _emptyArgs);
        }

        public void Cleanup()
        {

        }

        public string Name { get { return "Invoke Method Call"; } }
    }
}