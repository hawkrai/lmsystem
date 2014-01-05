using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models.KnowledgeTesting;

namespace LMPlatform.Data.Repositories
{
    public class QuestionsRepository : RepositoryBase<LmPlatformModelsContext, Question>, IQuestionsRepository
    {
        public QuestionsRepository(LmPlatformModelsContext dataContext) : base(dataContext)
        {
        }
    }
}
