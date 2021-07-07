using System.Collections.Generic;
using System.IO;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;
using LMPlatform.Models.KnowledgeTesting;

namespace Application.Infrastructure.KnowledgeTestsManagement
{
	public class TestsManagementService : ITestsManagementService
	{
		public void CheckForTestIsNotLocked(int testId)
		{
			if (testId == 0)
			{
				return;
			}

			var testsQuery = new Query<Test>(test => test.Id == testId)
				.Include(t => t.TestUnlocks);

			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var unlocks = repositoriesContainer.TestsRepository.GetBy(testsQuery).TestUnlocks;
				if (unlocks != null && unlocks.Count > 0)
				{
					throw new InvalidDataException("Тест не может быть изменён, т.к. доступен для прохождения");
				}
			}
		}

		public Test GetTest(int id, bool includeQuestions = false)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var query = new Query<Test>(test => test.Id == id);
				if (includeQuestions)
				{
					query.Include(t => t.Questions);
				}

				return repositoriesContainer.TestsRepository.GetBy(query);
			}
		}

		public Test SaveTest(Test test, bool withountValidation = false)
		{
			if (!withountValidation)
			{
				CheckForTestIsNotLocked(test.Id);
				ValidateTest(test);
			}
			
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				repositoriesContainer.TestsRepository.Save(test);
				repositoriesContainer.ApplyChanges();
				return test;
			}
		}

		private void ValidateTest(Test test)
		{
			if (test.CountOfQuestions <= 0)
			{
				throw new InvalidDataException("Количество вопросов должно быть больше нуля");
			}

			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				if (repositoriesContainer.TestsRepository.GetAll(new Query<Test>(t => t.Id != test.Id && t.Title == test.Title && t.SubjectId == test.SubjectId)).Any())
				{
					throw new InvalidDataException("Тест с таким названием уже существует в рамках данного предмета");
				}
			}
		}

		public void DeleteTest(int id)
		{
			CheckForTestIsNotLocked(id);
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				Test testToDelete = repositoriesContainer.TestsRepository.GetBy(
					new Query<Test>(test => test.Id == id));

				repositoriesContainer.TestsRepository.Delete(testToDelete);
				repositoriesContainer.ApplyChanges();
			}
		}

		public IPageableList<Test> GetPageableTests(int subjectId, string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null)
		{
			var query = new PageableQuery<Test>(pageInfo, test => test.SubjectId == subjectId);

			if (!string.IsNullOrEmpty(searchString))
			{
				query.AddFilterClause(test => test.Description.ToLower().Contains(searchString) ||
					test.Title.ToLower().Contains(searchString));
			}

			query.OrderBy(sortCriterias);
			query.Include(test => test.TestUnlocks);
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				return repositoriesContainer.TestsRepository.GetPageableBy(query);
			}
		}

		public IEnumerable<Test> GetTestsForSubject(int? subjectId, bool lite = false)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();

			var query = lite ? new Query<Test>() : new Query<Test>().Include(test => test.TestUnlocks);

			if (subjectId.HasValue)
			{
				query.AddFilterClause(test => test.SubjectId == subjectId.Value);
				if (!lite)
				{
					query.Include(t => t.Questions);
				}
			}

			var searchResults = repositoriesContainer.TestsRepository.GetAll(query).ToList();

			return searchResults;
		}

		public List<Question> GetQuestions()
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var data = repositoriesContainer.QuestionsRepository.GetAll();
				return data.ToList();
			}
		}

		public IEnumerable<TestUnlockInfo> GetTestUnlocksForTest(int groupId, int testId, string searchString = null)
		{
			IEnumerable<Student> studentResults;
			IEnumerable<TestUnlock> testUnlocksResults;
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				studentResults = GetStudents(groupId, searchString, repositoriesContainer).ToList();
				testUnlocksResults = GetTestUnlocks(studentResults.Select(student => student.Id).ToArray(), testId, repositoriesContainer).ToList();
			}

			var results = new List<TestUnlockInfo>();
			foreach (var student in studentResults)
			{
				results.Add(new TestUnlockInfo
				{
					StudentId = student.Id,
					TestId = testId,
					StudentName = student.FullName,
					Unlocked = testUnlocksResults.Any(testUnlock => testUnlock.StudentId == student.Id)
				});
			}

			int counter = 1;
			results = results.OrderBy(unlockInfo => unlockInfo.StudentName).ToList();
			foreach (var testUnlockInfo in results)
			{
				testUnlockInfo.Number = counter++;
			}

			return results;
		}

		public void UnlockTest(int[] studentIds, int testId, bool unlock)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var savedTestUnlocks = repositoriesContainer.TestUnlocksRepository.GetAll(new
					Query<TestUnlock>()
					.AddFilterClause(testUnlock => studentIds.Contains(testUnlock.StudentId) && testUnlock.TestId == testId))
					.ToList();

				repositoriesContainer.TestUnlocksRepository.Delete(savedTestUnlocks);
				if (unlock)
				{
					IEnumerable<TestUnlock> testUnlocks = studentIds.Select(studentId => new TestUnlock
					{
						StudentId = studentId,
						TestId = testId
					});

					repositoriesContainer.TestUnlocksRepository.Save(testUnlocks);
				}

				repositoriesContainer.ApplyChanges();
			}
		}

		public void UnlockTestForStudent(int testId, int studentId, bool unlocked)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var savedTestUnlock = repositoriesContainer.TestUnlocksRepository.GetAll(
					new Query<TestUnlock>().AddFilterClause(
						testUnlock => testUnlock.StudentId == studentId && testUnlock.TestId == testId)).SingleOrDefault();

				if (unlocked)
				{
					if (savedTestUnlock == null)
					{
						repositoriesContainer.TestUnlocksRepository.Save(new TestUnlock { StudentId = studentId, TestId = testId });
					}
				}
				else
				{
					if (savedTestUnlock != null)
					{
						repositoriesContainer.TestUnlocksRepository.Delete(savedTestUnlock);
					}
				}

				repositoriesContainer.ApplyChanges();
			}
		}

		public void UnlockAllTestForGroup(int groupId)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var students = repositoriesContainer.StudentsRepository.GetStudents(groupId);

				foreach (var studentT in students)
				{
					var savedTestsUnlock = repositoriesContainer.TestUnlocksRepository.GetAll(
						new Query<TestUnlock>().AddFilterClause(testUnlock => testUnlock.StudentId == studentT.Id));

					repositoriesContainer.TestUnlocksRepository.Delete(savedTestsUnlock);
				}

				repositoriesContainer.ApplyChanges();
			}
		}

		public IEnumerable<Test> GetTestForLector(int currentUserId)
		{
			IEnumerable<Test> searchResults;
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var query = new Query<Test>();
				query.AddFilterClause(test => test.Subject.SubjectLecturers.Any(sl => sl.LecturerId == currentUserId));

				searchResults = repositoriesContainer.TestsRepository.GetAll(query).ToList();
			}

			return searchResults;
		}

		public IEnumerable<Question> GetQuestionsFromAnotherTests(int testId, int currentUserId)
		{
			IEnumerable<Question> searchResults;
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var query = new Query<Question>();
				if (testId == 0)
				{
					query.AddFilterClause(
						question => question.Test.Subject.SubjectLecturers.Any(sl => sl.LecturerId == currentUserId));
				}
				else
				{
					query.AddFilterClause(
						question => question.TestId == testId);
				}

				searchResults = repositoriesContainer.QuestionsRepository.GetAll(query).ToList();
			}

			return searchResults;
		}

		private static IEnumerable<TestUnlock> GetTestUnlocks(int[] studentIds, int testId, LmPlatformRepositoriesContainer repositoriesContainer)
		{
			IEnumerable<TestUnlock> searchResults;
			searchResults = repositoriesContainer.TestUnlocksRepository.GetAll(new Query<TestUnlock>()
				.AddFilterClause(testUnlock => studentIds.Contains(testUnlock.StudentId))
				.AddFilterClause(testUnlock => testUnlock.TestId == testId)).ToList();
			return searchResults;
		}

		private IEnumerable<Student> GetStudents(int groupId, string searchString, LmPlatformRepositoriesContainer repositoriesContainer)
		{
			var studentsQuery = new Query<Student>();
			studentsQuery.AddFilterClause(student => student.GroupId == groupId);
			if (searchString != null)
			{
				studentsQuery.AddFilterClause(student => student.LastName.Contains(searchString)
					|| student.FirstName.Contains(searchString));
			}

			IQueryable<Student> students = repositoriesContainer.StudentsRepository.GetAll(studentsQuery);

			return students;
		}
	}
}
