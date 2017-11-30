using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Data;
using LMPlatform.Models.KnowledgeTesting;
using LMPlatform.Data.Infrastructure;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    class TestQuestionPassResultsRepository : RepositoryBase<LmPlatformModelsContext, TestQuestionPassResults>, ITestQuestionPassResultsRepository
    {
        public TestQuestionPassResultsRepository(LmPlatformModelsContext dataContext) : base(dataContext)
        {
        }

        public void Create(TestQuestionPassResults item)
        {
            using (var context = new LmPlatformModelsContext())
            {
                context.TestQuestionPassResults.Add(item);
            }
        }
    }
}
