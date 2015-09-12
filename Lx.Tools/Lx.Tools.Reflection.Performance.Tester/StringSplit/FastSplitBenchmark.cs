using Lx.Tools.Common;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.StringSplit
{
    class FastSplitBenchmark : BaseSplitBenchmark, IBenchmark
    {
        public string Name { get { return "Fast Split"; } }
        public void Init()
        {
        }

        public void Call()
        {
            foreach (var input in Inputs)
            {
                var result = StringEx.SplitKeepDelimiters(input.Value, input.Key.ToArray());
            }
        }

        public void Cleanup()
        {
        }
    }
}
