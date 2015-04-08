using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LMPlatform.UI.Services.Materials
{
    using System.Web.Http;
    using System.Web.Mvc;

    using Application.Core;
    using Application.Infrastructure.MaterialsManagement;

    using LMPlatform.Models;
    using LMPlatform.UI.Services.Modules;
    using LMPlatform.UI.Services.Modules.Materials;
    using LMPlatform.UI.ViewModels.SubjectModulesViewModel.ModulesViewModel;

    public class MaterialsService : IMaterialsService
    {
        private readonly LazyDependency<IMaterialsManagementService> _materialsManagementService = new LazyDependency<IMaterialsManagementService>();

        public IMaterialsManagementService MaterialsManagementService
        {
            get
            {
                return _materialsManagementService.Value;
            }
        }

        public FoldersResult GetFolders(string Pid, string subjectId)
        {
            int pid = int.Parse(Pid);
            int subjectid = int.Parse(subjectId);
            List<Folders> fl = MaterialsManagementService.GetFolders(pid, subjectid);
            return new FoldersResult
            {
                Folders = fl.Select(e => new FoldersViewData(e)).ToList(),
                Message = "Сообщение отправлено",
                Code = "200"
            };
        }

        public FoldersResult BackspaceFolder(string Id, string subjectId)
        {
            int id = int.Parse(Id);
            int subjectid = int.Parse(subjectId);
            int pid = MaterialsManagementService.GetPidById(id);
            List<Folders> fl = MaterialsManagementService.GetFolders(pid, subjectid);
            return new FoldersResult
            {
                Pid = pid,
                Folders = fl.Select(e => new FoldersViewData(e)).ToList(),
                Message = "Сообщение отправлено",
                Code = "200"
            };
        }

        public FoldersResult CreateFolder(string Pid, string subjectId)
        {
            try
            {
                int pid = int.Parse(Pid);
                int subjectid = int.Parse(subjectId);
                Folders fls = MaterialsManagementService.CreateFolder(pid, subjectid);
                List<Folders> fl = MaterialsManagementService.GetFolders(pid, subjectid);
                return new FoldersResult
                {
                    Folders = fl.Select(e => new FoldersViewData(e)).ToList(),
                    Message = "Папка создана",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new FoldersResult
                {
                    Message = "Ошибка создания папки",
                    Code = "500"
                };
            }
        }

        public FoldersResult DeleteFolder(string IdFolder)
        {
            try
            {
                int idfolder = int.Parse(IdFolder);
                MaterialsManagementService.DeleteFolder(idfolder);
                return new FoldersResult
                {
                    Message = "Папка удалена",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new FoldersResult
                {
                    Message = "Ошибка удаления папки",
                    Code = "500"
                };
            }
        }

        public DocumentsResult DeleteDocument(string IdDocument, string pid, string subjectId)
        {
            try
            {
                int iddocument = int.Parse(IdDocument);
                int parentIdFolder = int.Parse(pid);
                int subjectid = int.Parse(subjectId);
                MaterialsManagementService.DeleteDocument(iddocument);
                List<Materials> fl = MaterialsManagementService.GetDocumentsByIdFolders(parentIdFolder, subjectid);

                return new DocumentsResult
                {
                    Documents = fl.Select(e => new DocumentsViewData(e)).ToList(),
                    Message = "Документ удален",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new DocumentsResult
                {
                    Message = "Ошибка удаления документа",
                    Code = "500"
                };
            }
        }

        public FoldersResult RenameFolder(string id, string pid, string newName, string subjectId)
        {
            try
            {
                int idfolder = int.Parse(id);
                int parentIdFolder = int.Parse(pid);
                int subjectid = int.Parse(subjectId);
                string name = newName;
                MaterialsManagementService.RenameFolder(idfolder, name);
                List<Folders> fl = MaterialsManagementService.GetFolders(parentIdFolder, subjectid);
                return new FoldersResult
                {
                    Folders = fl.Select(e => new FoldersViewData(e)).ToList(),
                    Message = "Папка переименована",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new FoldersResult
                {
                    Message = "Ошибка переименования папки",
                    Code = "500"
                };
            }
        }

        public DocumentsResult RenameDocument(string id, string pid, string newName, string subjectId)
        {
            try
            {
                int iddocument = int.Parse(id);
                int parentIdFolder = int.Parse(pid);
                int subjectid = int.Parse(subjectId);
                string name = newName;
                MaterialsManagementService.RenameDocument(iddocument, name);
                List<Materials> fl = MaterialsManagementService.GetDocumentsByIdFolders(parentIdFolder, subjectid);
                return new DocumentsResult
                {
                    Documents = fl.Select(e => new DocumentsViewData(e)).ToList(),
                    Message = "Папка переименована",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new DocumentsResult
                {
                    Message = "Ошибка переименования папки",
                    Code = "500"
                };
            }
        }

        public FoldersResult SaveTextMaterials(string idd, string idf, string name, string text, string subjectId)
        {
            int id_document = int.Parse(idd);
            int id_folder = int.Parse(idf);
            int subjectid = int.Parse(subjectId);
            MaterialsManagementService.SaveTextMaterials(id_document, id_folder, name, text, subjectid);
            return new FoldersResult
            {
                Message = "Ошибка переименования папки",
                Code = "500"
            };
        }

        public DocumentsResult GetDocuments(string Pid, string subjectId)
        {
            int id = int.Parse(Pid);
            int subjectid = int.Parse(subjectId);
            List<Materials> dc = MaterialsManagementService.GetDocumentsByIdFolders(id, subjectid);
            return new DocumentsResult
            {
                Documents = dc.Select(e => new DocumentsViewData(e)).ToList(),
                Message = "Сообщение отправлено",
                Code = "200"
            };
        }

        public DocumentsResult GetText(string IdDocument)
        {
            int id = int.Parse(IdDocument);
            Materials document = MaterialsManagementService.GetTextById(id);
            return new DocumentsResult
            {
                Document = new DocumentsViewData(document),
                Message = "Сообщение отправлено",
                Code = "200"
            };
        }
    }
}
