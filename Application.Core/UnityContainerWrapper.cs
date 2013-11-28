using System;
using Microsoft.Practices.Unity;

namespace Application.Core
{
	public class UnityContainerWrapper : IUnityContainerWrapper
	{
		private readonly IUnityContainer _unityContainer;

		public UnityContainerWrapper(IUnityContainer container)
		{
			_unityContainer = container;
		}

		public IUnityContainerWrapper Register<T, TC>() where TC : T
		{
			_unityContainer.RegisterType<T, TC>();

			return this;
		}

		public IUnityContainer Container
		{
			get
			{
				return _unityContainer;
			}
		}
	}
}
