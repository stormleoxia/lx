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

using System.Collections.Generic;
using System.Linq;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.StringSplit
{
    /// <summary>
    ///     Extracted from
    ///     http://stackoverflow.com/questions/2484919/how-do-i-split-a-string-by-strings-and-include-the-delimiters-using-net
    /// </summary>
    internal class OptimizedSplitBenchmark : BaseSplitBenchmark, IBenchmark
    {
        public string Name
        {
            get { return "Optimized String Split"; }
        }

        public void Init()
        {
        }

        public void Call()
        {
            foreach (var keyValue in Inputs)
            {
                var input = keyValue.Value;
                var delimiters = keyValue.Key.ToArray();

                var nextPosition = delimiters.Select(d => input.IndexOf(d)).ToArray();
                var result = new List<string>();
                var pos = 0;
                while (true)
                {
                    var firstPos = int.MaxValue;
                    string delimiter = null;
                    for (var i = 0; i < nextPosition.Length; i++)
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
                        for (var i = 0; i < nextPosition.Length; i++)
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