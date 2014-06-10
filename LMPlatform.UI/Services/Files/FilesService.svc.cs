using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LMPlatform.UI.Services.Files
{
    using System.Configuration;

    using Application.Core;
    using Application.Infrastructure.FilesManagement;
    using Application.Infrastructure.MessageManagement;

    using LMPlatform.UI.Services.Modules.Lectures;
    using LMPlatform.UI.Services.Modules.Messages;

    public class FilesService : IFilesService
    {
        private readonly LazyDependency<IFilesManagementService> _filesManagementService =
                                                new LazyDependency<IFilesManagementService>();

        public IFilesManagementService FilesManagementService
        {
            get { return _filesManagementService.Value; }
        }

        public AttachmentResult GetFiles()
        {
            try
            {
                var attachments = FilesManagementService.GetAttachments(null).ToList();
                string storageRoot = ConfigurationManager.AppSettings["FileUploadPath"];
                var result = new AttachmentResult
                {
                    Files = attachments,
                    ServerPath = storageRoot,
                    Message = "Данные успешно загружены",
                    Code = "200"
                };

                return result;
            }
            catch (Exception e)
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
