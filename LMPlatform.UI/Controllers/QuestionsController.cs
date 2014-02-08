using System.Collections.Generic;
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
            var savedQuestion = QuestionsManagementService.SaveQuestion(questionViewModel.ToQuestion());

            return Json(QuestionViewModel.FromQuestion(savedQuestion));
        }

        #endregion

        public ActionResult Index(int testId)
        {
            Test test = TestsManagementService.GetTest(testId);
            ViewBag.TestName = test.Title;
            List<TestItemListViewModel> testModels = TestsManagementService.GetTestForSubject(test.SubjectId).Select(TestItemListViewModel.FromTest).ToList();
            testModels.Add(new TestItemListViewModel { Id = 0, Title = "Все тесты" });
            ViewBag.Tests = testModels;
            return View(testId);
        }

        public ActionResult GetQuestionsSelector(int testId, string searchString)
        {
            var questions = QuestionsManagementService.GetQuestionsForTest(testId, searchString).ToList();
            var questionModels = questions.Select(QuestionItemListViewModel.FromQuestion);
 
            return PartialView("_QuestionsSelectorResult", questionModels);
        }

        public DataTablesResult<QuestionItemListViewModel> GetQuestionsList(DataTablesParam dataTableParam, int testId)
        {
            var searchString = dataTableParam.GetSearchString();
            IPageableList<Question> questionsViewModels = QuestionsManagementService.GetPageableQuestions(testId, searchString, dataTableParam.ToPageInfo());

            return DataTableExtensions.GetResults(questionsViewModels.Items.Select(model => QuestionItemListViewModel.FromQuestion(model, PartialViewToString("_QuestionsGridActions", model.Id))), dataTableParam, questionsViewModels.TotalCount);
        }

        [HttpPost]
        public ActionResult AddQuestionsFromAnotherTest(IEnumerable<QuestionItemListViewModel> questionItems, int testId)
        {
            QuestionsManagementService.CopyQuestionsToTest(testId,
                questionItems.Where(questionItem => questionItem.Selected)
                .Select(questionItem => questionItem.Id).ToArray());

            return new ContentResult();
        }

        #region Dependencies

        public IQuestionsManagementService QuestionsManagementService
        {
            get
            {
                return ApplicationService<IQuestionsManagementService>();
            }
        }

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
