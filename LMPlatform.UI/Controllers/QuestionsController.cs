using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Application.Core.Data;
using Application.Core.UI.Controllers;
using Application.Core.UI.HtmlHelpers;
using Application.Infrastructure.KnowledgeTestsManagement;
using LMPlatform.Models;
using LMPlatform.Models.KnowledgeTesting;
using LMPlatform.UI.ViewModels.KnowledgeTestingViewModels;
using Mvc.JQuery.Datatables;

namespace LMPlatform.UI.Controllers
{
    [Authorize(Roles = "lector")]
    public class QuestionsController : BasicController
    {
        #region API

        [HttpGet]
        public JsonResult GetQuestion(int id)
        {
            var test = id == 0
                ? new QuestionViewModel()
                : QuestionViewModel.FromQuestion(QuestionsManagementService.GetQuestion(id));

            return Json(test, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetQuestions(int testId)
        {
            var questions = QuestionsManagementService.GetQuestionsForTest(testId).Select(QuestionItemListViewModel.FromQuestion).ToArray();
            return Json(questions, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult DeleteQuestion(int id)
        {
            QuestionsManagementService.DeleteQuestion(id);
            return Json(id);
        }

        [HttpPost]
        public JsonResult SaveQuestion(QuestionViewModel questionViewModel)
        {
            try
            {
                var savedQuestion = QuestionsManagementService.SaveQuestion(questionViewModel.ToQuestion());
                return Json(QuestionViewModel.FromQuestion(savedQuestion));
            }
            catch (Exception e)
            {
                return Json(new { ErrorMessage = e.Message });
            }
        }

        #endregion

        public ActionResult Index(int testId)
        {
            Test test = TestsManagementService.GetTest(testId);
            List<TestItemListViewModel> testModels = TestsManagementService.GetTestsForSubject(test.SubjectId).Select(TestItemListViewModel.FromTest).ToList();
            testModels.Add(new TestItemListViewModel { Id = 0, Title = "Все тесты" });
            ViewBag.Tests = testModels;
            return View(test);
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
