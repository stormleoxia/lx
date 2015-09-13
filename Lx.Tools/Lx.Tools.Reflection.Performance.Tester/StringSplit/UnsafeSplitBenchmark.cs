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
using System.Security;
using Lx.Tools.Common;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.StringSplit
{
    internal class UnsafeSplitBenchmark : BaseSplitBenchmark, IBenchmark
    {
        public string Name
        {
            get { return "Unsafe Split"; }
        }

        public void Init()
        {
        }

        public void Call()
        {
            foreach (var input in Inputs)
            {
                var result = SplitKeepDelimiters(input.Value, input.Key.ToArray());
            }
        }

        public void Cleanup()
        {
        }

        private unsafe string[] SplitKeepDelimiters(string input, string[] delimiters)
        {
            unchecked
            {
                var result = new List<string>(10);
                var length = input.Length;
                fixed (char* inputFixed = input)
                {
                    var inputPtr = inputFixed;
                    var endPtr = inputPtr + length;
                    var lastMatchPtr = inputFixed;
                    var delLen = delimiters.Length;
                    for (; inputPtr < endPtr; inputPtr++)
                    {
                        for (var j = 0; j < delLen; j++)
                        {
                            var str = delimiters[j];
                            fixed (char* sepFixed = str)
                            {
                                var sep = sepFixed;
                                var sepLen = str.Length;
                                if (((*inputPtr == *sep) && (inputPtr + sepLen <= endPtr)) &&
                                    ((sepLen == 1) || (CompareOrdinal(inputPtr, sep, sepLen))))
                                {
                                    result.Add(Substring(lastMatchPtr, inputPtr));
                                    result.Add(str);
                                    inputPtr += sepLen;
                                    lastMatchPtr = inputPtr;
                                    break;
                                }
                            }
                        }
                    }

                    if (lastMatchPtr < endPtr)
                        result.Add(Substring(lastMatchPtr, endPtr));
                    return result.ToArray();
                }
            }
        }

        /// <summary>
        ///     Create a substrings start at the specified start and ending just before the end pointer.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [SecurityCritical]
        private static unsafe string Substring(char* start, char* end)
        {
            unchecked
            {
                var res = new string('c', (int) (end - start));
                fixed (char* result = res)
                {
                    var moving = result;
                    while (start < end)
                    {
                        *moving = *start;
                        ++start;
                        ++moving;
                    }
                    *moving = '\0';
                }
                return res;
            }
        }

        [SuppressUnmanagedCodeSecurity]
        [SecurityCritical]
        internal static unsafe bool CompareOrdinal(char* first, char* second, int length)
        {
            unchecked
            {
                var i = 0;
                while (*first == *second)
                {
                    ++first;
                    ++second;
                    ++i;
                    if (i == length)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}