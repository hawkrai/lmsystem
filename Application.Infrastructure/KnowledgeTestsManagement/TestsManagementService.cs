using System.Collections;
using System.Collections.Generic;
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
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.TestsRepository.Save(test);
                repositoriesContainer.ApplyChanges();
                return test;
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

        public IEnumerable<Test> GetTestForSubject(int? subjectId)
        {
            IEnumerable<Test> searchResults;
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var query = new Query<Test>();
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

            return results;
        }

        public void UnlockTest(int groupId, IEnumerable<TestUnlock> testUnlocks)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var studentIds = repositoriesContainer.StudentsRepository.GetAll(new Query<Student>()
                    .AddFilterClause(student => student.GroupId == groupId))
                    .Select(student => student.Id)
                    .ToArray();

                var savedTestUnlocks = repositoriesContainer.TestUnlocksRepository.GetAll(new
                    Query<TestUnlock>()
                    .AddFilterClause(testUnlock => studentIds.Contains(testUnlock.StudentId)))
                    .ToList();

                repositoriesContainer.TestUnlocksRepository.Delete(savedTestUnlocks);
                repositoriesContainer.TestUnlocksRepository.Save(testUnlocks);
                
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
