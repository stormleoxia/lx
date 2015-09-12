using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.StringSplit
{
    class RegexMatchBenchmark : BaseSplitBenchmark, IBenchmark
    {
        public string Name { get { return "Regex Split with Match "; } }
        public void Init()
        {
            var list = new List<KeyValuePair<Regex, string>>();
            foreach (var input in Inputs)
            {
                var delimiters = input.Key;
                var key = "(" +
                    String.Join("|", delimiters.Select(d => Regex.Escape(d)).ToArray())
                    + ")";
                var pattern =  new Regex(key, RegexOptions.Compiled);
                list.Add(new KeyValuePair<Regex, string>(pattern, input.Value));

            }
            RegexInputs = list.ToArray();
        }

        public KeyValuePair<Regex, string>[] RegexInputs { get; private set; }

        public void Call()
        {
            foreach (var input in RegexInputs)
            {
                List<string> result = new List<string>();
                var matches = input.Key.Matches(input.Value);
                int index = 0;
                foreach (Match match in matches)
                {
                    var length = match.Index - index;
                    if (length > 0)
                    {
                        result.Add(input.Value.Substring(index, length));                        
                    }
                    result.Add(match.Value);
                    index = match.Index + match.Length;
                }
                if (index < input.Value.Length - 1)
                {
                    result.Add(input.Value.Substring(index, input.Value.Length - index));
                }
            }
        }

        public void Cleanup()
        {
        }
    }
}
