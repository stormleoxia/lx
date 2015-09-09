using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.Constructor
{
    internal class NewGenericConstructorBenchmark<T> : IBenchmark where T : new()
    {
        private T _constructible;
        public string Name { get; private set; }

        public NewGenericConstructorBenchmark()
        {
            Name = "Generic new";
        }

        public void Init()
        {
                
        }

        public void Call()
        {
            _constructible = new T();
        }

        public void Cleanup()
        {
        }
    }
}