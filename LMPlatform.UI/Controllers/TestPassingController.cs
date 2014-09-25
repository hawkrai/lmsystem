using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Application.Core.UI.Controllers;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.KnowledgeTestsManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;
using LMPlatform.Models.KnowledgeTesting;
using LMPlatform.UI.ViewModels.KnowledgeTestingViewModels;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
    public class TestPassingController : BasicController
    {
        [Authorize, HttpGet]
        public ActionResult StudentsTesting(int subjectId)
        {
            Subject subject = SubjectsManagementService.GetSubject(subjectId);
            bool available = TestPassingService.CheckForSubjectAvailableForStudent(CurrentUserId, subjectId);
            if (available)
            {
                return View(subject);
            }
            else
            {
                ViewBag.Message = "Данный предмет не доступен для студента";
                return View("Error");
            }
        }

        [Authorize, HttpGet]
        public JsonResult GetAvailableTests(int subjectId)
        {
            var availableTests = TestPassingService.GetAvailableTestsForStudent(CurrentUserId, subjectId)
                .Select(test => new
                {
                    Id = test.Id,
                    Title = test.Title,
                    Description = test.Description
                });
            
            return Json(availableTests, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult GetNextQuestion(int testId, int questionNumber)
        {
            if (questionNumber == 1 && TestsManagementService.GetTest(testId, true).Questions.Count == 0)
            {
                ViewBag.Message = "Тест не содержит ни одного вопроса";
                return PartialView("Error");
            }

            NextQuestionResult nextQuestion = TestPassingService.GetNextQuestion(testId, CurrentUserId, questionNumber);

            if (nextQuestion.Question == null)
            {
                ViewBag.Mark = nextQuestion.Mark;
                return PartialView("EndTest", nextQuestion.QuestionsStatuses);
            }

            return PartialView("GetNextQuestion", nextQuestion);
        }

        [Authorize, HttpGet]
        public JsonResult GetStudentResults(int subjectId)
        {
            var results = TestPassingService.GetStidentResults(subjectId, CurrentUserId).GroupBy(g => g.TestName).Select(group => new
            {
                Title = group.Key,
                Points = group.Last().Points
            });

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [Authorize, HttpGet]
        public JsonResult GetResults(int groupId, int subjectId)
        {
            TestResultItemListViewModel[] results = TestPassingService.GetPassTestResults(groupId, subjectId).Select(TestResultItemListViewModel.FromStudent).OrderBy(res => res.StudentName).ToArray();
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [Authorize, HttpGet]
        public JsonResult GetControlItems(int subjectId)
        {
            RealTimePassingResult[] passingResults = TestPassingService.GetRealTimePassingResults(subjectId).Where(result => result.PassResults.Count() > 0).ToArray();
            var groupedResults = passingResults.GroupBy(result => result.TestName).ToArray();

            var results = groupedResults.Select(result => new
            {
                Test = result.Key,
                Students = result.ToArray()
            }).ToArray();
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AnswerQuestionAndGetNext(IEnumerable<AnswerViewModel> answers, int testId, int questionNumber)
        {
            TestPassingService.MakeUserAnswer(answers != null && answers.Any() ? answers.Select(answerModel => answerModel.ToAnswer()) : null, CurrentUserId, testId, questionNumber);

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

        public ITestsManagementService TestsManagementService
        {
            get
            {
                return ApplicationService<ITestsManagementService>();
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
