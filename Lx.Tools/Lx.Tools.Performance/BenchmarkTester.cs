#region Copyright (c) 2015 Leoxia Ltd

//  Copyright © 2015 Leoxia Ltd
//  
//  This file is part of Lx.
// 
//  Lx is released under GNU General Public License unless stated otherwise.
//  You may not use this file except in compliance with the License.
//  You can redistribute it and/or modify it under the terms of the GNU General Public License 
//  as published by the Free Software Foundation, either version 3 of the License, 
//  or any later version.
//  
//  In case GNU General Public License is not applicable for your use of Lx, 
//  you can subscribe to commercial license on 
//  http://www.leoxia.com 
//  by contacting us through the form page or send us a mail
//  mailto:contact@leoxia.com
//   
//  Unless required by applicable law or agreed to in writing, 
//  Lx is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES 
//  OR CONDITIONS OF ANY KIND, either express or implied. 
//  See the GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License along with Lx.
//  It is present in the Lx root folder SolutionItems/GPL.txt
//  If not, see http://www.gnu.org/licenses/.

#endregion

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
            for (var i = 0; i < _iterations; ++i)
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
            var rate = Math.Round(_iterations/_miliseconds, 0);
            var suffix = "/ms";
            if (rate == 0)
            {
                rate = Math.Round(_iterations*1000/_miliseconds, 0);
                suffix = "/s";
            }
            builder.AppendLine("      Rate : " + rate + " Calls" + suffix);
            return builder.ToString();
        }
    }
}