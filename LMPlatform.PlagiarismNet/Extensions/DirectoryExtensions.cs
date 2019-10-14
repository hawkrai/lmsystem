using System.Collections.Generic;
using System.IO;

namespace LMPlatform.PlagiarismNet.Extensions
{
	public static class CustomDirectory
	{
		public static List<string> GetFiles(string path, string searchPattern,
			SearchOption option)
		{
			var patterns = searchPattern.Split('|');
			List<string> files = new List<string>();
			foreach (string sp in patterns)
				files.AddRange(System.IO.Directory.GetFiles(path, sp, option));
			files.Sort();
			return files;
		}
	}
}