using System;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.Constructor
{
    internal class GenericActivatorBenchmark : IBenchmark
    {
        private Constructible _constructible;
        public string Name { get; private set; }

        public GenericActivatorBenchmark()
        {
            Name = "Generic Activator";
        }

        public void Init()
        {
        }

        public void Call()
        {
            _constructible = Activator.CreateInstance<Constructible>();
        }

        public void Cleanup()
        {
        }
    }
}