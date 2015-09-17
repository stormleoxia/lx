using System;
using Microsoft.Practices.Unity;
using Moq;

namespace Lx.Tools.Tests.MockUnity
{
    public static class MoqContainerEx
    {
        public static Mock<TInterface> RegisterMoqInstance<TInterface>(this IUnityContainer container)
            where TInterface : class
        {
            var mock = MockUnit.Get<TInterface>();
            container.RegisterInstance(typeof (TInterface), mock.Object);
            return mock;
        }

        public static Mock RegisterMoqInstance(this IUnityContainer container, Type interfaceType)
        {
            var mock = MockUnit.Get(interfaceType);
            container.RegisterInstance(interfaceType, mock.Object);
            return mock;
        }
    }


}
