using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lx.Tools.Common;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.StringSplit
{
    class LxStringSplitBenchmark : BaseSplitBenchmark, IBenchmark
    {
        public string Name { get { return "String Split used by Lx"; } }

        public void Init()
        {

        }

        public void Call()
        {
            foreach (var input in Inputs)
            {
                StringEx.SplitKeepDelimiters(input.Value, input.Key.ToArray());
            }
        }

        public void Cleanup()
        {
        }
    }
}
