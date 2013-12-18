using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Application.Core.UI.Controllers;
using LMPlatform.UI.ViewModels.KnowledgeTestingViewModels;
using Mvc.JQuery.Datatables;

namespace LMPlatform.UI.Controllers
{
    [Authorize(Roles = "lector")]
    public class QuestionsManagementController : BasicController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public DataTablesResult<QuestionItemListViewModel> GetQuestionsList(DataTablesParam dataTableParam)
        {
            var testViewModels = new List<QuestionItemListViewModel>()
                {
                    new QuestionItemListViewModel(),
                    new QuestionItemListViewModel()
                }
                .AsQueryable();

            dataTableParam.sSearch = string.Empty;

            return DataTablesResult.Create(testViewModels, dataTableParam);
        }
    }
}
