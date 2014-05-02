using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Application.Core.UI.Controllers;
using Application.Core.UI.HtmlHelpers;
using Application.Infrastructure.ProjectManagement;
using Application.Infrastructure.SubjectManagement;
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
            return this.PartialView("Subjects/Modules/News/_NewsModule");
        }

        public ActionResult Lectures()
        {
            return this.PartialView("Subjects/Modules/Lectures/_LecturesModule");
        }

        public ActionResult Labs()
        {
            return this.PartialView("Subjects/Modules/Labs/_LabsModule");
        }

        public ActionResult Index(int subjectId)
        {
            var model = new SubjectWorkingViewModel(subjectId);
            return View(model);
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

        //public ActionResult CreateNews(int subjectid)
        //{
        //    var model = new NewsDataViewModel(0, subjectid);

        //    return PartialView("Subjects/Modules/News/_EditNews", model);
        //}

        //public ActionResult EditNews(int id, int subjectId)
        //{
        //    var model = new NewsDataViewModel(id, subjectId);

        //    return PartialView("Subjects/Modules/News/_EditNews", model);
        //}

        //[HttpPost, ValidateInput(false)]
        //public ActionResult SaveNews(NewsDataViewModel model)
        //{
        //    model.Save();
        //    var modelData = new ModulesDataWorkingViewModel(model.SubjectId, (int)ModuleType.News);
        //    return PartialView("Subjects/_ModuleTemplate", modelData);
        //}

        //[HttpPost]
        //public ActionResult DeleteNews(int id, int subjectId)
        //{
        //    var model = new NewsDataViewModel(id, subjectId);
        //    model.Delete();
        //    var modelData = new ModulesDataWorkingViewModel(subjectId, (int)ModuleType.News);
        //    return PartialView("Subjects/_ModuleTemplate", modelData);
        //}
        public ActionResult Subjects()
        {
            var model = new SubjectManagementViewModel(WebSecurity.CurrentUserId.ToString(CultureInfo.InvariantCulture));
            var subjects = model.Subjects;
            return View(subjects);
        }

        public ActionResult SubjectsForTests()
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

        public ActionResult CreateLectures(int subjectId)
        {
            var model = new LecturesDataViewModel(0, subjectId);

            return PartialView("Subjects/Modules/Lectures/_EditLectures", model);
        }

        public ActionResult EditLectures(int id, int subjectId)
        {
            var model = new LecturesDataViewModel(id, subjectId);

            return PartialView("Subjects/Modules/Lectures/_EditLectures", model);
        }

        [HttpPost]
        public ActionResult DeleteLectures(int id, int subjectId)
        {
            var model = new LecturesDataViewModel(id, subjectId);
            model.Delete();
            var modelData = new ModulesDataWorkingViewModel(model.SubjectId, (int)ModuleType.Lectures);
            return PartialView("Subjects/_ModuleTemplate", modelData);
        }

        //public ActionResult CreateLabs(int subjectId)
        //{
        //    var model = new LabsDataViewModel(0, subjectId);

        //    return PartialView("Subjects/Modules/Labs/_EditLabs", model);
        //}

        //public ActionResult EditLabs(int id, int subjectId)
        //{
        //    var model = new LabsDataViewModel(id, subjectId);

        //    return PartialView("Subjects/Modules/Labs/_EditLabs", model);
        //}
        public ActionResult CreatePractical(int subjectId)
        {
            var model = new PracticalsDataViewModel(0, subjectId);

            return PartialView("Subjects/Modules/Practicals/_EditPractical", model);
        }

        public ActionResult EditPractical(int id, int subjectId)
        {
            var model = new PracticalsDataViewModel(id, subjectId);

            return PartialView("Subjects/Modules/Practicals/_EditPractical", model);
        }

        //[HttpPost]
        //public ActionResult DeleteLabs(int id, int subjectId)
        //{
        //    var model = new LabsDataViewModel(id, subjectId);
        //    model.Delete();
        //    var modelData = new ModulesDataWorkingViewModel(model.SubjectId, (int)ModuleType.Labs);
        //    return PartialView("Subjects/_ModuleTemplate", modelData);
        //}
        [HttpPost]
        public ActionResult DeletePractical(int id, int subjectId)
        {
            var model = new PracticalsDataViewModel(id, subjectId);
            model.Delete();
            var modelData = new ModulesDataWorkingViewModel(model.SubjectId, (int)ModuleType.Practical);
            return PartialView("Subjects/_ModuleTemplate", modelData);
        }

        //[HttpPost, ValidateInput(false)]
        //public ActionResult SaveLabs(LabsDataViewModel model, string attachments)
        //{
        //    model.Save(attachments);
        //    var modelData = new ModulesDataWorkingViewModel(model.SubjectId, (int)ModuleType.Labs);
        //    return PartialView("Subjects/_ModuleTemplate", modelData);
        //}
        [HttpPost, ValidateInput(false)]
        public ActionResult SavePractical(PracticalsDataViewModel model, string attachments)
        {
            model.Save(attachments);
            var modelData = new ModulesDataWorkingViewModel(model.SubjectId, (int)ModuleType.Practical);
            return PartialView("Subjects/_ModuleTemplate", modelData);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SaveLectures(LecturesDataViewModel model, string attachments)
        {
            model.Save(attachments);
            var modelData = new ModulesDataWorkingViewModel(model.SubjectId, (int)ModuleType.Lectures);
            return PartialView("Subjects/_ModuleTemplate", modelData);
        }
    }
}