using System.Collections.Generic;
using System.Text;

namespace Lx.Tools.Performance
{
    public class BenchmarkGroupTester
    {
        private readonly IBenchmarkGroup _group;
        private readonly IList<BenchmarkTester> _testers = new List<BenchmarkTester>();

        public BenchmarkGroupTester(IBenchmarkGroup group)
        {
            _group = group;
        }

        public void Bench()
        {
            foreach (IBenchmark benchmark in _group.Benchmarks)
            {
                var tester = new BenchmarkTester(benchmark, _group.Iterations);
                _testers.Add(tester);
                tester.Bench();
                tester.Bench();
            }
        }

        public string GetResults()
        {
            var builder = new StringBuilder();
            builder.AppendLine(" == Results for " + _group.Name + " == ");
            foreach (BenchmarkTester tester in _testers)
            {
                builder.AppendLine(tester.GetResults());
            }
            return builder.ToString();
        }
    }
}