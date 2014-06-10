using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Application.Core;

using Application.Core.Data;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;

namespace Application.Infrastructure.FilesManagement
{
    public class FilesManagementService : IFilesManagementService
    {
        private readonly string _storageRoot = ConfigurationManager.AppSettings["FileUploadPath"];
        private readonly string _tempStorageRoot = ConfigurationManager.AppSettings["FileUploadPathTemp"];

        private readonly LazyDependency<IRepositoryBase<Attachment>> _attachmentsRepository = new LazyDependency<IRepositoryBase<Attachment>>();

        public IRepositoryBase<Attachment> AttachmentsRepository
        {
            get
            {
                return _attachmentsRepository.Value;
            }
        }

        public string GetFileDisplayName(string guid)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.AttachmentRepository.GetBy(new Query<Attachment>(e => e.FileName == guid)).Name;
            }
        }

        public void SaveFiles(IEnumerable<Attachment> attachments, string folder = "")
        {
            foreach (var attach in attachments)
            {
                SaveFile(attach, folder);
            }
        }

        public IList<Attachment> GetAttachments(string path)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return string.IsNullOrEmpty(path) ? repositoriesContainer.AttachmentRepository.GetAll().ToList() 
                    : repositoriesContainer.AttachmentRepository.GetAll(new Query<Attachment>(e => e.PathName == path)).ToList();
            }
        }

        public void DeleteFileAttachment(Attachment attachment)
        {
            var filePath = _storageRoot + attachment.PathName + "//" + attachment.FileName;

            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.AttachmentRepository.Delete(attachment);
                repositoriesContainer.ApplyChanges();
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private void SaveFile(Attachment attachment, string folder)
        {
            var targetDirectoty = Path.Combine(_storageRoot, folder);
            var tempFilePath = Path.Combine(_tempStorageRoot, attachment.FileName);
            var targetFilePath = Path.Combine(targetDirectoty, attachment.FileName);

            if (!Directory.Exists(targetDirectoty))
            {
                Directory.CreateDirectory(targetDirectoty);
            }

            if (File.Exists(tempFilePath))
            {
                File.Copy(tempFilePath, targetFilePath, true);
                File.Delete(tempFilePath);
            }
        }
    }
}