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