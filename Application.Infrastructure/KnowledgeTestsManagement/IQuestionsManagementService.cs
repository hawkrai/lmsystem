using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;
using LMPlatform.Models.KnowledgeTesting;

namespace Application.Infrastructure.KnowledgeTestsManagement
{
    public interface IQuestionsManagementService
    {
        Question GetQuestion(int id);

        IList<Question> GetQuestionsByConceptId(int conceptId);

        Question SaveQuestion(Question question);

        void DeleteQuestion(int id);

        IPageableList<Question> GetPageableQuestions(int testId, string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null);

        void CopyQuestionsToTest(int testId, int[] questionsIds);

        IEnumerable<Question> GetQuestionsForTest(int testId, string searchString = null);

        void ChangeQuestionNumber(int questionId, int number);

        void ChangeTestNumber(int testId, int number);
    }
}
