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
using System.CodeDom;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security;

namespace Lx.Tools.Common
{
    public static class StringEx
    {
        public static string ToPlatformPath(this string path)
        {
            return path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }

        public static string RemoveDotPath(this string path)
        {
            var components = path.Split('/', '\\');
            return string.Join(Path.DirectorySeparatorChar.ToString(),
                components.Select(x => x.Trim()).Where(x => x != "."));
        }

        public static bool IsDirectorySeparator(this char character)
        {
            return character == '/' || character == '\\';
        }

        /// <summary>
        /// Splits the input with delimiters while keeping them in the output list
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [SecurityCritical]
        public unsafe static string[] SplitKeepDelimiters(string input, string[] delimiters)
        {
            unchecked
            {
                List<string> result = new List<string>(10);
                int length = input.Length;
                fixed (char* inputFixed = input)
                {
                    char* inputPtr = inputFixed;
                    char* endPtr = inputPtr + length;
                    char* lastMatchPtr = inputFixed;
                    var delLen = delimiters.Length;
                    for (; inputPtr < endPtr; inputPtr++)
                    {
                        for (int j = 0; j < delLen; j++)
                        {
                            string str = delimiters[j];
                            fixed (char* sepFixed = str)
                            {
                                char* sep = sepFixed;
                                int sepLen = str.Length;
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
        /// Create a substrings start at the specified start and ending just before the end pointer.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [SecurityCritical]
        private unsafe static string Substring(char* start, char* end)
        {
            unchecked
            {
                char* result = stackalloc char[(int) (end - start + 1)];
                var moving = result;
                while (start < end)
                {
                    *moving = *start;
                    ++start;
                    ++moving;
                }
                *moving = '\0';
                var res = new string(result);
                return res;
            }
        }

        [SuppressUnmanagedCodeSecurity]
        [SecurityCritical]
        internal unsafe static bool CompareOrdinal(char* first, char* second, int length)
        {
            unchecked
            {
                int i = 0;
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