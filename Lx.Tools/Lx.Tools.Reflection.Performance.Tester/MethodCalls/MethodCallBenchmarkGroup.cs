using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Xml.Serialization;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester
{
    internal class MethodCallBenchmarkGroup : IBenchmarkGroup
    {
        public IEnumerable<IBenchmark> Benchmarks
        {
            get
            {
                return new IBenchmark[]
                {
                    new MethodInvokeBenchmark(),
                    new StandardMethodBenchmark(),
                    new ExpressionTreeCallBenchmark(),
                    new InvokeMemberBenchmark(),
                    new DelegateCallBenchmark()
                };
            }
        }
        public decimal Iterations { get { return 2000000; }}
        public string Name { get { return "Method Call"; }
        }
    }
}