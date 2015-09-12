using System.Collections.Generic;
using System.Linq;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.StringSplit
{
    /// <summary>
    /// Extracted from
    /// http://stackoverflow.com/questions/2484919/how-do-i-split-a-string-by-strings-and-include-the-delimiters-using-net
    /// </summary>
    class OptimizedSplitBenchmark : BaseSplitBenchmark, IBenchmark
    {
        public string Name { get { return "Optimized String Split"; } }
        public void Init()
        {
            
        }

        public void Call()
        {
            foreach (var keyValue in Inputs)
            {
                string input = keyValue.Value;
                string[] delimiters = keyValue.Key.ToArray();

                int[] nextPosition = delimiters.Select(d => input.IndexOf(d)).ToArray();
                List<string> result = new List<string>();
                int pos = 0;
                while (true)
                {
                    int firstPos = int.MaxValue;
                    string delimiter = null;
                    for (int i = 0; i < nextPosition.Length; i++)
                    {
                        if (nextPosition[i] != -1 && nextPosition[i] < firstPos)
                        {
                            firstPos = nextPosition[i];
                            delimiter = delimiters[i];
                        }
                    }
                    if (firstPos != int.MaxValue)
                    {
                        result.Add(input.Substring(pos, firstPos - pos));
                        result.Add(delimiter);
                        pos = firstPos + delimiter.Length;
                        for (int i = 0; i < nextPosition.Length; i++)
                        {
                            if (nextPosition[i] != -1 && nextPosition[i] < pos)
                            {
                                nextPosition[i] = input.IndexOf(delimiters[i], pos);
                            }
                        }
                    }
                    else
                    {
                        result.Add(input.Substring(pos));
                        break;
                    }
                }
            }
        }

        public void Cleanup()
        {
        }
    }
}
