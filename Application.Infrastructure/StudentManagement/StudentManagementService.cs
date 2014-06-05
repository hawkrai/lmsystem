using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.MessageManagement;
using Application.Infrastructure.UserManagement;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;

namespace Application.Infrastructure.StudentManagement
{
    public class StudentManagementService : IStudentManagementService
    {
        public Student GetStudent(int userId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.StudentsRepository.GetBy(new Query<Student>(e => e.Id == userId).Include(e => e.Group).Include(e => e.User));
            }
        }

        public IEnumerable<Student> GetGroupStudents(int groupId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.StudentsRepository.GetAll(new Query<Student>(e => e.GroupId == groupId).Include(e => e.Group)).ToList();
            }
        }

        public IEnumerable<Student> GetStudents()
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.StudentsRepository.GetAll(new Query<Student>().Include(e => e.Group).Include(e => e.User)).ToList();
            }
        }

        public IPageableList<Student> GetStudentsPageable(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null)
        {
            var query = new PageableQuery<Student>(pageInfo);
            query.Include(e => e.Group).Include(e => e.User);

            if (!string.IsNullOrEmpty(searchString))
            {
                query.AddFilterClause(
                    e => e.LastName.ToLower().StartsWith(searchString) || e.LastName.ToLower().Contains(searchString));
            }

            query.OrderBy(sortCriterias);
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var students = repositoriesContainer.StudentsRepository.GetPageableBy(query);
                return students;
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

        public void UpdateStudent(Student student)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.StudentsRepository.Save(student);
                repositoriesContainer.ApplyChanges();
            }
        }

        public bool DeleteStudent(int id)
        {
            return UserManagementService.DeleteUser(id);
        }

        private readonly LazyDependency<IUsersManagementService> _userManagementService =
            new LazyDependency<IUsersManagementService>();

        public IUsersManagementService UserManagementService
        {
            get { return _userManagementService.Value; }
        }
    }
}