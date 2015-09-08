using System;
using System.Reflection;

namespace Lx.Tools.Reflection
{
    /// <summary>
    /// Purpose of proxy is to effectively copy property with a strongly typed signature
    /// on the generic type parameters and serves as a base class for the
    /// implementation of interfaces depending only on the container type.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <typeparam name="TPropertyType">The type of the property type.</typeparam>
    internal class PropertyProxy<TContainer, TPropertyType>
    {
        private readonly Func<TContainer, TPropertyType> getProperty;
        private readonly Action<TContainer, TPropertyType> setProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCopierProxy&lt;TContainer, TPropertyType&gt;"/> class.
        /// Create the dynamic accessors to get and set property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyProxy(string propertyName)
        {
            this.getProperty = DynamicProperty<TContainer, TPropertyType>.CreateGetPropertyDelegate(propertyName);
            this.setProperty = DynamicProperty<TContainer, TPropertyType>.CreateSetPropertyDelegate(propertyName);
        }

        /// <summary>
        /// Gets the get property.
        /// </summary>
        /// <value>The get property.</value>
        protected Func<TContainer, TPropertyType> GetProperty
        {
            get { return this.getProperty; }
        }

        /// <summary>
        /// Gets the set property.
        /// </summary>
        /// <value>The set property.</value>
        protected Action<TContainer, TPropertyType> SetProperty
        {
            get { return this.setProperty; }
        }
    }

    /// <summary>
    /// Purpose of proxy is to effectively copy property with a strongly typed signature
    /// on the generic type parameters and serves as a base class for the
    /// implementation of interfaces depending only on the container type.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <typeparam name="TPropertyType">The type of the property type.</typeparam>
    internal class PropertyProxy<TSource, TDestination, TPropertyType>
    {
        private readonly Func<TSource, TPropertyType> getProperty;
        private readonly Action<TDestination, TPropertyType> setProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCopierProxy&lt;TContainer, TPropertyType&gt;"/> class.
        /// Create the dynamic accessors to get and set property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyProxy(string propertyName)
        {
            var sourceType = typeof(TSource);
            if (!sourceType.GetRuntimeProperty(propertyName).CanRead)
            {
                throw new InvalidOperationException(sourceType + " " + propertyName + " has no getter");
            }
            this.getProperty = DynamicProperty<TSource, TPropertyType>.CreateGetPropertyDelegate(propertyName);
            var destinationType = typeof(TDestination);
            if (!destinationType.GetRuntimeProperty(propertyName).CanWrite)
            {
                throw new InvalidOperationException(destinationType + " " + propertyName + " has no setter");
            }
            this.setProperty = DynamicProperty<TDestination, TPropertyType>.CreateSetPropertyDelegate(propertyName);
        }

        /// <summary>
        /// Gets the get property.
        /// </summary>
        /// <value>The get property.</value> 
        protected Func<TSource, TPropertyType> GetProperty
        {
            get { return this.getProperty; }
        }

        /// <summary>
        /// Gets the set property.
        /// </summary>
        /// <value>The set property.</value>
        protected Action<TDestination, TPropertyType> SetProperty
        {
            get { return this.setProperty; }
        }
    }
}
