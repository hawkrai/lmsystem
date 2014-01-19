using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Application.Core;
using Application.Core.Exceptions;
using Application.Core.UI;
using Application.Core.UI.Controllers;
using Application.Infrastructure.FilesManagement;
using Newtonsoft.Json;

namespace LMPlatform.UI.ApiControllers
{
    public class UploadController : ApiController
	{
		private readonly LazyDependency<IFilesManagementService> _filesManagementService = new LazyDependency<IFilesManagementService>();

		public IFilesManagementService FilesManagementService
		{
			get
			{
				return _filesManagementService.Value;
			}
		}

		#region UploadController Members

		[System.Web.Http.HttpDelete]
		public string DeleteFiles()
		{
			return HttpStatusCode.OK.ToString();
		}

		[System.Web.Http.HttpGet]
		public HttpResponseMessage DownloadFile(string fileName)
		{
		    if (!string.IsNullOrEmpty(fileName))
		    {
		        DownloadFileContent(fileName);
		    }
		    
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
		}

		[System.Web.Http.HttpGet]
		public IEnumerable<AttachedFile> GetUploadedFiles(string values, string deleteValues)
		{
            if (!string.IsNullOrEmpty(values))
			{
				return _GetUploadedFiles(values, deleteValues);
			}
			else
			{
				return new List<AttachedFile>();
			}
		}

		[System.Web.Http.HttpPost]
		public IEnumerable<AttachedFile> UploadFiles()
		{
			return _UploadFile(HttpContext.Current);
		}

		#endregion UploadController Members

		#region Fields

        private readonly string _storageRoot = ConfigurationManager.AppSettings["FileUploadPath"];
        private readonly string _storageRootTemp = ConfigurationManager.AppSettings["FileUploadPathTemp"];

		#endregion Fields

		#region Private Members

		private HttpResponseMessage DownloadFileContent(string fileName)
		{
			var filePath = _storageRoot + fileName;
			if (File.Exists(filePath))
			{
				var response = new HttpResponseMessage(HttpStatusCode.OK)
				{
					Content = new StreamContent(new FileStream(filePath, FileMode.Open, FileAccess.Read))
				};
				response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                if (HttpContext.Current.Request.Browser.Browser == "IE")
                {
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = HttpUtility.UrlPathEncode(FilesManagementService.GetFileDisplayName(Path.GetFileName(filePath))),
                    };
                }
                else
                {
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = FilesManagementService.GetFileDisplayName(Path.GetFileName(filePath))
                    };
                }

				return response;
			}

		    return null;
		}

		private IEnumerable<AttachedFile> _GetUploadedFiles(string filesPath, string deleteValues)
		{
			var values = JsonConvert.DeserializeObject<List<string>>(filesPath);
			var files =
				values.Select(value => value.Split(new[] { '/' }))
					.Select(
						split =>
							new AttachedFile(split[0], split[3], new FileInfo(_storageRoot + split[2] + "//" + split[3]), Convert.ToInt32(split[1]), deleteValues))
					.ToList();

			return files;
		}

		private string _GetFileType(HttpPostedFile fileType)
		{
			var split = fileType.ContentType.Split(new[] { '/' });
			switch (split[0])
			{
				case "image":
					return "Image";
				case "video":
					return "Video";
				case "audio":
					return "Audio";
			}

			return "Document";
		}

		private IEnumerable<AttachedFile> _UploadFile(HttpContext context)
		{
			var statuses = new List<AttachedFile>();

			for (var i = 0; i < context.Request.Files.Count; i++)
			{
				var file = context.Request.Files[i];
				var guidFileName = GetGuidFileName() + Path.GetExtension(file.FileName.ToLower());
                var fullPath = _storageRootTemp + guidFileName;

				file.SaveAs(fullPath);

				statuses.Add(new AttachedFile(Path.GetFileName(file.FileName), guidFileName, new FileInfo(fullPath), file.ContentLength, fullPath, "DELETE"));
			}

			return statuses;
		}

		private string GetGuidFileName()
		{
			return string.Format("N{0}", Guid.NewGuid().ToString("N").ToUpper());
		}

		#endregion Private Members
	}
}