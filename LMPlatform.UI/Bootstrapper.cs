using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;


namespace LMPlatform.UI
{
    public static class Bootstrapper
    {
        public static void Initialize()
        {
            var locator = new UnityServiceLocator(_ConfigureUnityContainer());

            ServiceLocator.SetLocatorProvider(() => locator);
        }

        public static IUnityContainer RegisterTypes(IUnityContainer container)
        {
            return container;
        }

        private static IUnityContainer _ConfigureUnityContainer()
        {
            var container = new UnityContainer();

            RegisterTypes(container);

            return container;
        }
    }
}