using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;
using LMPlatform.Models.KnowledgeTesting;

namespace Application.Infrastructure.KnowledgeTestsManagement
{
    public class TestPassingService : ITestPassingService
    {
        public NextQuestionResult GetNextQuestion(int testId, int userId, int nextQuestionNumber)
        {
            GheckForTestUnlockedForUser(testId, userId);

            List<AnswerOnTestQuestion> testAnswers = GetAnswersForTest(testId, userId);

            Tuple<Question, int> nextQuestion = GetQuestion(testAnswers, nextQuestionNumber, userId);
            Dictionary<int, PassedQuestionResult> questionsStatuses = GetQuestionStatuses(testAnswers);

            return new NextQuestionResult
            {
                Question = nextQuestion.Item1,
                Number = nextQuestion.Item2,
                QuestionsStatuses = questionsStatuses
            };  
        }

        public void MakeUserAnswer(IEnumerable<Answer> answers, int userId, int testId, int questionNumber)
        {
            AnswerOnTestQuestion answerOnTestQuestion = GetAnswerOntestQuestion(userId, testId, questionNumber);
            Question question = GetQuestionById(answerOnTestQuestion.QuestionId);

            switch (question.QuestionType)
            {
                case QuestionType.HasOneCorrectAnswer:
                    ProcessOneVariantAnswer(answers, question, answerOnTestQuestion);
                    break;
                case QuestionType.HasManyCorrectAnswers:
                    ProcessManyVariantsAnswer(answers, question, answerOnTestQuestion);
                    break;
                case QuestionType.TextAnswer:
                    ProcessTextAnswer(answers, question, answerOnTestQuestion);
                    break;
                case QuestionType.SequenceAnswer:
                    ProcessSequenceAnswer(answers.ToList(), question, answerOnTestQuestion);
                    break;
            }

            answerOnTestQuestion.Time = DateTime.UtcNow;
            SaveAnswerOnTestQuestion(answerOnTestQuestion);
        }

        public IEnumerable<Student> GetPassTestResults(int groupId, string searchString)
        {
            IEnumerable<Student> students;

            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                students = repositoriesContainer.StudentsRepository.GetAll(new Query<Student>(student => student.GroupId == groupId)
                    .Include(student => student.User.TestPassResults))
                    .ToList();
            }

            return students;
        }

        private void ProcessSequenceAnswer(List<Answer> answers, Question question, AnswerOnTestQuestion answerOntestQuestion)
        {
            bool isCorrect = true;
            if (answers.Count() != question.Answers.Count)
            {
                throw new InvalidDataException("Последовательность не совпадает с исходной");
            }

            var plainAnswers = question.Answers.ToList();
            for (int i = 0; i < answers.Count(); i++)
            {
                isCorrect = isCorrect && answers[i].Id == plainAnswers[i].Id;
            }

            if (isCorrect)
            {
                answerOntestQuestion.Points = question.ComlexityLevel;
            }
        }

        private void ProcessTextAnswer(IEnumerable<Answer> userAnswers, Question question, AnswerOnTestQuestion answerOntestQuestion)
        {
            if (userAnswers.Count() != 1)
            {
                throw new InvalidDataException("Пользователь должен указать 1 правильный ответ");
            }

            if (question.Answers.Select(answer => answer.Content.ToLower()).Contains(userAnswers.Single().Content.ToLower()))
            {
                answerOntestQuestion.Points = question.ComlexityLevel;
            }
        }

        private void ProcessManyVariantsAnswer(IEnumerable<Answer> userAnswers, Question question, AnswerOnTestQuestion answerOntestQuestion)
        {
            if (userAnswers.Count(answer => answer.СorrectnessIndicator > 0) == 0)
            {
                throw new InvalidDataException("Пользователь должен указать хотя бы 1 правильный ответ");
            }

            IEnumerable<Answer> correctAnswers = question.Answers.Where(answer => answer.СorrectnessIndicator > 0);

            bool isCorrect = true;
            foreach (var correctAnswer in correctAnswers)
            {
                isCorrect = isCorrect && userAnswers
                    .Where(answer => answer.СorrectnessIndicator > 0)
                    .Any(userAnswer => userAnswer.Id == correctAnswer.Id);
            }

            isCorrect = isCorrect && userAnswers.Count(answer => answer.СorrectnessIndicator > 0) == correctAnswers.Count();

            if (isCorrect)
            {
                answerOntestQuestion.Points = question.ComlexityLevel;
            }
        }

