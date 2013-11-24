using System.Collections.Generic;
using LMPlatform.Models.KnowledgeTesting;

namespace Application.Infrastructure.KnowledgeTestsManagement
{
    public interface ITestsManagementService
    {
        Test GetTest(int id);
        
        Test SaveTest(Test test);

        IEnumerable<Test> GetAllTests();
    }
}
