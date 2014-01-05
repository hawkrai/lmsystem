using System.Linq;
using System.Web.Mvc;
using Application.Core.Data;
using Application.Core.UI.Controllers;
using Application.Core.UI.HtmlHelpers;
using Application.Infrastructure.KnowledgeTestsManagement;
using LMPlatform.Models.KnowledgeTesting;
using LMPlatform.UI.ViewModels.KnowledgeTestingViewModels;
using Mvc.JQuery.Datatables;

namespace LMPlatform.UI.Controllers
{
    [Authorize(Roles = "lector")]
    public class QuestionsController : BasicController
    {
        #region API

        [HttpDelete]
        public JsonResult DeleteQuestion(int id)
        {
            QuestionsManagementService.DeleteQuestion(id);
            return Json(id);
        }

        [HttpPost]
        public JsonResult SaveQuestion(QuestionViewModel questionViewModel)
        {
            questionViewModel.TestId = 1;
            var savedQuestion = QuestionsManagementService.SaveQuestion(questionViewModel.ToQuestion());

            return Json(QuestionViewModel.FromQuestion(savedQuestion));
        }

        #endregion

        public ActionResult Index(int testId)
        {
            ViewBag.TestName = "Убрать тест";
            return View();
        }

        public DataTablesResult<QuestionItemListViewModel> GetQuestionsList(DataTablesParam dataTableParam)
        {
            var searchString = dataTableParam.GetSearchString();
            IPageableList<Question> questionsViewModels = QuestionsManagementService.GetPageableQuestions(1, searchString, dataTableParam.ToPageInfo());

            return DataTableExtensions.GetResults(questionsViewModels.Items.Select(model => QuestionItemListViewModel.FromQuestion(model, PartialViewToString("_QuestionsGridActions", model.Id))), dataTableParam, questionsViewModels.TotalCount);
        }

        #region Dependencies

        public IQuestionsManagementService QuestionsManagementService
        {
            get
            {
                return ApplicationService<IQuestionsManagementService>();
            }
        }

        #endregion
    }
}
