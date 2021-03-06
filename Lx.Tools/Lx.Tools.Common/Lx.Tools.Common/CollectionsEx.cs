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
using System.Runtime.InteropServices;

namespace Lx.Tools.Common
{
    public static class CollectionsEx
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items)
        {
            var hash = new HashSet<T>();
            foreach (var item in items)
            {
                hash.Add(item);
            }
            return hash;
        }

        public static Dictionary<TKey, TValue> ToDictionaryIgnoreDuplicates<TKey, TValue, TSource>(
            this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            foreach (var item in enumerable)
            {
                var key = keySelector(item);
                if (!dictionary.ContainsKey(key))
                {
                    dictionary[key] = valueSelector(item);
                }
            }
            return dictionary;
        }

        public static Stack<T> ToStack<T>(this IEnumerable<T> enumerable)
        {
            return new Stack<T>(enumerable);
        }
    }
}