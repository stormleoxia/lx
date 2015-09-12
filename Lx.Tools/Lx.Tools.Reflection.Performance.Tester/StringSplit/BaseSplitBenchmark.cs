using System.Collections.Generic;

namespace Lx.Tools.Reflection.Performance.Tester.StringSplit
{
    internal class BaseSplitBenchmark
    {
        public BaseSplitBenchmark()
        {
            Inputs = new[]
            {
                new KeyValuePair<List<string>, string>(new List<string>{@"\", "/"}, @"C:\Business\Value/File.txt"),
                new KeyValuePair<List<string>, string>(new List<string>{@":", ";"}, @"Business:Value::::;::::::a:b"),
                new KeyValuePair<List<string>, string>(new List<string>{".."}, @".. .. .. .. .. .. ....."),
                new KeyValuePair<List<string>, string>(new List<string>{"  "}, @"..  ..  .. ..  .. . .     .....  ")
            };
        }

        protected KeyValuePair<List<string>, string>[] Inputs { get; private set; }
    }
}
