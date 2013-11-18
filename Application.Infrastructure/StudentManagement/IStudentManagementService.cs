using System.Collections.Generic;
using LMPlatform.Models;

namespace Application.Infrastructure.StudentManagement
{
    public interface IStudentManagementService
    {
        Student GetStudent(int userId);

        List<Student> GetGroupStudents(int groupId);
    }
}