using System;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.Constructor
{
    internal class ActivatorBenchmark : IBenchmark
    {
        private Constructible _constructible;
        public string Name { get; private set; }

        public ActivatorBenchmark()
        {
            Name = "Activator";
        }

        public void Init()
        {
            
        }

        public void Call()
        {
            _constructible = (Constructible)Activator.CreateInstance(typeof (Constructible));
        }

        public void Cleanup()
        {
        }
    }
}