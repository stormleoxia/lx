using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lx.Tools.Reflection
{
    /// <summary>
    /// Provide the same functionality as MemberwiseClone() but without reflection on execution
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    public class MemberwiseCloner<TContainer>
    {
        private static readonly int length;
        private static readonly IPropertyCopier<TContainer>[] copiers;

        /// <summary>
        /// Initializes the <see cref="MemberwiseCloner&lt;TContainer&gt;"/> class.
        /// </summary>
        static MemberwiseCloner()
        {
            var type = typeof(TContainer);
            var properties = type.GetRuntimeProperties().ToArray();
            length = properties.Length;
            var list = new List<IPropertyCopier<TContainer>>(length);
            for (int i = 0; i < length; ++i)
            {
                var property = properties[i];
                if (property.CanRead && property.CanWrite)
                {
                    list.Add(new PropertyCopier<TContainer>(property.Name));
                }
            }
            length = list.Count;
            copiers = list.ToArray();
        }

        /// <summary>
        /// Do a shallow copy.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void ShallowCopy(TContainer source, TContainer destination)
        {
            for (int i = 0; i < length; ++i)
            {
                copiers[i].Copy(source, destination);
            }
        }
    }

    /// <summary>
    /// Provide the same functionality as MemberwiseClone() but without reflection on execution
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the container.</typeparam>
    public class MemberwiseCloner<TSource, TDestination>
    {
        private static readonly int length;
        private static readonly IPropertyCopier<TSource, TDestination>[] copiers;

        /// <summary>
        /// Initializes the <see cref="MemberwiseCloner&lt;TSource, TDestination&gt;"/> class.
        /// </summary>
        static MemberwiseCloner()
        {
            var type = typeof(TSource);
            var properties = type.GetRuntimeProperties().ToArray();
            length = properties.Length;
            copiers = new IPropertyCopier<TSource, TDestination>[length];
            for (int i = 0; i < length; ++i)
            {
                var property = properties[i];
                copiers[i] = new PropertyCopier<TSource, TDestination>(property.Name);
            }
        }

        /// <summary>
        /// Do a shallow copy.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void ShallowCopy(TSource source, TDestination destination)
        {
            for (int i = 0; i < length; ++i)
            {
                copiers[i].Copy(source, destination);
            }
        }
    }
}
