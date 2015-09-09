using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.Constructor
{
    class ConstructorBenchmarkGroup : IBenchmarkGroup
    {
        public ConstructorBenchmarkGroup()
        {
            Name = "Construction";
            Iterations = 500000;
            Benchmarks = new List<IBenchmark>
            {
                new NewConstructorBenchmark(), 
                new NewGenericConstructorBenchmark<Constructible>(),
                new ActivatorBenchmark(),
                new GenericActivatorBenchmark()
            };
        }

        public IEnumerable<IBenchmark> Benchmarks { get; private set; }
        public decimal Iterations { get; private set; }
        public string Name { get; private set; }
    }
}
