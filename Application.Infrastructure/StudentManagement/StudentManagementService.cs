using System.Collections.Generic;
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

                //search by full name, group number, login
                query.AddFilterClause(
                    e => (e.LastName + e.FirstName + e.MiddleName).Contains(searchString)
                    || e.Group.Name.ToLower().Contains(searchString)
                    || e.User.UserName.Contains(searchString));
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

		public int CountUnconfirmedStudents(int lecturerId)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var subjects = repositoriesContainer.RepositoryFor<SubjectLecturer>().GetAll(new Query<SubjectLecturer>(e => e.LecturerId == lecturerId).Include(e => e.Subject.SubjectGroups.Select(x => x.Group)));

				var groupIds = new List<int>();

				foreach (var subject in subjects)
				{
					groupIds.AddRange(subject.Subject.SubjectGroups.Select(e => e.GroupId));
				}

				var query =
					repositoriesContainer.RepositoryFor<Student>().GetAll(
						new Query<Student>(e => groupIds.Contains(e.GroupId) && !e.Confirmed.Value));

				var count = query.Any() ? query.Count() : 0;

				return count;
			}
		}

	    public void СonfirmationStudent(int studentId)
	    {
		    using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
		    {
				var student = this.GetStudent(studentId);

				student.Confirmed = true;

				this.UpdateStudent(student);

				var subjects = repositoriesContainer.SubjectRepository.GetSubjects(student.GroupId).Where(e => !e.IsArchive);

			    foreach (var subject in subjects)
			    {
				    if (!subject.SubjectGroups.Any(e => e.SubjectStudents.Any(x => x.StudentId == student.Id)))
				    {
						var firstOrDefault = subject.SubjectGroups.FirstOrDefault(e => e.GroupId == student.GroupId);
						if (firstOrDefault != null)
						{
							var subjectGroupId = firstOrDefault.Id;

							var modelFirstSubGroup = repositoriesContainer.SubGroupRepository.GetBy(new Query<SubGroup>(e => e.SubjectGroupId == subjectGroupId && e.Name == "first"));

							var subjectStudent = new SubjectStudent
							{
								StudentId = studentId,
								SubGroupId = modelFirstSubGroup.Id,
								SubjectGroupId = subjectGroupId
							};
							repositoriesContainer.RepositoryFor<SubjectStudent>().Save(subjectStudent);
							repositoriesContainer.ApplyChanges();
						}
				    }
			    }
		    }
	    }

		public void UnConfirmationStudent(int studentId)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var student = this.GetStudent(studentId);

				student.Confirmed = false;

				this.UpdateStudent(student);

				var subjects = repositoriesContainer.SubjectRepository.GetSubjects(student.GroupId).Where(e => !e.IsArchive);

				foreach (var subject in subjects)
				{
					if (subject.SubjectGroups.Any(e => e.SubjectStudents.Any(x => x.StudentId == student.Id)))
					{
						var firstOrDefault = subject.SubjectGroups.FirstOrDefault(e => e.GroupId == student.GroupId);

						if (firstOrDefault != null)
						{
							var subjectStudent = firstOrDefault.SubjectStudents.FirstOrDefault(e => e.StudentId == studentId);

							repositoriesContainer.RepositoryFor<SubjectStudent>().Delete(subjectStudent);
							repositoriesContainer.ApplyChanges();
						}
					}
				}
			}
		}

	    private readonly LazyDependency<IUsersManagementService> _userManagementService =
            new LazyDependency<IUsersManagementService>();

        public IUsersManagementService UserManagementService
        {
            get { return _userManagementService.Value; }
        }
    }
}