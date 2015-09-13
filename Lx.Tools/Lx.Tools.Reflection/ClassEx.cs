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
using System.Reflection;

namespace Lx.Tools.Reflection
{
    /// <summary>
    ///     Extension for any class
    /// </summary>
    public static class ClassEx
    {
        private static readonly IDictionary<Type, object> dictionary = new Dictionary<Type, object>();

        /// <summary>
        ///     Clone instance Property-wise.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static T PropertyWiseClone<T>(this T instance) where T : new()
        {
            var type = typeof (T);
            var clone = new T();
            IPropertyCopier<T> instanceCopier;
            object boxed;
            if (!dictionary.TryGetValue(type, out boxed))
            {
                instanceCopier = new InstanceCopier<T>();
                dictionary[type] = instanceCopier;
            }
            else
            {
                instanceCopier = (IPropertyCopier<T>) boxed;
            }
            instanceCopier.Copy(instance, clone);
            return clone;
        }
    }

    public sealed class InstanceCopier<T> : IPropertyCopier<T>
    {
        private readonly IPropertyCopier<T>[] _copiers;

        public InstanceCopier()
        {
            var properties = typeof (T).GetRuntimeProperties();
            var localCopiers = new List<IPropertyCopier<T>>();
            foreach (var property in properties)
            {
                localCopiers.Add(Properties.CreatePropertyCopier<T>(property));
            }
            _copiers = localCopiers.ToArray();
        }

        public void Copy(T source, T destination)
        {
            for (var index = 0; index < _copiers.Length; index++)
            {
                _copiers[index].Copy(source, destination);
            }
        }
    }

    public static class Properties
    {
        private static readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public static PropertyCopier<T> CreatePropertyCopier<T>(PropertyInfo property)
        {
            var key = typeof (T).FullName + "." + property.Name;
            object boxed;
            if (!_dictionary.TryGetValue(key, out boxed))
            {
                var copier = new PropertyCopier<T>(property.Name);
                _dictionary[key] = copier;
                return copier;
            }
            return (PropertyCopier<T>) boxed;
        }
    }
}