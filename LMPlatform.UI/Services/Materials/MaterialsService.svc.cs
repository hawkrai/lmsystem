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

        public FoldersResult GetFolders(string Pid)
        {
            int pid = int.Parse(Pid);
            List<Folders> fl = MaterialsManagementService.GetFolders(pid);
            return new FoldersResult
            {
                Folders = fl.Select(e => new FoldersViewData(e)).ToList(),
                Message = "Сообщение отправлено",
                Code = "200"
            };
        }
        
        public FoldersResult BackspaceFolder(string Id)
        {
            int id = int.Parse(Id);
            int pid = MaterialsManagementService.GetPidById(id);
            List<Folders> fl = MaterialsManagementService.GetFolders(pid);
            return new FoldersResult
            {
                Pid = pid,
                Folders = fl.Select(e => new FoldersViewData(e)).ToList(),
                Message = "Сообщение отправлено",
                Code = "200"
            };
        }

        public FoldersResult CreateFolder(string Pid)
        {
            try
            {
                int pid = int.Parse(Pid);
                Folders fls = MaterialsManagementService.CreateFolder(pid);
                List<Folders> fl = MaterialsManagementService.GetFolders(pid);
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
                List<Folders> fl = MaterialsManagementService.GetFolders(idfolder);
                return new FoldersResult
                {
                    Folders = fl.Select(e => new FoldersViewData(e)).ToList(),
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

        public FoldersResult RenameFolder(string id, string pid, string newName)
        {
            try
            {
                int idfolder = int.Parse(id);
                int parentIdFolder = int.Parse(pid);
                string name = newName;
                MaterialsManagementService.RenameFolder(idfolder, name);
                List<Folders> fl = MaterialsManagementService.GetFolders(parentIdFolder);
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

        public FoldersResult SaveTextMaterials(string idf, string name, string text)
        {
            int idfolder = int.Parse(idf);
            MaterialsManagementService.SaveTextMaterials(idfolder, name, text);
            return new FoldersResult
            {
                Message = "Ошибка переименования папки",
                Code = "500"
            };
        }
    }
}
