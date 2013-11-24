using System;
using System.ComponentModel;

namespace Application.Core.Extensions
{
	public static class CommonExtensions
	{
		public static string GetDescription(this Enum sourceObject)
		{
			return GetEnumDescription(sourceObject);
		}

		private static string GetEnumDescription<TEnum>(TEnum value)
		{
			var fieldInfo = value.GetType().GetField(value.ToString());

			var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

			return (attributes.Length > 0)
				? attributes[0].Description
				: value.ToString();
		}
	}
}
