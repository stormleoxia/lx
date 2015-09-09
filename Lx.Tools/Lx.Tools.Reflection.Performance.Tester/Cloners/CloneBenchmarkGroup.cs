using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.Cloners
{
    class CloneBenchmarkGroup : IBenchmarkGroup
    {
        public CloneBenchmarkGroup()
        {
            Benchmarks = new List<IBenchmark>{ new FrameworkCloneBenchmark(), new DelegateCloneBenchmark(), new ManualCloneBenchmark() };
            Iterations = 200000;
            Name = "Clone Instance";
        }

        public IEnumerable<IBenchmark> Benchmarks { get; private set; }
        public decimal Iterations { get; private set; }
        public string Name { get; private set; }
    }
}
