using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models.KnowledgeTesting;

namespace Application.Infrastructure.KnowledgeTestsManagement
{
    public interface IQuestionsManagementService
    {
        Question GetQuestion(int id);

        Question SaveQuestion(Question question);

        void DeleteQuestion(int id);

        IPageableList<Question> GetPageableQuestions(int testId, string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null);
    }
}
