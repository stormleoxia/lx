namespace Lx.Tools.Reflection
{
    /// <summary>
    /// Abstraction of property copier depending on source and destination
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    public interface IPropertyCopier<in TSource, in TDestination>
    {
        /// <summary>
        ///     Copies the property value from the specified source to specified destination.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        void Copy(TSource source, TDestination destination);
    }

    /// <summary>
    ///  Abstraction of property copier depending only on the container type
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    public interface IPropertyCopier<in TContainer>
    {
        /// <summary>
        /// Copies the property value from the specified source to specified destination.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        void Copy(TContainer source, TContainer destination);
    }
}