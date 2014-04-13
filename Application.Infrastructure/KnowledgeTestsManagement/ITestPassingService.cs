using System.Collections.Generic;
using LMPlatform.Models;
using LMPlatform.Models.KnowledgeTesting;

namespace Application.Infrastructure.KnowledgeTestsManagement
{
    public interface ITestPassingService
    {
        NextQuestionResult GetNextQuestion(int testId, int userId, int currentQuestionNumber);

        void MakeUserAnswer(IEnumerable<Answer> answers, int userId, int testId, int questionNumber);

        IEnumerable<Student> GetPassTestResults(int groupId, string searchString = null);

        IEnumerable<Test> GetAvailableTestsForStudent(int studentId, int subjectId);

        IEnumerable<RealTimePassingResult> GetRealTimePassingResults(int testId);

        IEnumerable<Test> GetTestsForSubject(int subjectId);
    }
}
