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
    using LMPlatform.UI.Services.Modules.News;
    using LMPlatform.UI.ViewModels.SubjectModulesViewModel.ModulesViewModel;

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MaterialsService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MaterialsService.svc or MaterialsService.svc.cs at the Solution Explorer and start debugging.
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

        public void DoWork()
        {
        }
    }
}
