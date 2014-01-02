using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Application.Core.UI.Controllers;
using Application.Core.UI.HtmlHelpers;
using Application.Infrastructure.ProjectManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;
using LMPlatform.UI.ViewModels.BTSViewModels;
using LMPlatform.UI.ViewModels.SubjectViewModels;
using Mvc.JQuery.Datatables;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
    [Authorize(Roles = "student, lector")]
    public class SubjectController : BasicController 
    {
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
        public ActionResult GetModuleData(int subjecyId, int moduleId)
        {
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
                Name = subject.Name,
                ShortName = subject.ShortName,
                Action = PartialViewToString("_SubjectActionList", new SubjectViewModel { SubjectId = subject.Id })
            };
        }
    }
}