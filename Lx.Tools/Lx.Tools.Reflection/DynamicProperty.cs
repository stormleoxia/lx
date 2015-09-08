using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Lx.Tools.Reflection
{
    /// <summary>
    /// Provides fast access, strongly typed, expensive to a property
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <typeparam name="TPropertyType">The type of the property type.</typeparam>
    public static class DynamicProperty<TContainer, TPropertyType>
    {
        /// <summary>
        /// Creates the get property delegate of given name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static Func<TContainer, TPropertyType> CreateGetPropertyDelegate(string propertyName)
        {
            var containerType = typeof(TContainer);
            var param = Expression.Parameter(containerType, "container");
            var func = Expression.Lambda(
                        Expression.PropertyOrField(param, propertyName),
                        param
                        );
            return (Func<TContainer, TPropertyType>)func.Compile();
        }

        /// <summary>
        /// Creates the set property delegate of given name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static Action<TContainer, TPropertyType> CreateSetPropertyDelegate(string propertyName)
        {
            var containerType = typeof(TContainer);
            var setMethod = containerType.GetRuntimeProperty(propertyName).SetMethod;
            var paramContainer = Expression.Parameter(containerType, "container");
            var paramValue = Expression.Parameter(typeof(TPropertyType), "value");
            Expression expr = paramContainer;
            var call = Expression.Call(paramContainer, setMethod,
                                       new Expression[] { paramValue }
                    );
            var action = Expression.Lambda(call, paramContainer, paramValue);
            return (Action<TContainer, TPropertyType>)action.Compile();
        }

    }

}
