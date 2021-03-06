﻿#region Copyright (c) 2015 Leoxia Ltd

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
using System.IO;
using System.Linq;

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
        ///     Splits the input with delimiters while keeping them in the output list
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="delimiters">The delimiters.</param>
        /// <returns></returns>
        public static string[] SplitKeepDelimiters(string input, string[] delimiters)
        {
            var result = new List<string>();
            var length = input.Length;
            var lastMatchEnd = 0;
            var delLen = delimiters.Length;
            for (var i = 0; i < length; ++i)
            {
                for (var j = 0; j < delLen; ++j)
                {
                    var str = delimiters[j];
                    var sepLen = str.Length;
                    if (((input[i] == str[0]) && (sepLen <= (length - i))) &&
                        ((sepLen == 1) || (String.CompareOrdinal(input, i, str, 0, sepLen) == 0)))
                    {
                        result.Add(input.Substring(lastMatchEnd, i - lastMatchEnd));
                        result.Add(str);
                        i += sepLen - 1;
                        lastMatchEnd = i + 1;
                        break;
                    }
                }
            }
            if (lastMatchEnd != length)
                result.Add(input.Substring(lastMatchEnd));
            return result.ToArray();
        }

        public static string IntersectFromEnd(string reference, string compared)
        {
            var refIndex = reference.Length - 1;
            var compIndex = compared.Length - 1;
            while (refIndex >= 0 && compIndex >= 0)
            {
                if (Char.ToLowerInvariant(reference[refIndex]) != Char.ToLowerInvariant(compared[compIndex]))
                {
                    var startIndex = refIndex + 1;
                    if (startIndex >= reference.Length)
                    {
                        return string.Empty;
                    }
                    return reference.Substring(startIndex, reference.Length - startIndex);
                }
                refIndex--;
                compIndex--;
            }
            return reference.Length > compared.Length ? compared : reference;
        }
    }
}