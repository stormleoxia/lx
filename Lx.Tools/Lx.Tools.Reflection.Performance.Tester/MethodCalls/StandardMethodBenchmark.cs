using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester
{
    internal class StandardMethodBenchmark : IBenchmark
    {
        private MethodContainer _container;

        public void Init()
        {
            _container = new MethodContainer();
        }

        public void Call()
        {
            _container.MethodCall();
        }

        public void Cleanup()
        {
            
        }

        public string Name { get { return "Standard Method Call"; } }
    }
}