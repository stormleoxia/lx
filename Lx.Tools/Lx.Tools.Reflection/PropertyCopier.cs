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
using System.Reflection;

namespace Lx.Tools.Reflection
{
    /// <summary>
    /// ast Access to Property Copy between two objects of the same type.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    public class PropertyCopier<TContainer> : IPropertyCopier<TContainer>
    {
        private readonly IPropertyCopier<TContainer> _copier;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PropertyCopier&lt;TContainer&gt;" /> class.
        ///     Instantiate the Proxy Type Strongly Typed depending on the type of the given property
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyCopier(string propertyName)
        {
            var containerType = typeof (TContainer);
            var propertyType = containerType.GetRuntimeProperty(propertyName).PropertyType;
            var dynamicProperty = typeof (PropertyCopierProxy<,>).MakeGenericType(containerType, propertyType);
            _copier = (IPropertyCopier<TContainer>) Activator.CreateInstance(dynamicProperty, propertyName);
        }

        /// <summary>
        ///     Copies the property value from the specified source to specified destination.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public void Copy(TContainer source, TContainer destination)
        {
            _copier.Copy(source, destination);
        }
    }

    /// <summary>
    ///     Fast Access to Property Copy between two objects of the same type.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    public class PropertyCopier<TSource, TDestination> : IPropertyCopier<TSource, TDestination>
    {
        private readonly IPropertyCopier<TSource, TDestination> _copier;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PropertyCopier&lt;TContainer&gt;" /> class.
        ///     Instantiate the Proxy Type Strongly Typed depending on the type of the given property
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyCopier(string propertyName)
        {
            var sourceType = typeof (TSource);
            var propertyType = sourceType.GetRuntimeProperty(propertyName).PropertyType;
            var destinationType = typeof (TDestination);
            if (propertyType != destinationType.GetRuntimeProperty(propertyName).PropertyType)
            {
                throw new InvalidOperationException(sourceType + " " + destinationType + " property " + propertyName +
                                                    " type mismatch");
            }
            var dynamicProperty = typeof (PropertyCopierProxy<,,>).MakeGenericType(sourceType, destinationType,
                propertyType);
            _copier = (IPropertyCopier<TSource, TDestination>) Activator.CreateInstance(dynamicProperty, propertyName);
        }

        /// <summary>
        ///     Copies the property value from the specified source to specified destination.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public void Copy(TSource source, TDestination destination)
        {
            _copier.Copy(source, destination);
        }
    }
}