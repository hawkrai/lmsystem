using System.Collections.Generic;
using Application.Core;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace Application.Infrastructure.StudentManagement
{
    public class StudentManagementService : IStudentManagementService
    {
        private readonly LazyDependency<IStudentsRepository> _studentsRepository = new LazyDependency<IStudentsRepository>();

        public IStudentsRepository StudentsRepository
        {
            get
            {
                return _studentsRepository.Value;
            }
        }

        public Student GetStudent(int userId)
        {
            return StudentsRepository.GetStudent(userId);
        }

        public List<Student> GetGroupStudents(int groupId)
        {
            return StudentsRepository.GetStudents(groupId);
        }
    }
}