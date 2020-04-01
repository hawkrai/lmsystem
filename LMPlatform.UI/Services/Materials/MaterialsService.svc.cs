using System;
using System.Linq;
using Application.Core;
using Application.Infrastructure.MaterialsManagement;
using LMPlatform.UI.Services.Modules.Materials;

namespace LMPlatform.UI.Services.Materials
{
    public class MaterialsService : IMaterialsService
    {
        private readonly LazyDependency<IMaterialsManagementService> _materialsManagementService = new LazyDependency<IMaterialsManagementService>();

        public IMaterialsManagementService MaterialsManagementService => _materialsManagementService.Value;

        public FoldersResult GetFolders(int pid, int subjectId)
        {
	        var fl = MaterialsManagementService.GetFolders(pid, subjectId);
            return new FoldersResult
            {
                Folders = fl.Select(e => new FoldersViewData(e)).ToList(),
                Message = "Сообщение отправлено",
                Code = "200"
            };
        }

        public FoldersResult BackspaceFolder(int id, int subjectId)
        {
            var pid = MaterialsManagementService.GetPidById(id);
            var fl = MaterialsManagementService.GetFolders(pid, subjectId);
            return new FoldersResult
            {
                Pid = pid,
                Folders = fl.Select(e => new FoldersViewData(e)).ToList(),
                Message = "Сообщение отправлено",
                Code = "200"
            };
        }

        public FoldersResult CreateFolder(int pid, int subjectId)
        {
            try
            {
	            var fls = MaterialsManagementService.CreateFolder(pid, subjectId);
                var fl = MaterialsManagementService.GetFolders(pid, subjectId);
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

        public FoldersResult DeleteFolder(int idFolder)
        {
            try
            {
                MaterialsManagementService.DeleteFolder(idFolder);
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

        public DocumentsResult DeleteDocument(int idDocument, int pid, int subjectId)
        {
            try
            {
                MaterialsManagementService.DeleteDocument(idDocument);
                var fl = MaterialsManagementService.GetDocumentsByIdFolders(pid, subjectId);

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

        public FoldersResult RenameFolder(int id, int parentIdFolder, string name, int subjectId)
        {
            try
            {
                MaterialsManagementService.RenameFolder(id, name);
                var fl = MaterialsManagementService.GetFolders(parentIdFolder, subjectId);
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

        public DocumentsResult RenameDocument(int id, int parentIdFolder, string newName, int subjectId)
        {
            try
            {
                MaterialsManagementService.RenameDocument(id, newName);
                var fl = MaterialsManagementService.GetDocumentsByIdFolders(parentIdFolder, subjectId);
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

        public FoldersResult SaveTextMaterials(int idDocument, int idFolder, string name, string text, int subjectId)
        {
	        MaterialsManagementService.SaveTextMaterials(idDocument, idFolder, name, text, subjectId);
            return new FoldersResult
            {
                Message = "Ошибка переименования папки",
                Code = "500"
            };
        }

        public DocumentsResult GetDocuments(int id, int subjectId)
        {
            var dc = MaterialsManagementService.GetDocumentsByIdFolders(id, subjectId);
            return new DocumentsResult
            {
                Documents = dc.Select(e => new DocumentsViewData(e)).ToList(),
                Message = "Сообщение отправлено",
                Code = "200"
            };
        }

        public DocumentsResult GetText(int id)
        {
            var document = MaterialsManagementService.GetTextById(id);
            if (document is null)
            {
                return new DocumentsResult
                {
	                Message = "Материал не найден",
	                Code = "400"
                };
            }
            return new DocumentsResult
            {
                Document = new DocumentsViewData(document),
                Message = "Материал получен",
                Code = "200"
            };
        }
    }
}
