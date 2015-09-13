using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.StringSplit
{
    /// <summary>
    /// Extracted from http://stackoverflow.com/questions/2484919/how-do-i-split-a-string-by-strings-and-include-the-delimiters-using-net
    /// </summary>
    class AnotherOptimizedSplitBenchmark : BaseSplitBenchmark, IBenchmark
    {
        public string Name { get { return "Another Optimized String Split"; } }
        public void Init()
        {

        }

        public void Call()
        {
            foreach (var input in Inputs)
            {
                SplitFromAMethod(input.Value, input.Key.ToArray());
            }
        }

        public void SplitFromAMethod(string searchStr, string[] separators)
        {
            List<string> result = new List<string>();
            int length = searchStr.Length;
            int lastMatchEnd = 0;
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < separators.Length; j++)
                {
                    string str = separators[j];
                    int sepLen = str.Length;
                    if (((searchStr[i] == str[0]) && (sepLen <= (length - i))) &&
                        ((sepLen == 1) || (String.CompareOrdinal(searchStr, i, str, 0, sepLen) == 0)))
                    {
                        result.Add(searchStr.Substring(lastMatchEnd, i - lastMatchEnd));
                        result.Add(separators[j]);
                        i += sepLen - 1;
                        lastMatchEnd = i + 1;
                        break;
                    }
                }
            }
            if (lastMatchEnd != length)
                result.Add(searchStr.Substring(lastMatchEnd));
        }

        public void Cleanup()
        {
        }
    }
}
