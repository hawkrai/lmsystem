using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using Application.Core.UI.Controllers;
using Application.Infrastructure.ConceptManagement;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.KnowledgeTestsManagement;
using Application.Infrastructure.SubjectManagement;
using Bootstrap;
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
		#region Dependencies

		public ITestPassingService TestPassingService => this.ApplicationService<ITestPassingService>();

		public ISubjectManagementService SubjectsManagementService =>
			this.ApplicationService<ISubjectManagementService>();

		public ITestsManagementService TestsManagementService => this.ApplicationService<ITestsManagementService>();

		public IGroupManagementService GroupManagementService => this.ApplicationService<IGroupManagementService>();

		private readonly LazyDependency<ITestQuestionPassingService> _testQuestionPassingService =
			new LazyDependency<ITestQuestionPassingService>();

		public ITestQuestionPassingService TestQuestionPassingService => this._testQuestionPassingService.Value;

		private readonly LazyDependency<ISubjectManagementService> subjectManagementService =
			new LazyDependency<ISubjectManagementService>();

		public ISubjectManagementService SubjectManagementService => this.subjectManagementService.Value;

		public IConceptManagementService ConceptManagementService =>
			this.ApplicationService<ConceptManagementService>();

		#endregion

		[Authorize]
		[HttpGet]
		public ActionResult StudentsTesting(int subjectId)
		{
			var subject = this.SubjectsManagementService.GetSubject(subjectId);
			var available = this.TestPassingService.CheckForSubjectAvailableForStudent(WebSecurity.CurrentUserId, subjectId);
			if (available)
			{
				return this.View(subject);
			}
			else
			{
				this.ViewBag.Message = "Данный предмет не доступен для студента";
				return this.View("Error");
			}
		}

		[HttpGet]
		public JsonResult GetTestDescription(int testId)
		{
			var test = this.TestsManagementService.GetTest(testId);
			var description = new
			{
				test.Title, test.Description
			};

			return JsonResponse(description) as JsonResult;
		}

		[Authorize]
		[HttpGet]
		public JsonResult GetAvailableTests(int subjectId)
		{
			var availableTests = this.TestPassingService.GetAvailableTestsForStudent(WebSecurity.CurrentUserId, subjectId)
				.Select(test => new
				{
					test.Id,
					test.Title,
					test.Description,
					test.ForSelfStudy,
					test.ForNN
				});

			return JsonResponse(availableTests) as JsonResult;
		}

		[HttpGet]
		public JsonResult GetAvailableTestsForMobile(int subjectId, int userId)
		{
			var availableTests = this.TestPassingService.GetAvailableTestsForStudent(userId, subjectId)
				.Select(test => new
				{
					test.Id,
					test.Title,
					test.Description,
					test.ForSelfStudy
				});

			return JsonResponse(availableTests) as JsonResult;
		}

		[HttpGet]
		public JsonResult GetNextQuestionJson(int testId, int questionNumber, int userId, bool excludeCorrectnessIndicator)
		{
			var result = this.TestPassingService.GetNextQuestion(testId, userId, questionNumber);
			Question question = null;
			if (result.Question != null)
			{
				if (excludeCorrectnessIndicator)
				{
					result.Question.Answers.ForEach(a => a.СorrectnessIndicator = default);
				}
				question = result.Question.Clone() as Question;
			}
			return JsonResponse(new
			{
				Question = question,
				result.Number,
				result.Seconds,
				result.SetTimeForAllTest,
				result.ForSelfStudy,
				IncompleteQuestionsNumbers = result.QuestionsStatuses.Where(qs => qs.Value == PassedQuestionResult.NotPassed).Select(qs => qs.Key)
			}) as JsonResult;
		}

		[HttpGet]
		public PartialViewResult GetNextQuestion(int testId, int questionNumber)
		{
			try
			{
				if (questionNumber == 1 && this.TestsManagementService.GetTest(testId, true).Questions.Count == 0)
				{
					this.ViewBag.Message = "Тест не содержит ни одного вопроса";
					return this.PartialView("Error");
				}

				var answers = this.TestPassingService.GetAnswersForTest(testId, WebSecurity.CurrentUserId);

				var nextQuestion = this.TestPassingService.GetNextQuestion(testId, WebSecurity.CurrentUserId, questionNumber);
				var testName = this.TestsManagementService.GetTest(testId, false).Title;
				this.ViewData["testName"] = testName;
				if (nextQuestion.Question == null)
				{
					this.ViewBag.Mark = nextQuestion.Mark;
					this.ViewBag.Percent = nextQuestion.Percent;
					//foreach(var item in nextQuestion.QuestionsStatuses)
					//{
					//    TestQuestionPassingService.SaveTestQuestionPassResults(new TestQuestionPassResults
					//    {
					//        StudentId = CurrentUserId,
					//        TestId = testId,
					//        QuestionNumber = item.Key,
					//        Result = (int)item.Value
					//    });
					//}

					var questions = this.TestsManagementService.GetTest(testId, true);

					var themIds = questions.Questions.OrderBy(e => e.ConceptId).ThenBy(e => e.Id)
						.Where(e => e.ConceptId.HasValue).Select(e => (int) e.ConceptId)
						.Distinct();

					var thems = new List<object>();

					foreach (var themId in themIds.OrderBy(e => e))
					{
						var them = this.ConceptManagementService.GetById(themId);
						thems.Add(new {name = them.Name, id = them.Id});
					}

					var array = new List<int>();

					foreach (var question in questions.Questions.OrderBy(e => e.ConceptId).ThenBy(e => e.Id))
					{
						var answer = answers.FirstOrDefault(e => e.QuestionId == question.Id);
						var data = 0;

						if (answer != null) data = answer.Points > 0 ? 1 : 0;

						array.Add(data);
					}

					dynamic resuls = new ExpandoObject();
					resuls.Answers = array.ToArray();
					resuls.QuestionsStatuses = nextQuestion.QuestionsStatuses;
					resuls.Thems = thems;
					resuls.NeuralData = questions.Data;
					resuls.FoNN = questions.ForNN;

					return PartialView("EndTest", resuls);
				}

				return this.PartialView("GetNextQuestion", nextQuestion);
			}
			catch (Exception ex)
	        {
				ViewBag.Message = ex.Message + ex.StackTrace;
				return PartialView("Error", ex.Message + ex.StackTrace);
	        }
		}

		[Authorize]
		[HttpGet]
		public JsonResult GetStudentResults(int subjectId)
		{
			var results = this.TestPassingService.GetStidentResults(subjectId, WebSecurity.CurrentUserId)
				.GroupBy(g => g.TestName)
				.Select(group => new
				{
					Title = group.Key,
					group.Last().Points,
					group.Last().Percent,
					this.TestsManagementService.GetTest(group.Last().TestId).ForSelfStudy,
					this.TestsManagementService.GetTest(group.Last().TestId).ForNN,
					this.TestsManagementService.GetTest(group.Last().TestId).BeforeEUMK,
					this.TestsManagementService.GetTest(group.Last().TestId).ForEUMK
				});

			return JsonResponse(results) as JsonResult;
		}

		[HttpPost]
		public JsonResult SaveNeuralNetwork(string data, int testId)
		{
			var test = this.TestsManagementService.GetTest(testId);
			test.Data = data;
			this.TestsManagementService.SaveTest(test, true);

			return this.Json("Ok");
		}

		[Authorize]
		[HttpGet]
		public JsonResult GetResults(int groupId, int subjectId)
		{
			var tests = this.TestsManagementService.GetTestsForSubject(subjectId);

			var subGroups = this.SubjectManagementService.GetSubGroupsV2(subjectId, groupId);

			var results = this.TestPassingService.GetPassTestResults(groupId, subjectId)
				.Select(x => TestResultItemListViewModel.FromStudent(x, tests, subGroups))
				.OrderBy(res => res.StudentName).ToArray();

			return JsonResponse(results) as JsonResult;
		}

		[HttpGet]
		public JsonResult GetUserAnswers(int studentId, int testId)
		{
			IList<UserAnswerViewModel> result = new List<UserAnswerViewModel>();

			var userAnswers = this.TestPassingService.GetAnswersForEndedTest(testId, studentId);
			foreach (var answer in userAnswers)
			{
				var test = this.TestsManagementService.GetTest(testId, true);
				var question = test.Questions.First(x => x.Id == answer.QuestionId);
				result.Add(new UserAnswerViewModel
				{
					Points = answer.Points,
					QuestionTitle = question.Title,
					QuestionDescription = question.Description,
					AnswerString = answer.AnswerString,
					Number = answer.Number
				});
			}

			result = result.OrderBy(x => x.Number).ToList();

			return JsonResponse(result) as JsonResult;
		}

		[Authorize]
		[HttpGet]
		public JsonResult GetControlItems(int subjectId)
		{
			var passingResults = this.TestPassingService.GetRealTimePassingResults(subjectId)
				.Where(result => result.PassResults.Any()).ToArray();
			var groupedResults = passingResults.GroupBy(result => result.TestName).ToArray();

			var results = groupedResults.Select(result => new
			{
				Test = result.Key,
				Students = result.ToArray()
			}).ToArray();
			return JsonResponse(results) as JsonResult;
		}

		[HttpPost]
		public JsonResult AnswerQuestionAndGetNext(IEnumerable<AnswerViewModel> answers, int testId, int questionNumber)
		{
			this.TestPassingService.MakeUserAnswer(
				answers != null && answers.Any() ? answers.Select(answerModel => answerModel.ToAnswer()) : null,
				WebSecurity.CurrentUserId, testId, questionNumber);

			return this.Json("Ok");
		}

		[HttpPost]
		public JsonResult AnswerQuestionAndGetNextMobile(IEnumerable<AnswerViewModel> answers, int testId,
			int questionNumber, int userId)
		{
			this.TestPassingService.MakeUserAnswer(
				answers != null && answers.Any() ? answers.Select(answerModel => answerModel.ToAnswer()) : null, userId,
				testId, questionNumber);

			return this.Json("Ok");
		}

		[HttpGet]
		public JsonResult GetQuestionsInfo()
		{
			var questions = this.TestsManagementService.GetQuestions();

			var questionsLevel = questions.ToDictionary(e => e.Id, t => t.ComlexityLevel);

			var answers = this.TestQuestionPassingService.GetAll();

			var level = 0;
			var groups = answers.GroupBy(e => e.QuestionId).Select(e => new
			{
				idQuestion = e.Key,
				complexity = questionsLevel.TryGetValue(e.Key, out level) ? level : 0,
				weight = 1,
				rightAnswers = e.Count(x => x.Points > 0),
				wrongAnswers = e.Count(x => x.Points == 0)
			});


			return JsonResponse(groups) as JsonResult;
		}

		[Authorize]
		[HttpGet]
		public void GetResultsExcel(int groupId, int subjectId, bool forSelfStudy)
		{
			var tests = this.TestsManagementService.GetTestsForSubject(subjectId)
				.Where(x => x.ForSelfStudy == forSelfStudy);

			var subGroups = this.SubjectManagementService.GetSubGroupsV2(subjectId, groupId);

			var results = this.TestPassingService.GetPassTestResults(groupId, subjectId)
				.Select(x => TestResultItemListViewModel.FromStudent(x, tests, subGroups))
				.OrderBy(res => res.StudentName).ToArray();

			var data = new SLExcelData();

			var rowsData = new List<List<string>>();

			foreach (var result in results)
			{
				var datas = new List<string>();
				datas.Add(result.StudentName);
				datas.AddRange(result.TestPassResults.Select(e =>
					e.Points != null ? $"{e.Points}({e.Percent}%)" : string.Empty));
				if (result.TestPassResults.Count(e => e.Points != null) > 0)
				{
					var pointsSum =
						Math.Round(
							(decimal) result.TestPassResults.Sum(e => e.Points).Value /
							result.TestPassResults.Count(e => e.Points != null), 0, MidpointRounding.AwayFromZero);
					//var percentSum = Math.Round((decimal)result.TestPassResults.Sum(e => e.Percent).Value / result.TestPassResults.Count(e => e.Percent != null), 0);
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
						sum += resultItemListViewModel.TestPassResults[index].Percent.Value;
				}

				index += 1;
				//total.Add((int)Math.Round(sumPoint/count, 0, MidpointRounding.AwayFromZero) + " (" + Math.Round(sum / count, 0) + "%)");
				total.Add(Math.Round(sum / count, 0) + "%");
			}

			data.Headers.Add("Студент");
			data.Headers.AddRange(results[0].TestPassResults.Select(e => e.TestName));
			data.DataRows.AddRange(rowsData);
			data.DataRows.Add(total);

			var file = new SLExcelWriter().GenerateExcel(data);

			this.Response.Clear();
			this.Response.Charset = "ru-ru";
			this.Response.HeaderEncoding = Encoding.UTF8;
			this.Response.ContentEncoding = Encoding.UTF8;
			this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			this.Response.AddHeader("Content-Disposition", "attachment; filename=TestResult.xlsx");
			this.Response.BinaryWrite(file);
			this.Response.Flush();
			this.Response.End();
		}
	}
}