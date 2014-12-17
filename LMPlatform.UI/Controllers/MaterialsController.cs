using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Core.UI.Controllers;
using Application.Infrastructure.FoldersManagement;
using LMPlatform.Models;

namespace LMPlatform.UI.Controllers
{
    public class MaterialsController : BasicController
    {
        public ActionResult Index()
        {
            List<Folders> folders = FoldersManagementService.GetAllFolders();
            ViewBag.folders = folders;

            ViewBag.NgApp = "materialsApp";
            ViewBag.NgController = "homeCtrl";

            return View();
        }

        public ActionResult Catalog()
        {
            ViewBag.Pid = 0;
            return PartialView();
        }

        public ActionResult New()
        {
            return View();
        }

        #region Dependencies

        public IFoldersManagementService FoldersManagementService
        {
            get
            {
                return ApplicationService<IFoldersManagementService>();
            }
        }
        #endregion
    }
}