        private void ProcessOneVariantAnswer(IEnumerable<Answer> userAnswers, Question question, AnswerOnTestQuestion answerOntestQuestion)
        {
            if (userAnswers.Count(answer => answer.СorrectnessIndicator > 0) != 1)
            {
                throw new InvalidDataException("Пользователь должен указать 1 правильный ответ");
            }

            Answer correctAnswer = question.Answers.Single(answer => answer.СorrectnessIndicator > 0);

            if (correctAnswer.Id == userAnswers.Single(answer => answer.СorrectnessIndicator > 0).Id)
            {
                answerOntestQuestion.Points = question.ComlexityLevel;
            }
        }

        private void SaveAnswerOnTestQuestion(AnswerOnTestQuestion answerOnTestQuestion)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.RepositoryFor<AnswerOnTestQuestion>().Save(answerOnTestQuestion);
                repositoriesContainer.ApplyChanges();
            }
        }

        private AnswerOnTestQuestion GetAnswerOntestQuestion(int userId, int testId, int questionNumber)
        {
            AnswerOnTestQuestion answerOnTestQuestion;
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                answerOnTestQuestion =
                    repositoriesContainer.RepositoryFor<AnswerOnTestQuestion>().GetBy(
                        new Query<AnswerOnTestQuestion>(answer => answer.UserId == userId && answer.TestId == testId && answer.Number == questionNumber));
            }

            return answerOnTestQuestion;
        }

        private Answer GetAnswerById(int id)
        {
            Answer answerResult;
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                answerResult =
                    repositoriesContainer.RepositoryFor<Answer>().GetBy(
                        new Query<Answer>(answer => answer.Id == id));
            }

            return answerResult;
        }

        private Dictionary<int, PassedQuestionResult> GetQuestionStatuses(IEnumerable<AnswerOnTestQuestion> testAnswers)
        {
            return testAnswers.ToDictionary(question => question.Number, GetQuestionStatus);
        }

        private PassedQuestionResult GetQuestionStatus(AnswerOnTestQuestion answer)
        {
            if (!answer.Time.HasValue)
            {
                return PassedQuestionResult.NotPassed;
            }

            return answer.Points > 0
                ? PassedQuestionResult.Success
                : PassedQuestionResult.Error;
        }

        private Tuple<Question, int> GetQuestion(IEnumerable<AnswerOnTestQuestion> testAnswers, int nextQuestionNumber, int userId)
        {
            var notPassedQuestions = testAnswers.Where(testAnswer => !testAnswer.Time.HasValue).ToList();
            if (notPassedQuestions.Any())
            {
                Tuple<Question, int> nextQuestion = GetNextQuestionsFromNotPassedItems(notPassedQuestions, nextQuestionNumber);
                return nextQuestion;
            }
            else
            {
                CloseTest(testAnswers, userId);
                return null;
            }
        }

        private void CloseTest(IEnumerable<AnswerOnTestQuestion> testAnswers, int userId)
        {
            int testId = testAnswers.First().TestId;
            var testPassResult = new TestPassResult
            {
                TestId = testId,
                StudentId = userId,
                Points = GetResultPoints(testAnswers),
                Time = DateTime.UtcNow
            };

            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.RepositoryFor<AnswerOnTestQuestion>().Delete(testAnswers);
                repositoriesContainer.RepositoryFor<TestPassResult>().Save(testPassResult);

                repositoriesContainer.ApplyChanges();
            }
        }

        private int GetResultPoints(IEnumerable<AnswerOnTestQuestion> testAnswers)
        {
            return testAnswers.Sum(testAnswer => testAnswer.Points);
        }

        private Tuple<Question, int> GetNextQuestionsFromNotPassedItems(List<AnswerOnTestQuestion> notPassedQuestions, int nextQuestionNumber)
        {
            int questionId;
            if (notPassedQuestions.Any(question => question.Number == nextQuestionNumber))
            {
                questionId = notPassedQuestions
                    .Single(question => question.Number == nextQuestionNumber)
                    .QuestionId;
            }
            else
            {
                var orderedAnswers = notPassedQuestions.OrderBy(question => question.Number).SkipWhile(question => question.Number < nextQuestionNumber).ToList();
                orderedAnswers.AddRange(notPassedQuestions.OrderBy(question => question.Number).TakeWhile(question => question.Number < nextQuestionNumber));

                AnswerOnTestQuestion answerOnTestQuestion = orderedAnswers.First();

                nextQuestionNumber = answerOnTestQuestion.Number;

                questionId = answerOnTestQuestion.QuestionId;
            }

            return new Tuple<Question, int>(GetQuestionById(questionId), nextQuestionNumber);
        }

        private Question GetQuestionById(int id)
        {
            Question queston;
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                queston =
                    repositoriesContainer.QuestionsRepository.GetBy(
                        new Query<Question>(question => question.Id == id)
                        .Include(question => question.Answers));
            }

            return queston;
        }

        /// <summary>
        /// Return records for current test or create
        /// </summary>
        private static List<AnswerOnTestQuestion> GetAnswersForTest(int testId, int userId)
        {
            List<AnswerOnTestQuestion> testAnswers;
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                IRepositoryBase<AnswerOnTestQuestion> repository = repositoriesContainer.RepositoryFor<AnswerOnTestQuestion>();
                testAnswers =
                    repository.GetAll(
                        new Query<AnswerOnTestQuestion>(
                            testAnswer => testAnswer.TestId == testId && testAnswer.UserId == userId)).ToList();
            }

            if (!testAnswers.Any())
            {
                StartNewTest(testId, userId);
                return GetAnswersForTest(testId, userId);
            }

            return testAnswers;
        }

        private static void StartNewTest(int testId, int userId)
        {
            Test test = GetTest(testId);
            int questionsCount = test.CountOfQuestions > test.Questions.Count
                ? test.Questions.Count
                : test.CountOfQuestions;

            var includedQuestions = test.Questions.Take(questionsCount);

            var answersTemplate = new List<AnswerOnTestQuestion>();

            int counter = 1;
            foreach (Question includedQuestion in includedQuestions)
            {
                answersTemplate.Add(new AnswerOnTestQuestion
                {
                    QuestionId = includedQuestion.Id,
                    TestId = testId,
                    UserId = userId,
                    Number = counter++
                });
            }

            SaveAnswersTemplate(answersTemplate);
        }

        private static void SaveAnswersTemplate(IEnumerable<AnswerOnTestQuestion> answersTemplate)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.RepositoryFor<AnswerOnTestQuestion>().Save(answersTemplate);
                repositoriesContainer.ApplyChanges();
            }
        }

        private static Test GetTest(int testId)
        {
            Test testResult;
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                testResult = repositoriesContainer.TestsRepository.GetBy(new Query<Test>(test => test.Id == testId)
                    .Include(test => test.Questions));
            }

            return testResult;
        }

        private void GheckForTestUnlockedForUser(int testId, int userId)
        {
            if (IsTestLockedForUser(testId, userId))
            {
                throw new AccessViolationException("Тест недоступен для текущего пользователя");
            }
        }

        /// <summary>
        /// Returns if test locked for student or user is lecturer
        /// </summary>
        private bool IsTestLockedForUser(int testId, int userId)
        {
            bool isTestLockedForUser;
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var userResult = repositoriesContainer.UsersRepository.GetBy(new Query<User>(user => user.Id == userId)
                    .Include(user => user.Lecturer));
                if (userResult.Lecturer != null)
                {
                    isTestLockedForUser = false;
                }
                else
                {
                    isTestLockedForUser = !repositoriesContainer.TestUnlocksRepository.GetAll(new Query<TestUnlock>()
                        .AddFilterClause(testUnlock => testUnlock.StudentId == userId)
                        .AddFilterClause(testUnlock => testUnlock.TestId == testId))
                        .Any();
                }
            }

            return isTestLockedForUser;
        }
    }
}
