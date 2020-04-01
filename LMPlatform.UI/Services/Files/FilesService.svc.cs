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

        public IFilesManagementService FilesManagementService => _filesManagementService.Value;

        public AttachmentResult GetFiles()
        {
            try
            {
                var attachments = FilesManagementService.GetAttachments(null).ToList();
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
