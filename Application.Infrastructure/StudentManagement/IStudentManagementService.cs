using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace Application.Infrastructure.StudentManagement
{
    public interface IStudentManagementService
    {
        Student GetStudent(int userId);

        IEnumerable<Student> GetGroupStudents(int groupId);

        IEnumerable<Student> GetStudents();

        IPageableList<Student> GetStudentsPageable(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null);

        void Save(Student student);

        void UpdateStudent(Student student);
    }
}