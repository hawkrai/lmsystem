using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Application.Core.UI.Controllers;
using Application.Core.UI.HtmlHelpers;
using Application.Infrastructure.KnowledgeTestsManagement;
using Application.Infrastructure.SubjectManagement;
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
            var savedTeat = TestsManagementService.SaveTest(testViewModel.ToTest());
            return Json(savedTeat);
        }

        #endregion

        [Authorize, HttpGet]
        public ActionResult Index(int subjectId)
        {
            var subject = SubjectsManagementService.GetSubject(subjectId);

            ViewBag.SubjectId = subjectId;
            ViewBag.SubjectName = subject.Name;

            return View(subjectId);
        }

        public DataTablesResult<TestItemListViewModel> GetTestsList(DataTablesParam dataTableParam, int subjectId)
        {
            var searchString = dataTableParam.GetSearchString();
            var testViewModels = TestsManagementService.GetPageableTests(subjectId, searchString, dataTableParam.ToPageInfo());

            return DataTableExtensions.GetResults(testViewModels.Items.Select(model => TestItemListViewModel.FromTest(model, PartialViewToString("_TestsGridActions", model.Id))), dataTableParam, testViewModels.TotalCount);
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

        public ISubjectManagementService SubjectsManagementService
        {
            get
            {
                return ApplicationService<ISubjectManagementService>();
            }
        }

        #endregion
    }
}
