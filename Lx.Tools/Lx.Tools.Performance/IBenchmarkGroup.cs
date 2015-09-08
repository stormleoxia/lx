using System.Collections.Generic;

namespace Lx.Tools.Performance
{
    public interface IBenchmarkGroup
    {
        IEnumerable<IBenchmark> Benchmarks { get; }
        decimal Iterations { get; }
        string Name { get; }
    }
}