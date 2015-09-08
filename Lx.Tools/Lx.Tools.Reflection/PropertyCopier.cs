using System;
using System.Reflection;

namespace Lx.Tools.Reflection
{
    /// <summary>
    /// Interface for property copier between two objects of the same type.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    public interface IPropertyCopier<TContainer>
    {
        /// <summary >
        /// Copies the property value from the specified source to specified destination.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        void Copy(TContainer source, TContainer destination);
    }

    /// <summary>
    /// Interface for property copier between two objects of the same type.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    public interface IPropertyCopier<TSource, TDestination>
    {
        /// <summary >
        /// Copies the property value from the specified source to specified destination.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        void Copy(TSource source, TDestination destination);
    }

    /// <summary>
    /// Purpose of proxy is to effectively copy property with a strongly typed signature
    /// on the generic type parameters and implement interface depending only on the container type.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <typeparam name="TPropertyType">The type of the property type.</typeparam>
    internal class PropertyCopierProxy<TContainer, TPropertyType> : PropertyProxy<TContainer, TPropertyType>, IPropertyCopier<TContainer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCopierProxy&lt;TContainer, TPropertyType&gt;"/> class.
        /// Create the dynamic accessors to get and set property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyCopierProxy(string propertyName) : base(propertyName)
        {}

        /// <summary >
        /// Copies the property value from the specified source to specified destination.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public void Copy(TContainer source, TContainer destination)
        {
            SetProperty(destination, GetProperty(source));
        }
    }

    /// <summary>
    /// Purpose of proxy is to effectively copy property with a strongly typed signature
    /// on the generic type parameters and implement interface depending only on the container type.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <typeparam name="TPropertyType">The type of the property type.</typeparam>
    internal class PropertyCopierProxy<TSource, TDestination, TPropertyType>: PropertyProxy<TSource, TDestination, TPropertyType>, IPropertyCopier<TSource, TDestination>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCopierProxy&lt;TContainer, TPropertyType&gt;"/> class.
        /// Create the dynamic accessors to get and set property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyCopierProxy(string propertyName):
            base(propertyName)
        { }

        /// <summary >
        /// Copies the property value from the specified source to specified destination.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public void Copy(TSource source, TDestination destination)
        {
            SetProperty(destination, GetProperty(source));
        }
    }

    /// <summary>
    /// Fast Access to Property Copy between two objects of the same type.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    public class PropertyCopier<TContainer> : IPropertyCopier<TContainer>
    {
        private readonly IPropertyCopier<TContainer> copier;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCopier&lt;TContainer&gt;"/> class.
        /// Instantiate the Proxy Type Strongly Typed depending on the type of the given property 
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyCopier(string propertyName)
        {
            var containerType = typeof(TContainer);
            var propertyType = containerType.GetRuntimeProperty(propertyName).PropertyType;
            var dynamicProperty = typeof(PropertyCopierProxy<,>).MakeGenericType(containerType, propertyType);
            copier = (IPropertyCopier<TContainer>)Activator.CreateInstance(dynamicProperty, new object[] { propertyName });
        }

        /// <summary >
        /// Copies the property value from the specified source to specified destination.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public void Copy(TContainer source, TContainer destination)
        {
            copier.Copy(source, destination);
        }
    }

    /// <summary>
    /// Fast Access to Property Copy between two objects of the same type.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    public class PropertyCopier<TSource, TDestination> : IPropertyCopier<TSource, TDestination>
    {
        private readonly IPropertyCopier<TSource, TDestination> copier;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCopier&lt;TContainer&gt;"/> class.
        /// Instantiate the Proxy Type Strongly Typed depending on the type of the given property 
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public PropertyCopier(string propertyName)
        {
            var sourceType = typeof(TSource);
            var propertyType = sourceType.GetRuntimeProperty(propertyName).PropertyType;
            var destinationType = typeof(TDestination);
            if (propertyType != destinationType.GetRuntimeProperty(propertyName).PropertyType)
            {
                throw new InvalidOperationException(sourceType + " " + destinationType + " property " +  propertyName + " type mismatch");
            }
            var dynamicProperty = typeof(PropertyCopierProxy<,,>).MakeGenericType(sourceType, destinationType, propertyType);
            copier = (IPropertyCopier<TSource, TDestination>)Activator.CreateInstance(dynamicProperty, new object[] { propertyName });
        }

        /// <summary >
        /// Copies the property value from the specified source to specified destination.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public void Copy(TSource source, TDestination destination)
        {
            copier.Copy(source, destination);
        }
    }
}
