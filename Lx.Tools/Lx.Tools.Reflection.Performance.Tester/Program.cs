using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var benchmarkGroups = new List<IBenchmarkGroup>();
            benchmarkGroups.Add(new MethodCallBenchmarkGroup());
            foreach (var group in benchmarkGroups)
            {
                var tester = new BenchmarkGroupTester(group);
                tester.Bench();
                Console.WriteLine(tester.GetResults());
            }
            Console.ReadLine();
        }
    }
}
