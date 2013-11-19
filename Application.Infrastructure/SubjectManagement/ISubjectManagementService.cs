using System.Collections.Generic;
using LMPlatform.Models;

namespace Application.Infrastructure.SubjectManagement
{
    public interface ISubjectManagementService
    {
        List<Subject> GetUserSubjects(int userId);

        Subject GetSubject(int id);
    }
}