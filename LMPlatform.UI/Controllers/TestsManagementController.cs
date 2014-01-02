using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Application.Core.UI.Controllers;
using Application.Infrastructure.KnowledgeTestsManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.UI.ViewModels.KnowledgeTestingViewModels;
using Mvc.JQuery.Datatables;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
    [Authorize(Roles = "lector")]
    public class TestsManagementController : BasicController
    {
        [Authorize, HttpGet]
        public ActionResult Index(int subjectId)
        {
            var subject = SubjectsManagementService.GetSubject(subjectId);
            
            ViewBag.SubjectName = subjectId;
            ViewBag.SubjectTitle = subject.Name;

            return View();
        }

        [HttpGet]
        public JsonResult GetTest(int id)
        {
            var test = id == 0 
                ? new TestViewModel()
                : TestViewModel.FromTest(TestsManagementService.GetTest(id));

            return Json(test, JsonRequestBehavior.AllowGet);
        }        
        
        //[HttpPost]
        public DataTablesResult<TestItemListViewModel> GetTestsList(DataTablesParam dataTableParam)
        {
            var testViewModels = TestsManagementService.GetAllTests()
                .Select(TestItemListViewModel.FromTest)
                .AsQueryable();

            dataTableParam.sSearch = string.Empty;

            return DataTablesResult.Create(testViewModels, dataTableParam);
        }

        [HttpPost]
        public JsonResult SaveTest(TestViewModel testViewModel)
        {
            var savedTeat = TestsManagementService.SaveTest(testViewModel.ToTest());
            return Json(savedTeat);
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
