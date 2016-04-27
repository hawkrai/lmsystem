﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.BugManagement;
using Application.Infrastructure.MessageManagement;
using Application.Infrastructure.ProjectManagement;
using Application.Infrastructure.UserManagement;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;
using Application.SearchEngine.SearchMethods;

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

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.Replace(" ", string.Empty);

                //search by full name
                query.AddFilterClause(
                    e => (e.LastName + e.FirstName + e.MiddleName).Contains(searchString)
                    || e.Group.Name.ToLower().Contains(searchString));
            }

            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var students = repositoriesContainer.StudentsRepository.GetPageableBy(query);
                return students;
            }
        }

        public Student Save(Student student)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.StudentsRepository.SaveStudent(student);
                repositoriesContainer.ApplyChanges();                
            }
            return student;
        }

        public void UpdateStudent(Student student)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.StudentsRepository.Save(student);
	            var user = repositoriesContainer.UsersRepository.GetBy(new Query<User>(e => e.Id == student.User.Id));
	            user.UserName = student.User.UserName;
	            user.Avatar = student.User.Avatar;
				repositoriesContainer.UsersRepository.Save(user);
                repositoriesContainer.ApplyChanges();
                new StudentSearchMethod().UpdateIndex(student);
            }
        }

        public bool DeleteStudent(int id)
        {
            new StudentSearchMethod().DeleteIndex(id);
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