using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models.KnowledgeTesting;

namespace Application.Infrastructure.KnowledgeTestsManagement
{
    public interface ITestsManagementService
    {
        Test GetTest(int id);
        
        Test SaveTest(Test test);

        void DeleteTest(int id);

        IPageableList<Test> GetPageableTests(int subjectId, string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null);
    }
}
