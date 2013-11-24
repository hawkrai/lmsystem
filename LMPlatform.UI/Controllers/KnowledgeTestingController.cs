using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Application.Core.UI.Controllers;
using Application.Core.UI.HtmlHelpers;
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

        [HttpPost]
        public DataTablesResult<TestItemListViewModel> GetTests(DataTablesParam dataTableParam)
        {
            var testViewModels = new List<TestItemListViewModel>
            {
                new TestItemListViewModel
                {
                    Id = 0,
                    Title = "First"
                },
                new TestItemListViewModel
                {
                    Id = 1,
                    Title = "Second"
                },
                new TestItemListViewModel
                {
                    Id = 2,
                    Title = "Second"
                },
                new TestItemListViewModel
                {
                    Id = 3,
                    Title = "Second"
                },
                new TestItemListViewModel
                {
                    Id = 4,
                    Title = "Second"
                },
                new TestItemListViewModel
                {
                    Id = 5,
                    Title = "Second"
                }
            };

            dataTableParam.sSearch = string.Empty;

            return DataTableExtensions.GetResults(testViewModels, dataTableParam, testViewModels.Count);
        }
    }
}
