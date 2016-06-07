using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Application.Infrastructure.Export;
using LMPlatform.Data.Infrastructure;
using Application.Infrastructure.FilesManagement;
using Application.Core;
using System.Diagnostics.CodeAnalysis;
using Application.Infrastructure.DPManagement;
using WebMatrix.WebData;
using System.Collections.Generic;
using LMPlatform.Models;
using Newtonsoft.Json;
using System;

namespace LMPlatform.UI.Controllers
{
    public class DpController : Controller
    {
        // GET: /Dp/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Projects()
        {
            return PartialView();
        }

        public ActionResult Project()
        {
            return PartialView();
        }

        public ActionResult Students()
        {
            return PartialView();
        }

        public ActionResult TaskSheet()
        {
            return PartialView();
        }

        public ActionResult Percentages()
        {
            return PartialView();
        }

        public ActionResult Percentage()
        {
            return PartialView();
        }

        public ActionResult PercentageResults()
        {
            return PartialView();
        }

        public ActionResult VisitStats()
        {
            return PartialView();
        }

        public ActionResult ConsultationDate()
        {
            return PartialView();
        }

        public ActionResult News()
        {
            return PartialView();
        }

        [System.Web.Http.HttpPost]
        public void DisableNews()
        {
            DpManagementService.DisableNews(WebSecurity.CurrentUserId, true);
        }

        [System.Web.Http.HttpPost]
        public void EnableNews()
        {
            DpManagementService.DisableNews(WebSecurity.CurrentUserId, false);
        }


        public ActionResult GetFileNews(int id)
        {
            if (id == 0)
            {
                return PartialView("Common/_FilesUploader", new List<Attachment>());
            }

            var model = DpManagementService.GetNews(id);
            return PartialView("Common/_FilesUploader", FilesManagementService.GetAttachments(model.Attachments).ToList());
        }

        [System.Web.Http.HttpPost]
        public System.Web.Mvc.JsonResult SaveNews(string id, string title, string body, string disabled,
           string isOldDate, string pathFile, string attachments)
        {
            var attachmentsModel = JsonConvert.DeserializeObject<List<Attachment>>(attachments).ToList();
            
            try
            {
                DpManagementService.SaveNews(new Models.DiplomProjectNews
                {
                    LecturerId = WebSecurity.CurrentUserId,
                    Id = int.Parse(id),
                    Attachments = pathFile,
                    Title = title,
                    Body = body,
                    Disabled = bool.Parse(disabled),
                    EditDate = DateTime.Now,
                }, attachmentsModel, WebSecurity.CurrentUserId);
                return new System.Web.Mvc.JsonResult()
                {
                    Data = new
                    {
                        Message = "Новость успешно сохранена",
                        Error = false
                    }
                };
            }
            catch (Exception)
            {
                return new System.Web.Mvc.JsonResult()
                {
                    Data = new
                    {
                        Message = "Произошла ошибка при сохранении новости",
                        Error = true
                    }
                };
            }
        }


        public void GetTasksSheetDocument(int diplomProjectId)
        {
            //todo
            var diplomProject =
                new LmPlatformModelsContext().DiplomProjects
                .Include(x => x.AssignedDiplomProjects.Select(y => y.Student.Group.Secretary.DiplomPercentagesGraphs))
                .Single(x => x.DiplomProjectId == diplomProjectId);

            string docName;
            if (diplomProject.AssignedDiplomProjects.Count == 1)
            {
                var stud = diplomProject.AssignedDiplomProjects.Single().Student;
                docName = string.Format("{0}_{1}", stud.LastName, stud.FirstName);
            }
            else
            {
                docName = string.Format("{0}", diplomProject.Theme);
            }

            Word.DiplomProjectToWord(docName, diplomProject, Response);
        }
        
        public string GetTasksSheetHtml(int diplomProjectId)
        {
            //todo
            var diplomProject =
                new LmPlatformModelsContext().DiplomProjects
                .Include(x => x.AssignedDiplomProjects.Select(y => y.Student.Group.Secretary.DiplomPercentagesGraphs))
                .Single(x => x.DiplomProjectId == diplomProjectId);

            return diplomProject.AssignedDiplomProjects.Count == 1 ? 
                Word.DiplomProjectToDocView(diplomProject.AssignedDiplomProjects.First()) : 
                Word.DiplomProjectToDocView(diplomProject);
        }

        public ActionResult TaskSheetEdit()
        {
            return PartialView();
        }

        private readonly LazyDependency<IFilesManagementService> filesManagementService = new LazyDependency<IFilesManagementService>();

        public IFilesManagementService FilesManagementService
        {
            get
            {
                return filesManagementService.Value;
            }
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private readonly LazyDependency<IDpManagementService> _dpManagementService = new LazyDependency<IDpManagementService>();

        private IDpManagementService DpManagementService
        {
            get { return _dpManagementService.Value; }
        }
    }
}
