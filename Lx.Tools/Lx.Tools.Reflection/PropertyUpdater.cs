using System;
using System.Reflection;

namespace Lx.Tools.Reflection
{
    /// <summary>
    /// Interface for property copy to a given container based on the difference between two instances of the same type.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    public interface IPropertyUpdater<TContainer>
    {
        /// <summary>
        /// Updates a property value of destination with the property value of source 
        /// if property value of source is different from property value of reference.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        void Update(TContainer reference, TContainer source, TContainer destination);
    }

    /// <summary>
    /// Strongly typed Fast Access to property update based on the difference between two instances of the same type  
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <typeparam name="TPropertyType">The type of the property type.</typeparam>
    internal class PropertyUpdaterProxy<TContainer, TPropertyType> : PropertyProxy<TContainer, TPropertyType>, IPropertyUpdater<TContainer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyUpdaterProxy&lt;TContainer, TPropertyType&gt;"/> class.
        /// Create the dynamic accessors to get and set property, by reflection and compilation.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyUpdaterProxy(string propertyName) : base(propertyName)
        {
        }

        /// <summary>
        /// Updates a property value of destination with the property value of source
        /// if property value of source is different from property value of reference.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public void Update(TContainer reference, TContainer source, TContainer destination)
        {
            var sourceValue = GetProperty(source);
            if (!GetProperty(reference).Equals(sourceValue))
            {
                SetProperty(destination, sourceValue);
            }
        }
    }

    /// <summary>
    /// Fast access to property update based on the difference between two instances of the same type
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    public class PropertyUpdater<TContainer> : IPropertyUpdater<TContainer>
    {
        private readonly IPropertyUpdater<TContainer> updater;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyUpdater&lt;TContainer&gt;"/> class.
        /// Instantiate by reflection the strongly typed proxy which will do the update
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyUpdater(string propertyName)
        {
            var containerType = typeof(TContainer);
            var propertyType = containerType.GetRuntimeProperty(propertyName).PropertyType;
            var dynamicProperty = typeof(PropertyUpdaterProxy<,>).MakeGenericType(containerType, propertyType);
            updater = (IPropertyUpdater<TContainer>)Activator.CreateInstance(dynamicProperty, new object[] { propertyName });
        }

        /// <summary>
        /// Updates a property value of destination with the property value of source
        /// if property value of source is different from property value of reference.
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
