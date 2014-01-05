using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Data.Repositories;
using LMPlatform.Models.KnowledgeTesting;

namespace Application.Infrastructure.KnowledgeTestsManagement
{
    public class QuestionsManagementService : IQuestionsManagementService
    {
        public Question GetQuestion(int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.QuestionsRepository.GetBy(new Query<Question>(question => question.Id == id));
            }
        }

        public Question SaveQuestion(Question question)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.QuestionsRepository.Save(question);
                repositoriesContainer.ApplyChanges();
                return question;
            }
        }

        public void DeleteQuestion(int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                Question questionToDelete = repositoriesContainer.QuestionsRepository.GetBy(
                    new Query<Question>(question => question.Id == id));

                repositoriesContainer.QuestionsRepository.Delete(questionToDelete);
                repositoriesContainer.ApplyChanges();
            }
        }

        public IPageableList<Question> GetPageableQuestions(int testId, string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null)
        {
            var query = new PageableQuery<Question>(pageInfo, question => question.TestId == testId);

            if (!string.IsNullOrEmpty(searchString))
            {
                query.AddFilterClause(question => question.Description.ToLower().Contains(searchString) ||
                    question.Title.ToLower().Contains(searchString));
            }

            query.OrderBy(sortCriterias);
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.QuestionsRepository.GetPageableBy(query);
            }
        }
    }
}
