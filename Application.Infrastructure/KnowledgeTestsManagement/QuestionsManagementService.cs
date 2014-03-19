using System.Collections.Generic;
using System.Linq;
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
                return repositoriesContainer.QuestionsRepository.GetBy(new Query<Question>(question => question.Id == id)
                    .Include(question => question.Answers));
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

        public void CopyQuestionsToTest(int testId, int[] questionsIds)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var query = new Query<Question>(question => questionsIds.Contains(question.Id));
                query.Include(question => question.Answers);
                var questionsToCopy = repositoriesContainer.QuestionsRepository.GetAll(query);

                var copiedQuestions = new List<Question>();
                foreach (var questionToCopy in questionsToCopy)
                {
                    var copiedQuestion = questionToCopy.Clone() as Question;
                    copiedQuestion.TestId = testId;
                    copiedQuestions.Add(copiedQuestion);
                }
                   
                repositoriesContainer.QuestionsRepository.Save(copiedQuestions);
                repositoriesContainer.ApplyChanges();
            }
        }

        public IEnumerable<Question> GetQuestionsForTest(int testId, string searchString = null)
        {
            IEnumerable<Question> searchResults;
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var query = new Query<Question>();
                if (testId != 0)
                {
                    query.AddFilterClause(question => question.TestId == testId);
                }

                if (searchString != null)
                {
                    query.AddFilterClause(question => question.Title.Contains(searchString));
                }

                searchResults = repositoriesContainer.QuestionsRepository.GetAll(query).ToList();
            }

            return searchResults;
        }
    }
}
