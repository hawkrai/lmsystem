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
    }
}
