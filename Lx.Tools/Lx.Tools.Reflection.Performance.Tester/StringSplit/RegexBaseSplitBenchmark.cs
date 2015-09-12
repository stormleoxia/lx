using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.StringSplit
{
    class CompiledRegexSplitBenchmark : BaseSplitBenchmark, IBenchmark
    {
        public string Name { get { return "Regex Split";  } }

        public KeyValuePair<string, string>[] RegexInputs { get; private set; } 

        public void Init()
        {
            var list = new List<KeyValuePair<string, string>>();
            foreach (var input in Inputs)
            {
                var delimiters = input.Key;
                var key = "(" + 
                          String.Join("|", delimiters.Select(d => Regex.Escape(d)).ToArray()) 
                          + ")";
                list.Add(new KeyValuePair<string, string>(key, input.Value));
            }
            RegexInputs = list.ToArray();
        }

        public void Call()
        {
            foreach (var input in RegexInputs)
            {
                var value = Regex.Split(input.Value, input.Key);
            }
        }

        public void Cleanup()
        {
        }
    }
}