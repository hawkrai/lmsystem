using System.Collections.Generic;
using Application.Core;
using Application.Core.Data;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models.KnowledgeTesting;

namespace Application.Infrastructure.KnowledgeTestsManagement
{
    public class TestsManagementService : ITestsManagementService
    {
        public Test GetTest(int id)
        {
            return TestsRepository.GetBy(new Query<Test>(test => test.Id == id));
        }

        public Test SaveTest(Test test)
        {
            TestsRepository.Save(test);
            return test;
        }

        public IEnumerable<Test> GetAllTests()
        {
            return TestsRepository.GetAll();
        }

        #region Dependencies

        private readonly LazyDependency<ITestsRepository> _testsRepository = new LazyDependency<ITestsRepository>();

        public ITestsRepository TestsRepository
        {
            get
            {
                return _testsRepository.Value;
            }
        }

        #endregion
    }
}
