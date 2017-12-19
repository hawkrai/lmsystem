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
    using Application.Infrastructure.ConceptManagement;
    using LMPlatform.UI.Services.Modules.Concept;

    [Authorize]
    public class TestsController : BasicController
    {
        public string TestContentPath
        {
            get { return ConfigurationManager.AppSettings["TestContentPath"]; }
        }

        [Authorize, HttpGet]
        public ActionResult KnowledgeTesting(int subjectId)
        {
            Subject subject = SubjectsManagementService.GetSubject(subjectId);
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
            var testViewModels = tests.Select(TestItemListViewModel.FromTest);

            return Json(testViewModels, JsonRequestBehavior.AllowGet);
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
            int[] groupIds = subject.SubjectGroups.Select(subjectGroup => subjectGroup.GroupId).ToArray();
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
				Students = subGroup.SubjectStudents.Where(e => e.Student.Confirmed == null || e.Student.Confirmed.Value).Select(student => new
                {
                    Id = student.StudentId,
                    Name = student.Student.FullName,
					Unlocked = testUnlocks.FirstOrDefault(e => e.StudentId == student.StudentId) != null ? testUnlocks.Single(unlock => unlock.StudentId == student.StudentId).Unlocked : false
                }).OrderBy(student => student.Name).ToArray()
            }).ToArray();

            return Json(subgroups, JsonRequestBehavior.AllowGet);
        }

        #region Questions

        [HttpGet]
        public JsonResult GetQuestions(int testId)
        {
            var questions = QuestionsManagementService.GetQuestionsForTest(testId).Select(QuestionItemListViewModel.FromQuestion).ToArray();
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
