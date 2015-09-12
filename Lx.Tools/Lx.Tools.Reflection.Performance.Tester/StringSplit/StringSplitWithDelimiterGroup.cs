using System.Collections.Generic;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.StringSplit
{
    class StringSplitWithDelimiterGroup : IBenchmarkGroup
    {
        public StringSplitWithDelimiterGroup()
        {
            Name = "String Split with several string delimiters";
            Iterations = 10000;
            Benchmarks = new List<IBenchmark>
            {
                new CompiledRegexSplitBenchmark(), 
                new OptimizedBuggySplitBenchmark(), 
                new OptimizedSplitBenchmark(),
                new RegexSplitBenchmark(),
                new RegexMatchBenchmark(),
                new FastSplitBenchmark()
            };
        }

        public IEnumerable<IBenchmark> Benchmarks { get; private set; }
        public decimal Iterations { get; private set; }
        public string Name { get; private set; }
    }
}
