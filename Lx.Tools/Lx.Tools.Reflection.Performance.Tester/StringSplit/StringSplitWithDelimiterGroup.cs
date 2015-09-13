using System.Collections.Generic;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.StringSplit
{
    class StringSplitWithDelimiterGroup : IBenchmarkGroup
    {
        public StringSplitWithDelimiterGroup()
        {
            Name = "String Split with several string delimiters";
            Iterations = 20000;
            Benchmarks = new List<IBenchmark>
            {
                new CompiledRegexSplitBenchmark(), 
                new AnotherOptimizedSplitBenchmark(), 
                new OptimizedSplitBenchmark(),
                new RegexSplitBenchmark(),
                new RegexMatchBenchmark(),
                new UnsafeSplitBenchmark(),
                new LxStringSplitBenchmark()
            };
        }

        public IEnumerable<IBenchmark> Benchmarks { get; private set; }
        public decimal Iterations { get; private set; }
        public string Name { get; private set; }
    }
}
