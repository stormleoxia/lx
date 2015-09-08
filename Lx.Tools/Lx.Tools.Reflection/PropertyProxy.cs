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
using System.Reflection;

namespace Lx.Tools.Reflection
{
    /// <summary>
    ///     Purpose of proxy is to effectively copy property with a strongly typed signature
    ///     on the generic type parameters and serves as a base class for the
    ///     implementation of interfaces depending only on the container type.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <typeparam name="TPropertyType">The type of the property type.</typeparam>
    internal class PropertyProxy<TContainer, TPropertyType>
    {
        private readonly Func<TContainer, TPropertyType> getProperty;
        private readonly Action<TContainer, TPropertyType> setProperty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PropertyCopierProxy&lt;TContainer, TPropertyType&gt;" /> class.
        ///     Create the dynamic accessors to get and set property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyProxy(string propertyName)
        {
            getProperty = DynamicProperty<TContainer, TPropertyType>.CreateGetPropertyDelegate(propertyName);
            setProperty = DynamicProperty<TContainer, TPropertyType>.CreateSetPropertyDelegate(propertyName);
        }

        /// <summary>
        ///     Gets the get property.
        /// </summary>
        /// <value>The get property.</value>
        protected Func<TContainer, TPropertyType> GetProperty
        {
            get { return getProperty; }
        }

        /// <summary>
        ///     Gets the set property.
        /// </summary>
        /// <value>The set property.</value>
        protected Action<TContainer, TPropertyType> SetProperty
        {
            get { return setProperty; }
        }
    }

    /// <summary>
    ///     Purpose of proxy is to effectively copy property with a strongly typed signature
    ///     on the generic type parameters and serves as a base class for the
    ///     implementation of interfaces depending only on the container type.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <typeparam name="TPropertyType">The type of the property type.</typeparam>
    internal class PropertyProxy<TSource, TDestination, TPropertyType>
    {
        private readonly Func<TSource, TPropertyType> getProperty;
        private readonly Action<TDestination, TPropertyType> setProperty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PropertyCopierProxy&lt;TContainer, TPropertyType&gt;" /> class.
        ///     Create the dynamic accessors to get and set property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyProxy(string propertyName)
        {
            var sourceType = typeof (TSource);
            if (!sourceType.GetRuntimeProperty(propertyName).CanRead)
            {
                throw new InvalidOperationException(sourceType + " " + propertyName + " has no getter");
            }
            getProperty = DynamicProperty<TSource, TPropertyType>.CreateGetPropertyDelegate(propertyName);
            var destinationType = typeof (TDestination);
            if (!destinationType.GetRuntimeProperty(propertyName).CanWrite)
            {
                throw new InvalidOperationException(destinationType + " " + propertyName + " has no setter");
            }
            setProperty = DynamicProperty<TDestination, TPropertyType>.CreateSetPropertyDelegate(propertyName);
        }

        /// <summary>
        ///     Gets the get property.
        /// </summary>
        /// <value>The get property.</value>
        protected Func<TSource, TPropertyType> GetProperty
        {
            get { return getProperty; }
        }

        /// <summary>
        ///     Gets the set property.
        /// </summary>
        /// <value>The set property.</value>
        protected Action<TDestination, TPropertyType> SetProperty
        {
            get { return setProperty; }
        }
    }
}