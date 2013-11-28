using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Core.Data;
using LMPlatform.Data.Repositories;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace Application.Infrastructure.StudentManagement
{
    public class StudentManagementService : IStudentManagementService
    {
        public Student GetStudent(int userId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.StudentsRepository.GetBy(new Query<Student>(e => e.Id == userId).Include(e => e.Group));
            }
        }

        public List<Student> GetGroupStudents(int groupId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.StudentsRepository.GetAll(new Query<Student>(e => e.GroupId == groupId).Include(e => e.Group)).ToList();
            }
        }

        public List<Student> GetStudents()
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.StudentsRepository.GetAll(new Query<Student>().Include(e => e.Group)).ToList();
            }
        }

        public void Save(Student student)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.StudentsRepository.SaveStudent(student);
                repositoriesContainer.ApplyChanges();
            }
        }
    }
}