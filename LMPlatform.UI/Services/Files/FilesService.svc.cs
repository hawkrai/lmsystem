using System;
using System.Linq;
using LMPlatform.UI.Services.Modules.Files;
using System.Configuration;
using Application.Core;
using Application.Infrastructure.FilesManagement;

namespace LMPlatform.UI.Services.Files
{
	public class FilesService : IFilesService
	{
		private readonly LazyDependency<IFilesManagementService> _filesManagementService =
			new LazyDependency<IFilesManagementService>();

		public IFilesManagementService FilesManagementService => this._filesManagementService.Value;

		public AttachmentResult GetFiles()
		{
			try
			{
				var attachments = this.FilesManagementService.GetAttachments(null).ToList();
				var storageRoot = ConfigurationManager.AppSettings["FileUploadPath"];
				var result = new AttachmentResult
				{
					Files = attachments,
					ServerPath = storageRoot,
					Message = "Данные успешно загружены",
					Code = "200"
				};

				return result;
			}
			catch
			{
				return new AttachmentResult
				{
					Message = "Произошла ошибка при получении данных",
					Code = "500"
				};
			}
		}

		public AttachmentResult GetFilesWithPagination(string namePart, int filesPerPage, int pageNumber)
		{
			try
			{
				var attachments = this.FilesManagementService.GetAttachments(namePart, filesPerPage, pageNumber)
					.ToList();
				var storageRoot = ConfigurationManager.AppSettings["FileUploadPath"];
				var result = new AttachmentResult
				{
					Files = attachments,
					ServerPath = storageRoot,
					Message = "Данные успешно загружены",
					Code = "200"
				};

				return result;
			}
			catch
			{
				return new AttachmentResult
				{
					Message = "Произошла ошибка при получении данных",
					Code = "500"
				};
			}
		}
	}
}