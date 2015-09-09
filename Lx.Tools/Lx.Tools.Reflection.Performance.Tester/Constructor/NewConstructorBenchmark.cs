using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.Constructor
{
    internal class NewConstructorBenchmark : IBenchmark
    {
        private Constructible _constructible;
        public string Name { get; private set; }

        public NewConstructorBenchmark()
        {
            Name = "New";
        }

        public void Init()
        {
            
        }

        public void Call()
        {
            _constructible = new Constructible();
        }

        public void Cleanup()
        {
        }
    }
}