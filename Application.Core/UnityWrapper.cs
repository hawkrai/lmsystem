using Microsoft.Practices.ServiceLocation;

namespace Application.Core
{
	public class UnityWrapper
	{
		public static T Resolve<T>()
		{
			return ServiceLocator.Current.GetInstance<T>();
		}
	}
}
