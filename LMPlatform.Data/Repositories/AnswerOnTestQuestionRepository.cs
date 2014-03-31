using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models.KnowledgeTesting;

namespace LMPlatform.Data.Repositories
{
    public class AnswerOnTestQuestionRepository : RepositoryBase<LmPlatformModelsContext, AnswerOnTestQuestion>, IAnswerOnTestQuestionRepository
    {
        public AnswerOnTestQuestionRepository(LmPlatformModelsContext dataContext) : base(dataContext)
        {
        }
    }
}
