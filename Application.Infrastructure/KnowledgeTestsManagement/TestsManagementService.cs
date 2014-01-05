using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Data.Repositories;
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
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.TestsRepository.GetPageableBy(query);
            }
        }
    }
}
