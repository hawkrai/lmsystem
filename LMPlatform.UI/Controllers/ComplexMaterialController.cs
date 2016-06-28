using Application.Core.UI.Controllers;
using Application.Infrastructure.ConceptManagement;
using Application.Infrastructure.FoldersManagement;
using Application.Infrastructure.KnowledgeTestsManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;
using LMPlatform.UI.ViewModels.ComplexMaterialsViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
    public class ComplexMaterialController : BasicController
    {
        public ActionResult Index(int subjectId = 1)
        {
            IEnumerable<Concept> conceptList = ConceptManagementService.GetRootElements(WebSecurity.CurrentUserId);
            ViewBag.NgApp = "complexMaterialsApp";
            ViewBag.NgController = "homeCtrl";
            return View();
        }

        [HttpPost]
        public ActionResult SaveRootConcept(AddOrEditRootConceptViewModel model)
        {
            model.Save();
            return null;
        }

        [HttpPost]
        public ActionResult SaveConcept(AddOrEditConceptViewModel model)
        {
            if (!model.IsGroup && !String.IsNullOrEmpty(model.FileData))
            {
                var attachmentsModel = JsonConvert.DeserializeObject<List<Attachment>>(model.FileData).ToList();
                model.SetAttachments(attachmentsModel);
            }
            model.Save();
            return null;
        }


        public ActionResult AddRootConcept(String subjectId)
        {
            Int32 currentSubjectId = 0;
            var conceptViewModel = new AddOrEditRootConceptViewModel(WebSecurity.CurrentUserId, 0);
            if (Int32.TryParse(subjectId, out currentSubjectId))
                conceptViewModel.SelectedSubjectId = currentSubjectId;
            return PartialView("_AddRootConceptForm", conceptViewModel);
        }

        public ActionResult EditRootConcept(Int32 id)
        {
            var conceptViewModel = new AddOrEditRootConceptViewModel(WebSecurity.CurrentUserId, id);
            return PartialView("_AddRootConceptForm", conceptViewModel);
        }

        public ActionResult AddConcept(Int32 parentId)
        {
            var conceptViewModel = new AddOrEditConceptViewModel(WebSecurity.CurrentUserId, 0, parentId);
            return PartialView("_AddConceptForm", conceptViewModel);
        }

        public ActionResult AddFolderConcept(Int32 parentId)
        {
            var conceptViewModel = new AddOrEditConceptViewModel(WebSecurity.CurrentUserId, 0, parentId);
            conceptViewModel.IsGroup = true;
            return PartialView("_AddFolderConceptForm", conceptViewModel);
        }

        public ActionResult EditConcept(Int32 id, Int32 parentId)
        {
            var conceptViewModel = new AddOrEditConceptViewModel(WebSecurity.CurrentUserId, id, parentId);
            if (conceptViewModel.IsGroup)
                return PartialView("_AddFolderConceptForm", conceptViewModel);
            else
                return PartialView("_AddConceptForm", conceptViewModel);
        }

        public ActionResult OpenConcept(Int32 id)
        {
            var conceptViewModel = new AddOrEditConceptViewModel(WebSecurity.CurrentUserId, id);
            return PartialView("_OpenConceptForm", conceptViewModel);
        }

        public ActionResult ShowMemo()
        {
            return PartialView("_MemoForm");
        }

        public ActionResult Catalog()
        {
            return PartialView();
        }

        //public ActionResult Tests(Int32 subjectId)
        //{
        //    Subject subject = SubjectsManagementService.GetSubject(subjectId);
        //    return PartialView();
        //}

        public ActionResult Tests(String subjectId)
        {
            Subject subject = SubjectsManagementService.GetSubject(1);
            return PartialView(subject);
            //return PartialView("~/Views/Tests/KnowledgeTesting.cshtml", subject);
            //return PartialView("~/Content/KnowledgeTesting/passing.html", subject);
            
        }
       

        public ActionResult Map()
        {
            return PartialView();
        }

        public IConceptManagementService ConceptManagementService
        {
            get
            {
                return ApplicationService<IConceptManagementService>();
            }
        }

        public ISubjectManagementService SubjectsManagementService
        {
            get
            {
                return ApplicationService<ISubjectManagementService>();
            }
        }
    }
}
