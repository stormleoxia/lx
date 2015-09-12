using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.StringSplit
{
    class RegexSplitBenchmark : BaseSplitBenchmark, IBenchmark
    {
        public string Name { get { return "Compiled Regex Split";  } }

        public KeyValuePair<Regex, string>[] RegexInputs { get; private set; } 

        public void Init()
        {
            var list = new List<KeyValuePair<Regex, string>>();
            foreach (var input in Inputs)
            {
                var delimiters = input.Key;
                var key = new Regex("(" + 
                    String.Join("|", delimiters.Select(d => Regex.Escape(d)).ToArray()) 
                    + ")", RegexOptions.Compiled);
                
                list.Add(new KeyValuePair<Regex, string>(key, input.Value));
            }
            RegexInputs = list.ToArray();
        }

        public void Call()
        {
            foreach (var input in RegexInputs)
            {
                var value = input.Key.Split(input.Value);
            }
        }

        public void Cleanup()
        {
        }
    }
}
