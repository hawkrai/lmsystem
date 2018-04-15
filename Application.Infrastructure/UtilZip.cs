using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;
using LMPlatform.Models;

namespace Application.Infrastructure
{
	public static class UtilZip
	{
		public static ZipFile CreateZipFile(string path, ZipFile zip, List<Attachment> files, string userName)
		{
			//var zip = new ZipFile(Encoding.UTF8);
			zip.AddDirectoryByName(userName);
			var i = 1;
			foreach (var fileName in files)
			{

				if (File.Exists(path + fileName.PathName + "//" + fileName.FileName))
				{
					zip.AddFile(path + fileName.PathName + "//" + fileName.FileName, userName).FileName = "\\" + userName  + "\\"+ i.ToString() + "." + fileName.Name;
					i++;
				}				
			}

			return zip;
		}
	}
}
