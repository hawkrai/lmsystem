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
    public class TestPassingController : BasicController
    {
        [Authorize, HttpGet]
        public ActionResult GetAvailableTests(int subjectId)
        {
            var availableTests = TestPassingService.GetAvailableTestsForStudent(CurrentUserId, subjectId);
            return View(availableTests);
        }

        [Authorize, HttpGet]
        public ActionResult GetTestResultsForGroup(int groupId)
        {            
            return PartialView("TestResultsTable", groupId);
        }

        public DataTablesResult<TestResultItemListViewModel> GetTestsTesults(DataTablesParam dataTableParam, int groupId)
        {
            var searchString = dataTableParam.GetSearchString();
            var students = TestPassingService.GetPassTestResults(groupId, searchString);

            return DataTableExtensions.GetResults(students.Select(student => TestResultItemListViewModel.FromStudent(student, new HtmlString(PartialViewToString("_TestsResultsGridColumn", student)))), dataTableParam, students.Count());
        }

        [Authorize, HttpGet]
        public ActionResult TestResults(int subjectId)
        {
            Subject subject = SubjectsManagementService.GetSubject(subjectId);
            int[] groupIds = subject.SubjectGroups.Select(subjectGroup => subjectGroup.GroupId).ToArray();
            ViewBag.Groups = GroupManagementService.GetGroups(new Query<Group>(group => groupIds.Contains(group.Id))).ToList();

            //var students = TestPassingService.GetPassTestResults(1);
            return View(subjectId);
        }

        [HttpGet]
        public PartialViewResult GetNextQuestion(int testId, int questionNumber)
        {
            NextQuestionResult nextQuestion = TestPassingService.GetNextQuestion(testId, CurrentUserId, questionNumber);
            if (nextQuestion.Question == null)
            {
                return PartialView("EndTest", nextQuestion.QuestionsStatuses);
            }

            return PartialView("GetNextQuestion", nextQuestion);
        }

        [HttpPost]
        public JsonResult AnswerQuestionAndGetNext(IEnumerable<AnswerViewModel> answers, int testId, int questionNumber)
        {
            TestPassingService.MakeUserAnswer(answers.Select(answerModel => answerModel.ToAnswer()), CurrentUserId, testId, questionNumber);
            return Json("Ok");
        }

        [HttpGet]
        public ActionResult StartTest(int testId)
        {
            Test test = TestsManagementService.GetTest(testId);
            return View(test);
        }

        [Authorize, HttpGet]
        public ActionResult TestsForPassing(int subjectId)
        {
            ViewBag.SubjectId = subjectId;
            IEnumerable<Test> tests = TestPassingService.GetTestsForSubject(subjectId);
            return View(tests);
        }

        [Authorize, HttpGet]
        public ActionResult RealTimePassingForTest(int testId)
        {
            ViewBag.SubjectId = TestsManagementService.GetTest(testId).SubjectId;
            IEnumerable<RealTimePassingResult> passingResults = TestPassingService.GetRealTimePassingResults(testId);
            return View(passingResults);
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
