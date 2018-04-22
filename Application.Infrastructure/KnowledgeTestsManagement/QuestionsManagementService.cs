using System.Collections.Generic;
using System.IO;
using System.Linq;
using Application.Core.Data;
using Application.Core.Extensions;
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
                    .Include(question => question.Answers).Include(q=>q.ConceptQuestions.Select(cq=>cq.Concept)));
            }
        }

        public void CheckForTestIsNotLocked(int testId)
        {
            var testsQuery = new Query<Test>(test => test.Id == testId)
                .Include(t => t.TestUnlocks);

            var answersQuery = new Query<AnswerOnTestQuestion>(a => a.TestId == testId);

            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                if (!repositoriesContainer.TestsRepository.GetBy(testsQuery).ForSelfStudy &&
                    (repositoriesContainer.TestsRepository.GetBy(testsQuery).TestUnlocks.Count > 0 ||
                    repositoriesContainer.RepositoryFor<AnswerOnTestQuestion>().GetAll(answersQuery).Count() != 0))
                {
                    throw new InvalidDataException("Тест не может быть изменён, т.к. доступен для прохождения");
                }
            }
        }

        public void ChangeQuestionNumber(int questionId, int number)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var question = repositoriesContainer.QuestionsRepository.GetAll(new Query<Question>(x => x.Id == questionId)).FirstOrDefault();
                question.QuestionNumber = number;
                repositoriesContainer.QuestionsRepository.Save(question);
            }
        }

        public void ChangeTestNumber(int testId, int number)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var test = repositoriesContainer.TestsRepository.GetAll(new Query<Test>(x => x.Id == testId)).FirstOrDefault();
                test.TestNumber = number;
                repositoriesContainer.TestsRepository.Save(test);
            }
        }

        public Question SaveQuestion(Question question)
        {
            CheckForTestIsNotLocked(question.TestId);

            ValidateQuestion(question);

            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.QuestionsRepository.Save(question);

                Question existingQuestion = GetQuestion(question.Id);
                var answersToDelete = existingQuestion.Answers.Where(a => question.Answers.All(answer => answer.Id != a.Id));

                // TODO: Resolve problem (items are saved only first time)
                foreach (Answer answer in question.Answers)
                {
                    answer.QuestionId = question.Id;
                }

                repositoriesContainer.RepositoryFor<Answer>().Save(question.Answers);
                repositoriesContainer.RepositoryFor<Answer>().Delete(answersToDelete);

                repositoriesContainer.ApplyChanges();
                return question;
            }
        }

        private void ValidateQuestion(Question question)
        {
            if (string.IsNullOrEmpty(question.Title))
            {
                throw new InvalidDataException("Название вопроса не должно быть пустым");
            }

            if (question.ComlexityLevel <= 0)
            {
                throw new InvalidDataException("Сложность теста должна быть боьше нуля");
            }

            if (question.Answers.Any(answer => string.IsNullOrEmpty(answer.Content)))
            {
                throw new InvalidDataException("Вопрос содержит пустые варианты ответа");
            }

            switch (question.QuestionType)
            {
                case QuestionType.HasOneCorrectAnswer:
                    ValidateOneCorrectVariantQuestion(question);
                    break;
                case QuestionType.HasManyCorrectAnswers:
                    ValidateManyCorrectVariantsQuestion(question);
                    break;
                case QuestionType.TextAnswer:
                    ValidateTextAnswerQuestion(question);
                    break;
                case QuestionType.SequenceAnswer:
                    ValidateSequenceAnswerQuestion(question);
                    break;
            }
        }

        private void ValidateSequenceAnswerQuestion(Question question)
        {
            if (question.Answers.Count < 2)
            {
                throw new InvalidDataException("Последовательность должна состоять хотя бы из 2 элементов");
            }
        }

        private void ValidateTextAnswerQuestion(Question question)
        {
            if (question.Answers.Count < 1)
            {
                throw new InvalidDataException("Вопрос должен иметь хотя бы 1 правильный ответ");
            }
        }

        private void ValidateManyCorrectVariantsQuestion(Question question)
        {
            if (question.Answers.Count < 3)
            {
                throw new InvalidDataException("Вопрос должен иметь хотя бы 3 варианта");
            }

            int correctAnswersCount = question.Answers.Count(answer => answer.СorrectnessIndicator > 0);

            if (correctAnswersCount <= 1)
            {
                throw new InvalidDataException("Вопрос должен иметь хотя бы 2 правильных ответа");
            }
        }

        private void ValidateOneCorrectVariantQuestion(Question question)
        {
            if (question.Answers.Count < 2)
            {
                throw new InvalidDataException("Вопрос должен иметь хотя бы 2 варианта");
            }

            int correctAnswersCount = question.Answers.Count(answer => answer.СorrectnessIndicator > 0);

            if (correctAnswersCount == 0)
            {
                throw new InvalidDataException("Вопрос должен иметь хотя бы один правильный ответ");
            }  
          
            if (correctAnswersCount > 1)
            {
                throw new InvalidDataException("Вопрос должен иметь только один правильный ответ");
            }
        }

        public void DeleteQuestion(int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                Question questionToDelete = repositoriesContainer.QuestionsRepository.GetBy(
                    new Query<Question>(question => question.Id == id));

                CheckForTestIsNotLocked(questionToDelete.TestId);

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
            CheckForTestIsNotLocked(testId);
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

        public IList<Question> GetQuestionsByConceptId(int conceptId)
        {
            IList<Question> searchResults;
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var query = new Query<Question>();
                query.AddFilterClause(question => question.ConceptId == conceptId);

                searchResults = repositoriesContainer.QuestionsRepository.GetAll(query).ToList();
            }

            return searchResults;
        }
    }
}
