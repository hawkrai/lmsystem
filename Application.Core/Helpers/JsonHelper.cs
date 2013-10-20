using System.Collections.Generic;
using Newtonsoft.Json;

namespace Application.Core.Helpers
{
	public class JsonHelper
	{
		public static IEnumerable<TResult> DeserializeIEnumerable<TResult>(string jsonString)
		{
			IEnumerable<TResult> result = new List<TResult>();

			if (!string.IsNullOrEmpty(jsonString))
			{
				result = JsonConvert.DeserializeObject<IEnumerable<TResult>>(jsonString);
			}

			return result;
		}

		public static string SerializeObject(object serializingObject)
		{
			var result = JsonConvert.SerializeObject(serializingObject);

			return result;
		}
	}
}
