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

        IEnumerable<TestPassResult> GetStidentResults(int subjectId, int currentUserId);

        bool CheckForSubjectAvailableForStudent(int studentId, int subjectId);
    }
}
