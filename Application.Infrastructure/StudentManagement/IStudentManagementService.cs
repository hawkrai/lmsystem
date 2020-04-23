using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace Application.Infrastructure.StudentManagement
{
    public interface IStudentManagementService
    {
        Student GetStudent(int userId, bool lite = false);

        IEnumerable<Student> GetGroupStudents(int groupId);

        IEnumerable<Student> GetStudents(IQuery<Student> query = null);

        IPageableList<Student> GetStudentsPageable(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null);

        Student Save(Student student);

        void UpdateStudent(Student student);

        bool DeleteStudent(int id);

	    int CountUnconfirmedStudents(int lecturerId);

	    void СonfirmationStudent(int studentId);

	    void UnConfirmationStudent(int studentId);
    }
}