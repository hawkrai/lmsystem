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
        public Test GetTest(int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.TestsRepository.GetBy(new Query<Test>(test => test.Id == id));
            }
        }

        public Test SaveTest(Test test)
        {
            ValidateTest(test);
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
        }

        public void DeleteTest(int id)
        {
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

        public IEnumerable<Test> GetTestsForSubject(int? subjectId)
        {
            IEnumerable<Test> searchResults;
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var query = new Query<Test>().Include(test => test.TestUnlocks);

                if (subjectId.HasValue)
                {
                    query.AddFilterClause(test => test.SubjectId == subjectId.Value);
                }

                searchResults = repositoriesContainer.TestsRepository.GetAll(query).ToList();
            }

            return searchResults;
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
                var savedTestUnlock = repositoriesContainer.TestUnlocksRepository.GetAll(new
                    Query<TestUnlock>()
                    .AddFilterClause(testUnlock => testUnlock.StudentId == studentId && testUnlock.TestId == testId))
                    .SingleOrDefault();

                if (unlocked)
                {
                    if (savedTestUnlock == null)
                    {
                        repositoriesContainer.TestUnlocksRepository.Save(new TestUnlock
                        {
                            StudentId = studentId,
                            TestId = testId
                        });
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

        private static IEnumerable<TestUnlock> GetTestUnlocks(int[] studentIds, int testId,  LmPlatformRepositoriesContainer repositoriesContainer)
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
