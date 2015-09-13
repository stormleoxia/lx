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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.StringSplit
{
    /// <summary>
    ///     Extracted from
    ///     http://stackoverflow.com/questions/2484919/how-do-i-split-a-string-by-strings-and-include-the-delimiters-using-net
    /// </summary>
    internal class AnotherOptimizedSplitBenchmark : BaseSplitBenchmark, IBenchmark
    {
        public string Name
        {
            get { return "Another Optimized String Split"; }
        }

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

        public void Cleanup()
        {
        }

        public void SplitFromAMethod(string searchStr, string[] separators)
        {
            var result = new List<string>();
            var length = searchStr.Length;
            var lastMatchEnd = 0;
            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < separators.Length; j++)
                {
                    var str = separators[j];
                    var sepLen = str.Length;
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
    }
}