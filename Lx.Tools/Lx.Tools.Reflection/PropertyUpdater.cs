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
    ///     Interface for property copy to a given container based on the difference between two instances of the same type.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    public interface IPropertyUpdater<TContainer>
    {
        /// <summary>
        ///     Updates a property value of destination with the property value of source
        ///     if property value of source is different from property value of reference.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        void Update(TContainer reference, TContainer source, TContainer destination);
    }

    /// <summary>
    ///     Strongly typed Fast Access to property update based on the difference between two instances of the same type
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <typeparam name="TPropertyType">The type of the property type.</typeparam>
    internal class PropertyUpdaterProxy<TContainer, TPropertyType> : PropertyProxy<TContainer, TPropertyType>,
        IPropertyUpdater<TContainer>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PropertyUpdaterProxy&lt;TContainer, TPropertyType&gt;" /> class.
        ///     Create the dynamic accessors to get and set property, by reflection and compilation.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyUpdaterProxy(string propertyName) : base(propertyName)
        {
        }

        /// <summary>
        ///     Updates a property value of destination with the property value of source
        ///     if property value of source is different from property value of reference.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public void Update(TContainer reference, TContainer source, TContainer destination)
        {
            var sourceValue = PropertyGetter(source);
            if (!PropertyGetter(reference).Equals(sourceValue))
            {
                PropertySetter(destination, sourceValue);
            }
        }
    }

    /// <summary>
    ///     Fast access to property update based on the difference between two instances of the same type
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    public class PropertyUpdater<TContainer> : IPropertyUpdater<TContainer>
    {
        private readonly IPropertyUpdater<TContainer> updater;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PropertyUpdater&lt;TContainer&gt;" /> class.
        ///     Instantiate by reflection the strongly typed proxy which will do the update
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyUpdater(string propertyName)
        {
            var containerType = typeof (TContainer);
            var propertyType = containerType.GetRuntimeProperty(propertyName).PropertyType;
            var dynamicProperty = typeof (PropertyUpdaterProxy<,>).MakeGenericType(containerType, propertyType);
            updater = (IPropertyUpdater<TContainer>) Activator.CreateInstance(dynamicProperty, propertyName);
        }

        /// <summary>
        ///     Updates a property value of destination with the property value of source
        ///     if property value of source is different from property value of reference.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public void Update(TContainer reference, TContainer source, TContainer destination)
        {
            updater.Update(reference, source, destination);
        }
    }
}