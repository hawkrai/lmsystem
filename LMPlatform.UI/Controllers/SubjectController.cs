using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using Application.Core.UI.Controllers;
using Application.Core.UI.HtmlHelpers;
using Application.Infrastructure;
using Application.Infrastructure.ProjectManagement;
using Application.Infrastructure.SubjectManagement;
using Ionic.Zip;
using LMPlatform.Models;
using LMPlatform.UI.ViewModels.BTSViewModels;
using LMPlatform.UI.ViewModels.SubjectModulesViewModel.ModulesViewModel;
using LMPlatform.UI.ViewModels.SubjectViewModels;
using Mvc.JQuery.Datatables;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
    using System.Collections.Generic;

    using Application.Core;
    using Application.Infrastructure.FilesManagement;

    [Authorize(Roles = "student, lector")]
    public class SubjectController : BasicController 
    {
        private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();
        private readonly LazyDependency<IFilesManagementService> filesManagementService = new LazyDependency<IFilesManagementService>();

        public IFilesManagementService FilesManagementService
        {
            get
            {
                return filesManagementService.Value;
            }
        }

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return subjectManagementService.Value;
            }
        }

        public ActionResult News()
        {
            if (User.IsInRole("student"))
            {
                return this.PartialView("Subjects/Modules/StudentsModule/News/_NewsModule");    
            }

            return this.PartialView("Subjects/Modules/News/_NewsModule");
        }

        public ActionResult Lectures()
        {
            if (User.IsInRole("student"))
            {
                return this.PartialView("Subjects/Modules/StudentsModule/Lectures/_LecturesModule");
            }

            return this.PartialView("Subjects/Modules/Lectures/_LecturesModule");
        }

        public ActionResult Labs()
        {
            if (User.IsInRole("student"))
            {
                return this.PartialView("Subjects/Modules/StudentsModule/Labs/_LabsModule");
            }

            return this.PartialView("Subjects/Modules/Labs/_LabsModule");
        }

        public ActionResult Practicals()
        {
            if (User.IsInRole("student"))
            {
                return this.PartialView("Subjects/Modules/StudentsModule/Practicals/_PracticalModule");
            }

            return this.PartialView("Subjects/Modules/Practicals/_PracticalModule");
        }

        public ActionResult SubjectAttachments()
        {
            return this.PartialView("Subjects/Modules/SubjectAttachments/_SubjectAttachmentsModule");
        }

        public ActionResult GetFileSubject(string subjectId)
        {
            var lectures = new List<Attachment>();
            foreach (var att in SubjectManagementService.GetLecturesAttachments(int.Parse(subjectId)))
            {
                lectures.AddRange(FilesManagementService.GetAttachments(att).ToList());
            }

            var labs = new List<Attachment>();
            foreach (var att in SubjectManagementService.GetLabsAttachments(int.Parse(subjectId)))
            {
                lectures.AddRange(FilesManagementService.GetAttachments(att).ToList());
            }

            var practicals = new List<Attachment>();
            foreach (var att in SubjectManagementService.GetPracticalsAttachments(int.Parse(subjectId)))
            {
                lectures.AddRange(FilesManagementService.GetAttachments(att).ToList());
            }

            return new JsonResult()
                       {
                           Data = new
                                      {
                                          Lectures = this.PartialViewToString("Subjects/Modules/_FilesUploaderNoAdd", lectures),
                                          Labs = this.PartialViewToString("Subjects/Modules/_FilesUploaderNoAdd", labs),
                                          Practicals = this.PartialViewToString("Subjects/Modules/_FilesUploaderNoAdd", practicals)
                                      },
                                      JsonRequestBehavior = JsonRequestBehavior.AllowGet
                       };
        }

        public ActionResult Index(int subjectId)
        {
            if (SubjectManagementService.IsWorkingSubject(WebSecurity.CurrentUserId, subjectId))
            {
                var model = new SubjectWorkingViewModel(subjectId);
                return View(model);    
            }
            else if (User.IsInRole("student"))
            {
                var model = new SubjectWorkingViewModel(subjectId);
                return View(model);       
            }

            return RedirectToAction("Index", "Lms");
        }

        public ActionResult Management()
        {
            return View();
        }

        public ActionResult Create()
        {
            var model = new SubjectEditViewModel(0);

            return PartialView("_CreateSubjectView", model);
        }

        public ActionResult EditSubject(int id)
        {
            var model = new SubjectEditViewModel(id);

            return PartialView("_CreateSubjectView", model);
        }

        public ActionResult DeleteSubject(int id)
        {
            SubjectManagementService.DeleteSubject(id);
            return null;
        }

        [HttpPost]
        public ActionResult SaveSubject(SubjectEditViewModel model)
        {
            model.Save(WebSecurity.CurrentUserId);
            return null;
        }

        public ActionResult GetFileLectures(int id)
        {
            if (id == 0)
            {
                return PartialView("Common/_FilesUploader", new List<Attachment>());    
            }

            var model = SubjectManagementService.GetLectures(id);
            return PartialView("Common/_FilesUploader", FilesManagementService.GetAttachments(model.Attachments).ToList());
        }

        public ActionResult GetFileLabs(int id)
        {
            if (id == 0)
            {
                return PartialView("Common/_FilesUploader", new List<Attachment>());
            }

            var model = SubjectManagementService.GetLabs(id);
            return PartialView("Common/_FilesUploader", FilesManagementService.GetAttachments(model.Attachments).ToList());
        }

		public ActionResult GetUserFilesLab(int id)
		{
			if (id == 0)
			{
				return PartialView("Common/_FilesUploader", new List<Attachment>());
			}

			var model = SubjectManagementService.GetUserLabFile(id);
			return PartialView("Common/_FilesUploader", FilesManagementService.GetAttachments(model.Attachments).ToList());
		}

        public ActionResult GetFilePracticals(int id)
        {
            if (id == 0)
            {
                return PartialView("Common/_FilesUploader", new List<Attachment>());
            }

            var model = SubjectManagementService.GetPractical(id);
            return PartialView("Common/_FilesUploader", FilesManagementService.GetAttachments(model.Attachments).ToList());
        }

		public FileResult GetZipLabs(int id, int subjectId)
	    {
		    var zip = new ZipFile(Encoding.UTF8);

		    var subGroups = SubjectManagementService.GetSubGroup(id);

		    foreach (var subGroup in subGroups.SubjectStudents)
		    {
			    var model = SubjectManagementService.GetUserLabFiles(subGroup.StudentId, subjectId);

			    var attachments = new List<Attachment>();

			    foreach (var data in model)
			    {
					attachments.AddRange(FilesManagementService.GetAttachments(data.Attachments));    
			    }

				UtilZip.CreateZipFile(ConfigurationManager.AppSettings["FileUploadPath"], zip, attachments, subGroup.Student.FullName.Replace(" ", "_"));
		    }

			var memoryStream = new MemoryStream();

			zip.Save(memoryStream);

			memoryStream.Seek(0, SeekOrigin.Begin);

			return new FileStreamResult(memoryStream, "application/zip") { FileDownloadName = subGroups.SubjectGroup.Group.Name + ".zip" };
	    }

        public ActionResult Subjects()
        {
            var model = new SubjectManagementViewModel(WebSecurity.CurrentUserId.ToString(CultureInfo.InvariantCulture));
            var subjects = model.Subjects;
            return View(subjects);
        }

        [HttpPost]
        public ActionResult GetModuleData(int subjectId, int moduleId)
        {
            var model = new ModulesDataWorkingViewModel(subjectId, moduleId);
            return PartialView("Subjects/_ModuleTemplate", model);
        }

        [HttpPost]
        public ActionResult GetModuleDataSubMenu(int subjectId, int moduleId, ModuleType type)
        {
            var model = new ModulesDataWorkingViewModel(subjectId, moduleId, type);
            return PartialView("Subjects/_ModuleTemplate", model);
        }
        
	    public ActionResult SubGroups(int subjectId)
	    {
		    var model = new SubjectWorkingViewModel(subjectId);
			return PartialView("_SubGroupEdit", model.SubGroups);
	    }

        [HttpPost]
        public ActionResult SubGroupsChangeGroup(string subjectId, string groupId)
        {
            var model = new SubjectWorkingViewModel(int.Parse(subjectId));
            
            return PartialView("_SubGroupsEditTemplate", model.SubGroup(int.Parse(groupId)));
        }

        [HttpPost]
        public ActionResult SaveSubGroup(string subjectId, string groupId, string subGroupFirstIds, string subGroupSecondIds)
        {
            var model = new SubGroupEditingViewModel();
            model.SaveSubGroups(int.Parse(subjectId), int.Parse(groupId), subGroupFirstIds, subGroupSecondIds);
            return null;
        }

        [HttpPost]
        public DataTablesResult<SubjectListViewModel> GetSubjects(DataTablesParam dataTableParam)
        {
            var searchString = dataTableParam.GetSearchString();
            var subjects = ApplicationService<ISubjectManagementService>().GetSubjectsLecturer(WebSecurity.CurrentUserId, pageInfo: dataTableParam.ToPageInfo(), searchString: searchString);

            return DataTableExtensions.GetResults(subjects.Items.Select(_GetSubjectRow), dataTableParam, subjects.TotalCount);
        }

        public SubjectListViewModel _GetSubjectRow(Subject subject)
        {
            return new SubjectListViewModel
            {
                Name = string.Format("<a href=\"{0}\">{1}</a>", Url.Action("Index", "Subject", new { subjectId = subject.Id }), subject.Name),
                ShortName = subject.ShortName,
                Action = PartialViewToString("_SubjectActionList", new SubjectViewModel { SubjectId = subject.Id })
            };
        }

	    public ActionResult IsAvailableSubjectName(string name, string id)
	    {
		    return Json(!SubjectManagementService.IsSubjectName(name, id),JsonRequestBehavior.AllowGet);
	    }

		public ActionResult IsAvailableSubjectShortName(string name, string id)
		{
			return Json(!SubjectManagementService.IsSubjectShortName(name, id), JsonRequestBehavior.AllowGet);
		}
    }
}