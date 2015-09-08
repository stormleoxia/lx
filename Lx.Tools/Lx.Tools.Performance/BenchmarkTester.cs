using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Lx.Tools.Performance
{
    public class BenchmarkTester
    {
        private readonly IBenchmark _benchmark;
        private readonly decimal _iterations;
        private long _miliseconds;
        private long _ticks;

        public BenchmarkTester(IBenchmark benchmark, decimal iterations)
        {
            _benchmark = benchmark;
            _iterations = iterations;
        }

        public void Bench()
        {
            GC.Collect();
            _benchmark.Init();
            _benchmark.Call(); // Warm up
            Thread.Sleep(500);
            var stopwatch = new Stopwatch(); 
            stopwatch.Start();
            for (int i = 0; i < _iterations; ++i)
            {
                _benchmark.Call();
            }
            stopwatch.Stop();
            _benchmark.Cleanup();
            _miliseconds = stopwatch.ElapsedMilliseconds;
            _ticks = stopwatch.ElapsedTicks;
        }

        public string GetResults()
        {
            var builder = new StringBuilder();
            builder.AppendLine("   Benchmark " + _benchmark.Name);
            builder.AppendLine("      Elapsed : " + TimeSpan.FromMilliseconds(_miliseconds) + ".");
            builder.AppendLine("      Elapsed : " + _miliseconds + " ms.");
            builder.AppendLine("      Elapsed : " + _ticks + " ticks.");
            decimal rate = Math.Round(_iterations/_miliseconds, 0);
            string suffix = "/ms";
            if (rate == 0)
            {
                rate = Math.Round(_iterations*1000/_miliseconds, 0);
                suffix = "/s";
            }
            builder.AppendLine("      Rate : " + rate + " Calls" +  suffix);
            return builder.ToString();
        }
    }
}