using System.Linq;
using System.Web.Mvc;
using Application.Core;
using Application.Core.UI.Controllers;
using Application.Infrastructure.KnowledgeTestsManagement;
using LMPlatform.UI.ViewModels.KnowledgeTestingViewModels;
using Mvc.JQuery.Datatables;

namespace LMPlatform.UI.Controllers
{
    [Authorize(Roles = "student, lector")]
    public class KnowledgeTestingController : BasicController
    {
        [Authorize, HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize, HttpGet]
        public ActionResult Tests()
        {
            return View();
        }

        [Authorize, HttpGet]
        public ActionResult TestResults()
        {
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
        
        [HttpPost]
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

        #region Dependencies

        public ITestsManagementService TestsManagementService
        {
            get
            {
                return ApplicationService<ITestsManagementService>();
            }
        }
        #endregion
    }
}
