using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Application.Core;

using Application.Core.Data;
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

        //private readonly LazyDependency<ICollectionItemsAttachmentsRepository> _collectionItemsAttachmentsRepository = new LazyDependency<ICollectionItemsAttachmentsRepository>();

        //public ICollectionItemsAttachmentsRepository CollectionItemsAttachmentsRepository
        //{
        //    get
        //    {
        //        return _collectionItemsAttachmentsRepository.Value;
        //    }
        //}
        public string GetFileDisplayName(string guid)
        {
            return AttachmentsRepository.GetBy(new Query<Attachment>(e => e.FileName == guid)).Name;
        }

        public void SaveFiles(IEnumerable<Attachment> attachments, string folder = "")
        {
            foreach (var attach in attachments)
            {
                SaveFile(attach, folder);
            }
        }

        public void DeleteFileAttachment(Attachment attachment)
        {
            //if (CollectionItemsAttachmentsRepository.GetPageableBy().Items.All(e => e.AttachmentId != attachment.Id))
            //{
            string fileType = null;
            switch (attachment.AttachmentType)
            {
                case AttachmentType.Image:
                    fileType = "Image";
                    break;
                case AttachmentType.Video:
                    fileType = "Video";
                    break;
                case AttachmentType.Audio:
                    fileType = "Audio";
                    break;
                case AttachmentType.Document:
                    fileType = "Document";
                    break;
            }

            var filePath = _storageRoot + fileType + "//" + attachment.Name;

            AttachmentsRepository.Delete(attachment);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            //}
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