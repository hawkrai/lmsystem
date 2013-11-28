using Application.Core;
using Application.Infrastructure;
using LMPlatform.Data.Infrastructure;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace LMPlatform.UI
{
    public static class Bootstrapper
    {
        public static void Initialize()
        {
            var locator = new UnityServiceLocator(_ConfigureUnityContainer());

            ServiceLocator.SetLocatorProvider(() => locator);
        }

        public static IUnityContainer RegisterTypes(IUnityContainerWrapper containerWrapper)
        {
            DataModule.Initialize(containerWrapper);
            ApplicationServiceModule.Initialize(containerWrapper);

            return containerWrapper.Container;
        }

        private static IUnityContainer _ConfigureUnityContainer()
        {
            var container = new UnityContainer();
            var containerWrapper = new UnityContainerWrapper(container);

            return RegisterTypes(containerWrapper);
        }
    }
}