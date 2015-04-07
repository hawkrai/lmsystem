using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Core.UI.Controllers;
using Application.Infrastructure.FoldersManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;

namespace LMPlatform.UI.Controllers
{
    public class MaterialsController : BasicController
    {
        public ActionResult Index(int subjectId)
        {
            List<Folders> folders = FoldersManagementService.GetAllFolders();
            ViewBag.folders = folders;
            Subject subject = SubjectManagementService.GetSubject(subjectId);

            int SubjectModulesId = subject.SubjectModules.First().Id;

            Folders rootFolder = FoldersManagementService.FolderRootBySubjectModuleId(SubjectModulesId);

            ViewBag.Folder = rootFolder;
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
            //ViewBag.NgApp = "materialsApp";
            //ViewBag.NgController = "newCtrl";
            return PartialView();
        }

        public IFoldersManagementService FoldersManagementService
        {
            get
            {
                return ApplicationService<IFoldersManagementService>();
            }
        }

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return ApplicationService<ISubjectManagementService>();
            }
        }
    }
}
