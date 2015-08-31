using Microsoft.Practices.Unity;

namespace Lx.Tools.Common
{
    public static class UnityContainerEx
    {

        public static IUnityContainer Register<TInterface, TType>(this IUnityContainer container) where TType : TInterface
        {
            container.RegisterType(typeof(TInterface), typeof(TType));
            return container;
        }
    }
}
