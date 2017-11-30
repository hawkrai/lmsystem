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
    using System;
    using System.Text;

    using Application.Core.SLExcel;
    using Application.Core;
    using Application.Infrastructure.TestQuestionPassingManagement;

    public class TestPassingController : BasicController
    {
        private readonly LazyDependency<ITestQuestionPassingService> _testQuestionPassingService = new LazyDependency<ITestQuestionPassingService>();

        public ITestQuestionPassingService TestQuestionPassingService
        {
            get { return _testQuestionPassingService.Value; }
        }

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

        [HttpGet]
        public JsonResult GetTestDescription(int testId)
        {
            Test test = TestsManagementService.GetTest(testId);
            var description = new
            {
                Title = test.Title,
                Description = test.Description
            };

            return Json(description, JsonRequestBehavior.AllowGet);
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
                ViewBag.Percent = nextQuestion.Percent;
                foreach(var item in nextQuestion.QuestionsStatuses)
                {
                    TestQuestionPassingService.SaveTestQuestionPassResults(new TestQuestionPassResults
                    {
                        StudentId = CurrentUserId,
                        TestId = testId,
                        QuestionNumber = item.Key,
                        Result = (int)item.Value
                    });
                }
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
                Points = group.Last().Points,
                Percent = group.Last().Percent
            });

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [Authorize, HttpGet]
        public JsonResult GetResults(int groupId, int subjectId)
        {
            var tests = TestsManagementService.GetTestsForSubject(subjectId);

            TestResultItemListViewModel[] results = TestPassingService.GetPassTestResults(groupId, subjectId).Select(x => TestResultItemListViewModel.FromStudent(x, tests)).OrderBy(res => res.StudentName).ToArray();
            
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

        [Authorize, HttpGet]
        public void GetResultsExcel(int groupId, int subjectId)
        {
            var tests = TestsManagementService.GetTestsForSubject(subjectId);

            TestResultItemListViewModel[] results = TestPassingService.GetPassTestResults(groupId, subjectId).Select(x => TestResultItemListViewModel.FromStudent(x, tests)).OrderBy(res => res.StudentName).ToArray();

            var data = new SLExcelData();

            var rowsData = new List<List<string>>();

            foreach (var result in results)
            {
                var datas = new List<string>();
                datas.Add(result.StudentName);
                datas.AddRange(result.TestPassResults.Select(e => e.Points != null ? string.Format("{0}({1}%)", e.Points, e.Percent) : string.Empty));
                if (result.TestPassResults.Count(e => e.Points != null) > 0)
                {
                    var pointsSum = Math.Round((decimal)result.TestPassResults.Sum(e => e.Points).Value / result.TestPassResults.Count(e => e.Points != null), 0, MidpointRounding.AwayFromZero);
                    var percentSum = Math.Round((decimal)result.TestPassResults.Sum(e => e.Percent).Value / result.TestPassResults.Count(e => e.Percent != null), 0);
                    //datas.Add(pointsSum + " (" + percentSum + "%)");

                    datas.Add(pointsSum.ToString());
                }
                rowsData.Add(datas);
            }

            var index = 0;
            var total = new List<string>()
                                  {
                                      "Средний процен за тест",
                                  };

            foreach (var testResultItemListViewModel in results[0].TestPassResults)
            {
                var count = 0;
                decimal sum = 0;
                decimal sumPoint = 0;
                foreach (var resultItemListViewModel in results)
                {
                    if (resultItemListViewModel.TestPassResults[index].Points != null)
                    {
                        count += 1;
                        sumPoint += resultItemListViewModel.TestPassResults[index].Points.Value;
                    }

                    if (resultItemListViewModel.TestPassResults[index].Percent != null)
                    {
                        sum += resultItemListViewModel.TestPassResults[index].Percent.Value;
                    }

                }
                index += 1;
                //total.Add((int)Math.Round(sumPoint/count, 0, MidpointRounding.AwayFromZero) + " (" + Math.Round(sum / count, 0) + "%)");
                total.Add(Math.Round(sum / count, 0) + "%");
            }

            data.Headers.Add("Студент");
            data.Headers.AddRange(results[0].TestPassResults.Select(e => e.TestName));
            data.DataRows.AddRange(rowsData);
            data.DataRows.Add(total);

            var file = (new SLExcelWriter()).GenerateExcel(data);

            Response.Clear();
            Response.Charset = "ru-ru";
            Response.HeaderEncoding = Encoding.UTF8;
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=TestResult.xlsx");
            Response.BinaryWrite(file);
            Response.Flush();
            Response.End();
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
