#region Copyright (c) 2015 Leoxia Ltd

// #region Copyright (c) 2015 Leoxia Ltd
// 
// // Copyright © 2015 Leoxia Ltd.
// // 
// // This file is part of Lx.
// //
// // Lx is released under GNU General Public License unless stated otherwise.
// // You may not use this file except in compliance with the License.
// // You can redistribute it and/or modify it under the terms of the GNU General Public License 
// // as published by the Free Software Foundation, either version 3 of the License, 
// // or any later version.
// // 
// // In case GNU General Public License is not applicable for your use of Lx, 
// // you can subscribe to commercial license on 
// // http://www.leoxia.com 
// // by contacting us through the form page or send us a mail
// // mailto:contact@leoxia.com
// //  
// // Unless required by applicable law or agreed to in writing, 
// // Lx is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES 
// // OR CONDITIONS OF ANY KIND, either express or implied. 
// // See the GNU General Public License for more details.
// //
// // You should have received a copy of the GNU General Public License along with Lx.
// // It is present in the Lx root folder SolutionItems/GPL.txt
// // If not, see http://www.gnu.org/licenses/.
// //
// 
// #endregion

#endregion

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lx.Tools.Reflection
{
    /// <summary>
    ///     Provide the same functionality as MemberwiseClone() but without reflection on execution
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    public class MemberwiseCloner<TContainer>
    {
        private static readonly int length;
        private static readonly IPropertyCopier<TContainer>[] copiers;

        /// <summary>
        ///     Initializes the <see cref="MemberwiseCloner&lt;TContainer&gt;" /> class.
        /// </summary>
        static MemberwiseCloner()
        {
            var type = typeof (TContainer);
            var properties = type.GetRuntimeProperties().ToArray();
            length = properties.Length;
            var list = new List<IPropertyCopier<TContainer>>(length);
            for (var i = 0; i < length; ++i)
            {
                var property = properties[i];
                if (property.CanRead && property.CanWrite)
                {
                    list.Add(new PropertyCopier<TContainer>(property.Name));
                }
            }
            length = list.Count;
            copiers = list.ToArray();
        }

        /// <summary>
        ///     Do a shallow copy.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void ShallowCopy(TContainer source, TContainer destination)
        {
            for (var i = 0; i < length; ++i)
            {
                copiers[i].Copy(source, destination);
            }
        }
    }

    /// <summary>
    ///     Provide the same functionality as MemberwiseClone() but without reflection on execution
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the container.</typeparam>
    public class MemberwiseCloner<TSource, TDestination>
    {
        private static readonly int length;
        private static readonly IPropertyCopier<TSource, TDestination>[] copiers;

        /// <summary>
        ///     Initializes the <see cref="MemberwiseCloner&lt;TSource, TDestination&gt;" /> class.
        /// </summary>
        static MemberwiseCloner()
        {
            var type = typeof (TSource);
            var properties = type.GetRuntimeProperties().ToArray();
            length = properties.Length;
            copiers = new IPropertyCopier<TSource, TDestination>[length];
            for (var i = 0; i < length; ++i)
            {
                var property = properties[i];
                copiers[i] = new PropertyCopier<TSource, TDestination>(property.Name);
            }
        }

        /// <summary>
        ///     Do a shallow copy.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void ShallowCopy(TSource source, TDestination destination)
        {
            for (var i = 0; i < length; ++i)
            {
                copiers[i].Copy(source, destination);
            }
        }
    }
}