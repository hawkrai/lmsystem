using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Core.Data;
using Application.Core.UI.Controllers;
using Application.Core.UI.HtmlHelpers;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.KnowledgeTestsManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;
using LMPlatform.Models.KnowledgeTesting;
using LMPlatform.UI.ViewModels.KnowledgeTestingViewModels;
using Mvc.JQuery.Datatables;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
    [Authorize(Roles = "lector")]
    public class TestsController : BasicController
    {
        #region API

        [HttpGet]
        public JsonResult GetTest(int id)
        {
            var test = id == 0
                ? new TestViewModel()
                : TestViewModel.FromTest(TestsManagementService.GetTest(id));

            return Json(test, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult DeleteTest(int id)
        {
            TestsManagementService.DeleteTest(id);
            return Json(id);
        }

        [HttpPost]
        public JsonResult SaveTest(TestViewModel testViewModel)
        {
            var savedTest = TestsManagementService.SaveTest(testViewModel.ToTest());
            return Json(savedTest);
        }

        #endregion

        [Authorize, HttpGet]
        public ActionResult Index(int subjectId)
        {
            Subject subject = SubjectsManagementService.GetSubject(subjectId);
            int[] groupIds = subject.SubjectGroups.Select(subjectGroup => subjectGroup.GroupId).ToArray();
            ViewBag.Groups = GroupManagementService.GetGroups(new Query<Group>(group => groupIds.Contains(group.Id)));
            return View(subject);
        }

        [Authorize, HttpGet]
        public ActionResult TestResults(int subjectId)
        {
            var students = TestPassingService.GetPassTestResults(1);
            return View(subjectId);
        }

        public DataTablesResult<TestResultItemListViewModel> GetTestsTesults(DataTablesParam dataTableParam, int groupId)
        {
            var searchString = dataTableParam.GetSearchString();
            var students = TestPassingService.GetPassTestResults(groupId, searchString);

            return DataTableExtensions.GetResults(students.Select(student => TestResultItemListViewModel.FromStudent(student, new HtmlString(PartialViewToString("_TestsResultsGridColumn", student)))), dataTableParam, students.Count());
        }

        public DataTablesResult<TestItemListViewModel> GetTestsList(DataTablesParam dataTableParam, int subjectId)
        {
            var searchString = dataTableParam.GetSearchString();
            var testViewModels = TestsManagementService.GetPageableTests(subjectId, searchString, dataTableParam.ToPageInfo());

            return DataTableExtensions.GetResults(testViewModels.Items.Select(model => TestItemListViewModel.FromTest(model, PartialViewToString("_TestsGridActions", TestItemListViewModel.FromTest(model)))), dataTableParam, testViewModels.TotalCount);
        }

        public ActionResult GetUnlockResults(int? groupId, string searchString, int testId)
        {
            if (!groupId.HasValue)
            {
                return new ContentResult { Content = Messages.SubjectHasNoGroups };
            }

            ViewBag.GroupId = groupId.Value;
            IEnumerable<TestUnlockInfo> unlockInfos = TestsManagementService.GetTestUnlocksForTest(groupId.Value, testId, searchString);
            return PartialView("_TestUnlocksSelectorResult", unlockInfos);
        }

        public ActionResult UnlockTests(int[] studentIds, int testId, bool unlock)
        {
            TestsManagementService.UnlockTest(studentIds, testId, unlock);
            return Json("Ok");
        }

        [HttpPost]
        public ActionResult ChangeLockForUserForStudent(int testId, int studentId, bool unlocked)
        {
            TestsManagementService.UnlockTestForStudent(testId, studentId, unlocked);
            return Json("Ok");
        }

        protected int CurrentUserId
        {
            get
            {
                return int.Parse(WebSecurity.CurrentUserId.ToString(CultureInfo.InvariantCulture));
            }
        }

        #region Dependencies

        public ITestsManagementService TestsManagementService
        {
            get
            {
                return ApplicationService<ITestsManagementService>();
            }
        }

        public ITestPassingService TestPassingService
        {
            get
            {
                return ApplicationService<ITestPassingService>();
            }
        }

        public ISubjectManagementService SubjectsManagementService
        {
            get
            {
                return ApplicationService<ISubjectManagementService>();
            }
        }

        public IGroupManagementService GroupManagementService
        {
            get
            {
                return ApplicationService<IGroupManagementService>();
            }
        }

        #endregion
    }
}
