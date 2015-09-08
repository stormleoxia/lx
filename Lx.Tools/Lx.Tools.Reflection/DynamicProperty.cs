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

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Lx.Tools.Reflection
{
    /// <summary>
    ///     Provides fast access, strongly typed, expensive to a property
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <typeparam name="TPropertyType">The type of the property type.</typeparam>
    public static class DynamicProperty<TContainer, TPropertyType>
    {
        /// <summary>
        ///     Creates the get property delegate of given name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static Func<TContainer, TPropertyType> CreateGetPropertyDelegate(string propertyName)
        {
            var containerType = typeof (TContainer);
            var param = Expression.Parameter(containerType, "container");
            var func = Expression.Lambda(
                Expression.PropertyOrField(param, propertyName),
                param
                );
            return (Func<TContainer, TPropertyType>) func.Compile();
        }

        /// <summary>
        ///     Creates the set property delegate of given name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static Action<TContainer, TPropertyType> CreateSetPropertyDelegate(string propertyName)
        {
            var containerType = typeof (TContainer);
            var setMethod = containerType.GetRuntimeProperty(propertyName).SetMethod;
            var paramContainer = Expression.Parameter(containerType, "container");
            var paramValue = Expression.Parameter(typeof (TPropertyType), "value");
            Expression expr = paramContainer;
            var call = Expression.Call(paramContainer, setMethod, paramValue);
            var action = Expression.Lambda(call, paramContainer, paramValue);
            return (Action<TContainer, TPropertyType>) action.Compile();
        }
    }
}