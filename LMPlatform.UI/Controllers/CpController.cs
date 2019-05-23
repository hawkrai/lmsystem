using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Application.Infrastructure.Export;
using LMPlatform.Data.Infrastructure;
using Application.Infrastructure.CPManagement;
using System.Diagnostics.CodeAnalysis;
using Application.Core;
using LMPlatform.Models;
using System.Collections.Generic;
using Application.Infrastructure.FilesManagement;
using Newtonsoft.Json;
using System;
using WebMatrix.WebData;
using LMPlatform.UI.ViewModels.SubjectViewModels;
using System.Globalization;
using Application.Infrastructure.SubjectManagement;

namespace LMPlatform.UI.Controllers
{
    public class CpController : Controller
    {
        //
        // GET: /YE/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Projects()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult Project()
        {
            return PartialView();
        }

        public ActionResult TaskSheet()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult Students()
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
        public ActionResult UploadProject()
        {
            return PartialView();
        }
        public ActionResult GetCpRepo()
        {
            return PartialView();
        }
        public ActionResult Subjects(int subjectId)
        {
            List<SubjectViewModel> CourseProjectSubjects;
            var s = SubjectManagementService.GetUserSubjects(WebSecurity.CurrentUserId).Where(e => !e.IsArchive);
            CourseProjectSubjects = s.Where(cs => ModulesManagementService.GetModules(cs.Id).Any(m => m.ModuleType == ModuleType.YeManagment))
    .Select(e => new SubjectViewModel(e)).ToList();
            return View(CourseProjectSubjects);
        }

        public void GetTasksSheetDocument(int courseProjectId)
        {
            
            var courseProject =
                new LmPlatformModelsContext().CourseProjects
                .Include(x => x.AssignedCourseProjects.Select(y => y.Student.Group.Secretary.CoursePercentagesGraphs))
                .Single(x => x.CourseProjectId == courseProjectId);

            string docName;
            if (courseProject.AssignedCourseProjects.Count == 1)
            {
                var stud = courseProject.AssignedCourseProjects.Single().Student;
                docName = string.Format("{0}_{1}", stud.LastName, stud.FirstName);
            }
            else
            {
                docName = string.Format("{0}", courseProject.Theme);
            }

             WordCourseProject.CourseProjectToWord(docName, courseProject, Response);
        }

        [System.Web.Http.HttpPost]
        public void DisableNews(string subjectId)
        {
            CpManagementService.DisableNews(int.Parse(subjectId), true);
        }

        [System.Web.Http.HttpPost]
        public void EnableNews(string subjectId)
        {
            CpManagementService.DisableNews(int.Parse(subjectId), false);
        }

        public string GetTasksSheetHtml(int courseProjectId)
        {
            //todo
            var courseProject =
                new LmPlatformModelsContext().CourseProjects
                .Include(x=>x.AssignedCourseProjects.Select(y=>y.Student.Group))
                //.Include(x=>x.Lecturer.CoursePercentagesGraphs)
                //.Include(x => x.AssignedCourseProjects.Select(y => y.Student.Group.Secretary.CoursePercentagesGraphs))
                .Single(x => x.CourseProjectId == courseProjectId);

           return courseProject.AssignedCourseProjects.Count == 1 ?
                WordCourseProject.CourseProjectToDocView(courseProject.AssignedCourseProjects.First()) :
                WordCourseProject.CourseProjectToDocView(courseProject);
        }

        public ActionResult GetFileNews(int id)
        {
            if (id == 0)
            {
                return PartialView("Common/_FilesUploader", new List<Attachment>());
            }

            var model = CpManagementService.GetNews(id);
            return PartialView("Common/_FilesUploader", FilesManagementService.GetAttachments(model.Attachments).ToList());
        }

        [System.Web.Http.HttpPost]
        public System.Web.Mvc.JsonResult SaveNews(string subjectId, string id, string title, string body, string disabled,
           string isOldDate, string pathFile, string attachments)
        {
            var attachmentsModel = JsonConvert.DeserializeObject<List<Attachment>>(attachments).ToList();
            var subject = int.Parse(subjectId);
            try
            {
                CpManagementService.SaveNews(new Models.CourseProjectNews
                {
                    SubjectId = subject,
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

        public ActionResult TaskSheetEdit()
        {
            return PartialView();
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private readonly LazyDependency<ICPManagementService> _cpManagementService = new LazyDependency<ICPManagementService>();

        private ICPManagementService CpManagementService
        {
            get { return _cpManagementService.Value; }
        }

        private readonly LazyDependency<IFilesManagementService> filesManagementService = new LazyDependency<IFilesManagementService>();

        public IFilesManagementService FilesManagementService
        {
            get
            {
                return filesManagementService.Value;
            }
        }

        private readonly LazyDependency<ISubjectManagementService> _subjectManagementService = new LazyDependency<ISubjectManagementService>();


        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return _subjectManagementService.Value;
            }
        }

        private readonly LazyDependency<IModulesManagementService> _modulesManagementService = new LazyDependency<IModulesManagementService>();

        public IModulesManagementService ModulesManagementService
        {
            get
            {
                return _modulesManagementService.Value;
            }
        }
    }
}
