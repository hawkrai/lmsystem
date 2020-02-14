using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Application.Core.Data;
using Application.Core.UI.Controllers;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.KnowledgeTestsManagement;
using Application.Infrastructure.StudentManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;
using LMPlatform.Models.KnowledgeTesting;
using LMPlatform.UI.ViewModels.KnowledgeTestingViewModels;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
    using System.Configuration;
    using System.IO;
    using System.Web;
    using System.Web.Http;
    using Application.Infrastructure.ConceptManagement;
    using LMPlatform.UI.Services.Modules.Concept;
    using LMPlatform.UI.ViewModels.SubjectViewModels;

    [Authorize]
    public class TestsController : BasicController
    {
        public string TestContentPath
        {
            get { return ConfigurationManager.AppSettings["TestContentPath"]; }
        }

        [Authorize(Roles = "lector"), HttpGet]
        public ActionResult KnowledgeTesting(int subjectId)
        {
            if (!User.IsInRole("lector"))
            {
                return PartialView("Error");
            }

            var subject = SubjectsManagementService.GetSubject(subjectId);
            return View("KnowledgeTesting", subject);
        }

        [HttpPost]
        public JsonResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                byte[] fileContent = null;

                using (var memoryStream = new MemoryStream())
                {
                    file.InputStream.CopyTo(memoryStream);
                    fileContent = memoryStream.ToArray();
                }
                var fileName = TestContentPath + Guid.NewGuid().ToString("N") + System.IO.Path.GetExtension(file.FileName);

                System.IO.File.WriteAllBytes(fileName, fileContent);

                return null;
            }
            catch (Exception e)
            {
                return Json(new { ErrorMessage = e.Message });
            }
        }

        [HttpGet]
        public JsonResult GetFiles()
        {
            try
            {
                var url = Request.Url.Authority;

                var dirs = Directory.GetFiles(TestContentPath);

                return Json(dirs.Select(e => new
                                                 {
                                                     Url = "http://" + url + "/UploadedTestFiles/" + System.IO.Path.GetFileName(e)
                                                 }).ToList(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { ErrorMessage = e.Message });
            }
        }

        public JsonResult GetTests(int? subjectId)
        {
            var tests = TestsManagementService.GetTestsForSubject(subjectId);
            var testViewModels = tests.Select(TestItemListViewModel.FromTest).OrderBy(x => x.TestNumber);

            return Json(testViewModels, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRecomendations(int subjectId)
        {
            var result = new List<object>();
            var predTest = TestsManagementService.GetTestsForSubject(subjectId).FirstOrDefault(x => x.BeforeEUMK);
            if (predTest != null)
            {
                var predTestResult = TestPassingService.GetStidentResults(subjectId, CurrentUserId).FirstOrDefault(x => x.TestId == predTest.Id);
                if (predTestResult == null || predTestResult.Points == null)
                {
                    return Json(new object[]
                    {
                        new { IsTest = true, Id = predTest.Id, Text = "Пройдите предтест" }
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            foreach(var recommendedConcept in GetMaterialsRecomendations(predTest.Id, 0))
            {
                if (recommendedConcept != null && recommendedConcept.Concept != null)
                {
                    var testIds = GetTestForEUMKConcept(recommendedConcept.Concept.Id, subjectId, 0);
					if (testIds != null && testIds.Any())
					{
						result.Add(new { IsTest = false, Id = recommendedConcept.Concept.Id, Text = "Рекомендуемый для прочтения материал" });
						if (testIds != null && testIds.Count() > 0)
						{
							foreach (var testId in testIds)
							{
								result.Add(new { IsTest = true, Id = testId, Text = "Пройдите тест!" });
							}
							return Json(result, JsonRequestBehavior.AllowGet);
						}
					}					
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

		[AllowAnonymous]
		public JsonResult GetRecomendationsMobile(int subjectId, int userId)
		{
			var result = new List<object>();
			var predTest = TestsManagementService.GetTestsForSubject(subjectId).FirstOrDefault(x => x.BeforeEUMK);
			if (predTest != null)
			{
				var predTestResult = TestPassingService.GetStidentResults(subjectId, userId).FirstOrDefault(x => x.TestId == predTest.Id);
				if (predTestResult == null || predTestResult.Points == null)
				{
					return Json(new object[]
					{
						new { IsTest = true, Id = predTest.Id, Text = "Пройдите предтест" }
					}, JsonRequestBehavior.AllowGet);
				}
			}
			foreach (var recommendedConcept in GetMaterialsRecomendations(predTest.Id, userId))
			{
				if (recommendedConcept != null && recommendedConcept.Concept != null)
				{
					var testIds = GetTestForEUMKConcept(recommendedConcept.Concept.Id, subjectId, userId);
					if (testIds != null && testIds.Count() > 0)
					{
						result.Add(new { IsTest = false, Id = recommendedConcept.Concept.Id, Text = "Рекомендуемый для прочтения материал" });
						foreach (var testId in testIds)
						{
							result.Add(new { IsTest = true, Id = testId, Text = "Пройдите тест!" });
						}
						return Json(result, JsonRequestBehavior.AllowGet);
					}
				}
			}

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		private IEnumerable<int> GetTestForEUMKConcept(int conceptId, int subjectId, int userId)
        {
            var testIds = QuestionsManagementService.GetQuestionsByConceptId(conceptId).Select(x => x.TestId).Distinct();

            foreach(var testId in testIds)
            {
                var test = TestsManagementService.GetTest(testId);
                if(test.ForEUMK)
                {
                    var testResult = TestPassingService.GetStidentResults(subjectId, userId == 0 ? CurrentUserId : userId).FirstOrDefault(x => x.TestId == test.Id);

                    if (testResult == null)
                    {
                        yield return test.Id;
                    }
                    else if (testResult != null && (testResult.Points == null || testResult.Points < 10))
                    {
                        yield return test.Id;
                    }
                }
            }
        }

        private IList<ConceptResult> GetMaterialsRecomendations(int predTestId, int userId)
        {
            IList<ConceptResult> result = new List<ConceptResult>();
            try
            {
                var test = TestsManagementService.GetTest(predTestId, true);
                if (test.Questions != null)
                {
                    foreach(var question in test.Questions)
                    {
                        if (question.ConceptId.HasValue)
                        {
                            var points = TestPassingService.GetPointsForQuestion(userId == 0 ? CurrentUserId : userId, question.Id);
                            if (points == 0 || points == null)
                            {
                                var concept = ConceptManagementService.GetById(question.ConceptId.Value);
                                result.Add(new ConceptResult
                                {
                                    Concept = new ConceptViewData(concept)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return result;
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
        public JsonResult SaveTest(TestViewModel testViewModel)
        {
            try
            {
                var savedTest = TestsManagementService.SaveTest(testViewModel.ToTest());
                return Json(savedTest);
            }
            catch (Exception e)
            {
                return Json(new { ErrorMessage = e.Message });
            }
        }

        [HttpPatch]
        public JsonResult OrderQuestions([FromBody] Dictionary<string, int> newOrder)
        {
            try
            {
                foreach(var item in newOrder)
                {
                    var questionId = int.Parse(item.Key);
                    QuestionsManagementService.ChangeQuestionNumber(questionId, item.Value);
                }
                return Json("Ok");
            }
            catch (Exception e)
            {
                return Json(new { ErrorMessage = e.Message });
            }
        }

        [HttpPatch]
        public JsonResult OrderTests([FromBody] Dictionary<string, int> newOrder)
        {
            try
            {
                foreach (var item in newOrder)
                {
                    var testId = int.Parse(item.Key);
                    QuestionsManagementService.ChangeTestNumber(testId, item.Value);
                }
                return Json("Ok");
            }
            catch (Exception e)
            {
                return Json(new { ErrorMessage = e.Message });
            }
        }


        [HttpDelete]
        public JsonResult DeleteTest(int id)
        {
            TestsManagementService.DeleteTest(id);
            return Json(id);
        }

        public ActionResult UnlockTests(int[] studentIds, int testId, bool unlock)
        {
            TestsManagementService.UnlockTest(studentIds, testId, unlock);
            return Json("Ok");
        }

        [HttpPost]
        public ActionResult ChangeLockForUserForStudent(int testId, int studentId, bool unlocked)
        {
            TestsManagementService.UnlockTestForStudent(testId, studentId, unlocked);
            if (unlocked)
            {
                TestPassResult passedByUser = TestPassingService.GetTestPassingTime(testId, studentId);
                if (passedByUser != null)
                {
                    Student student = StudentManagementService.GetStudent(studentId);
                    return Json(new
                    {
                        PassedTime = passedByUser.StartTime.ToShortDateString(),
                        Test = TestsManagementService.GetTest(testId).Title,
                        Student = string.Format("{0} {1}", student.FirstName, student.LastName),
                        Points = passedByUser.Points
                    });
                }
            }

            return Json("Ok");
        }

        [HttpGet]
        public JsonResult GetTestForLector()
        {
            IEnumerable<Test> tests = TestsManagementService.GetTestForLector(CurrentUserId);
            var testViewModels = tests.Select(TestViewModel.FromTest).OrderBy(t => t.Title).ToList();
            testViewModels.Add(new TestViewModel()
            {
                Id = 0,
                Title = "Все тесты"
            });

            return Json(testViewModels, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetQuestionsFromAnotherTests(int testId)
        {
            IEnumerable<Question> questions = TestsManagementService.GetQuestionsFromAnotherTests(testId, CurrentUserId);
            var questionViewModels = questions.Select(QuestionViewModel.FromQuestion).ToList();

            return Json(questionViewModels, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGroups(int subjectId)
        {
            Subject subject = SubjectsManagementService.GetSubject(subjectId);
            int[] groupIds = subject.SubjectGroups.Where(x => x.IsActiveOnCurrentGroup).Select(subjectGroup => subjectGroup.GroupId).ToArray();
            var groups = GroupManagementService.GetGroups(new Query<Group>(group => groupIds.Contains(group.Id)))
                .Select(group => new
                {
                    Id = group.Id,
                    Name = group.Name
                }).ToArray();

            return Json(groups, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubGroups(int groupId, int subjectId, int testId)
        {
            IEnumerable<TestUnlockInfo> testUnlocks = TestsManagementService.GetTestUnlocksForTest(groupId, testId);

	        var test = SubjectsManagementService.GetSubGroups(subjectId, groupId);

			var subgroups = test.Select(subGroup => new
            {
                Name = subGroup.Name,
				Students = subGroup.SubjectStudents.Where(e => e.Student.GroupId == groupId && (e.Student.Confirmed == null || e.Student.Confirmed.Value)).Select(student => new
                {
                    Id = student.StudentId,
                    Name = student.Student.FullName,
					Unlocked = testUnlocks.FirstOrDefault(e => e.StudentId == student.StudentId) != null ? testUnlocks.Single(unlock => unlock.StudentId == student.StudentId).Unlocked : false
                }).OrderBy(student => student.Name).ToArray()
            }).ToArray();

            return Json(subgroups, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Subjects(int subjectId)
        {
            List<SubjectViewModel> CourseProjectSubjects;
            var s = SubjectsManagementService.GetUserSubjects(WebSecurity.CurrentUserId).Where(e => !e.IsArchive);
            CourseProjectSubjects = s.Where(cs => ModulesManagementService.GetModules(cs.Id).Any(m => m.ModuleType == ModuleType.SmartTest))
    .Select(e => new SubjectViewModel(e)).ToList();
            return View(CourseProjectSubjects);
        }

        #region Questions

        [HttpGet]
        public JsonResult GetQuestions(int testId)
        {
            var questions = QuestionsManagementService.GetQuestionsForTest(testId).Select(QuestionItemListViewModel.FromQuestion).OrderBy(x => x.QuestionNumber).ToArray();
            return Json(questions, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetQuestion(int id)
        {
            var test = id == 0
                ? new QuestionViewModel { Answers = new[] { new AnswerViewModel { IsCorrect = 0 } }, ComplexityLevel = 1 }
                : QuestionViewModel.FromQuestion(QuestionsManagementService.GetQuestion(id));
            return Json(test, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetConcepts(int subjectId)
        {
            var concepts = ConceptManagementService.GetRootTreeElementsBySubject(subjectId);
            var result = concepts.Select(c => new ConceptViewData(c, true, (Concept concept) =>
            {
                return concept.IsGroup && !ConceptManagementService.IsTestModule(concept.Name);
            }));
            return Json(result, JsonRequestBehavior.AllowGet);
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
                var test = TestsManagementService.GetTest(questionViewModel.TestId, true);
                if (test.ForEUMK)
                {
                    if (questionViewModel.ConceptId == null)
                    {
                        var questions = QuestionsManagementService.GetQuestionsForTest(questionViewModel.TestId).ToArray();
                        if (questions.Length > 0)
                        {
                            questionViewModel.ConceptId = questions.First().ConceptId;
                        }
                    }
                    else
                    {
                        foreach (var questionId in test.Questions.Select(x => x.Id))
                        {
                            var question = QuestionsManagementService.GetQuestion(questionId);
                            question.ConceptId = questionViewModel.ConceptId;
                            QuestionsManagementService.SaveQuestion(question);
                        }
                    }
                }

                var savedQuestion = QuestionsManagementService.SaveQuestion(questionViewModel.ToQuestion());

                return Json(QuestionViewModel.FromQuestion(savedQuestion));
            }
            catch (Exception e)
            {
                return Json(new { ErrorMessage = e.Message });
            }
        }

        [HttpPost]
        public JsonResult AddQuestionsFromAnotherTest(int[] questionItems, int testId)
        {
            try
            {
                QuestionsManagementService.CopyQuestionsToTest(testId, questionItems);

                return Json("Ok");
            }
            catch (Exception e)
            {
                return Json(new { ErrorMessage = e.Message });
            }
        }

        #endregion

        protected int CurrentUserId
        {
            get
            {
                return int.Parse(WebSecurity.CurrentUserId.ToString(CultureInfo.InvariantCulture));
            }
        }

        #region Dependencies

        public IModulesManagementService ModulesManagementService
        {
            get
            {
                return ApplicationService<IModulesManagementService>();
            }
        }

        public ITestsManagementService TestsManagementService
        {
            get
            {
                return ApplicationService<ITestsManagementService>();
            }
        }

        public IQuestionsManagementService QuestionsManagementService
        {
            get
            {
                return ApplicationService<IQuestionsManagementService>();
            }
        }

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

        public IGroupManagementService GroupManagementService
        {
            get
            {
                return ApplicationService<IGroupManagementService>();
            }
        }

        public IStudentManagementService StudentManagementService
        {
            get
            {
                return ApplicationService<StudentManagementService>();
            }
        }

        public IConceptManagementService ConceptManagementService
        {
            get { return ApplicationService<ConceptManagementService>(); }
        }

        #endregion
    }
}
