using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Core.Data;
using LMPlatform.Data.Repositories;
using LMPlatform.Data.Repositories.RepositoryContracts;
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
                return repositoriesContainer.TestsRepository.GetBy(new Query<Test>(e => e.Id == id));
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

        public IEnumerable<Test> GetAllTests()
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.TestsRepository.GetAll().ToList();
            }
        }
    }
}
