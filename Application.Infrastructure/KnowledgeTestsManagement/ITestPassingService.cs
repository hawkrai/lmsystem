using System;
using System.Collections.Generic;
using LMPlatform.Models;
using LMPlatform.Models.KnowledgeTesting;

namespace Application.Infrastructure.KnowledgeTestsManagement
{
    public interface ITestPassingService
    {
        NextQuestionResult GetNextQuestion(int testId, int userId, int currentQuestionNumber);

        void MakeUserAnswer(IEnumerable<Answer> answers, int userId, int testId, int questionNumber);

        IEnumerable<Student> GetPassTestResults(int groupId, int subjectId);

        IEnumerable<Test> GetAvailableTestsForStudent(int studentId, int subjectId);

        IEnumerable<RealTimePassingResult> GetRealTimePassingResults(int subjectId);

        IEnumerable<Test> GetTestsForSubject(int subjectId);

        TestPassResult GetTestPassingTime(int testId, int studentId);

        List<TestPassResult> GetStidentResults(int subjectId, int currentUserId);

        List<AnswerOnTestQuestion> GetAnswersForTest(int testId, int userId);

        bool CheckForSubjectAvailableForStudent(int studentId, int subjectId);

        /// <summary>
        /// Returns set of students and average marks for subject
        /// </summary>
        Dictionary<int, double?> GetAverageMarkForTests(int groupId, int subjectId);
    }
}
