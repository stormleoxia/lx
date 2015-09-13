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

namespace Lx.Tools.Reflection
{
    /// <summary>
    ///     Purpose of proxy is to effectively copy property with a strongly typed signature
    ///     on the generic type parameters and implement interface depending only on the container type.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <typeparam name="TPropertyType">The type of the property type.</typeparam>
    internal class PropertyCopierProxy<TSource, TDestination, TPropertyType> :
        PropertyProxy<TSource, TDestination, TPropertyType>, IPropertyCopier<TSource, TDestination>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PropertyCopierProxy&lt;TContainer, TPropertyType&gt;" /> class.
        ///     Create the dynamic accessors to get and set property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyCopierProxy(string propertyName) :
            base(propertyName)
        {
        }

        /// <summary>
        ///     Copies the property value from the specified source to specified destination.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public void Copy(TSource source, TDestination destination)
        {
            PropertySetter(destination, PropertyGetter(source));
        }
    }

    /// <summary>
    ///     Purpose of proxy is to effectively copy property with a strongly typed signature
    ///     on the generic type parameters and implement interface depending only on the container type.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <typeparam name="TPropertyType">The type of the property type.</typeparam>
    internal class PropertyCopierProxy<TContainer, TPropertyType> : PropertyProxy<TContainer, TPropertyType>,
        IPropertyCopier<TContainer>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PropertyCopierProxy&lt;TContainer, TPropertyType&gt;" /> class.
        ///     Create the dynamic accessors to get and set property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyCopierProxy(string propertyName) : base(propertyName)
        {
        }

        /// <summary>
        ///     Copies the property value from the specified source to specified destination.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public void Copy(TContainer source, TContainer destination)
        {
            PropertySetter(destination, PropertyGetter(source));
        }
    }
}