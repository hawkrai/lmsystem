using System.Linq;

namespace Application.Core.Extensions
{
	using System;

	public static class StringExtentions
	{
		public static string Crop(this string source, int maxLength)
		{
			if (!string.IsNullOrEmpty(source))
			{
				if (source.Length <= maxLength)
				{
					return source;
				}

				return source.Substring(0, maxLength);
			}

			return source;
		}

        public static string ToUpperFirst(this string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return string.Empty;
            }

            return char.ToUpper(inputString[0]) + inputString.Substring(1);
        }

		public static byte[] GetBytes(this string source)
		{
			var bytes = new byte[source.Length * sizeof(char)];

			Buffer.BlockCopy(source.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}

		public static string GetString(this byte[] bytes)
		{
			var chars = new char[bytes.Length / sizeof(char)];
			Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
			return new string(chars);
		}

        public static string RemoveStringEntries(this string str, params string[] entries)
        {
            return entries.Aggregate(str, (current, entry) => current.Replace(entry, string.Empty));
        }
	}
}
