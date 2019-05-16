using System;
using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Models.DP;
using WebMatrix.WebData;

namespace Application.Infrastructure.UserManagement
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Security;

    using Application.Core;
    using Application.Core.Constants;
    using Application.Core.Extensions;
    using Application.Infrastructure.AccountManagement;
    using Application.Infrastructure.ProjectManagement;

    using LMPlatform.Data.Repositories;
    using LMPlatform.Data.Repositories.RepositoryContracts;
    using LMPlatform.Models;
    using CPManagement;
    public class UsersManagementService : IUsersManagementService
    {
        private readonly LazyDependency<IUsersRepository> _usersRepository = new LazyDependency<IUsersRepository>();
        private readonly LazyDependency<IAccountManagementService> _accountManagementService = new LazyDependency<IAccountManagementService>();
        private readonly LazyDependency<IProjectManagementService> _projectManagementService = new LazyDependency<IProjectManagementService>();

        private readonly LazyDependency<ICPManagementService> _cpManagementService = new LazyDependency<ICPManagementService>();

        private ICPManagementService CPManagementService
        {
            get { return _cpManagementService.Value; }
        }

        public IUsersRepository UsersRepository
        {
            get
            {
                return _usersRepository.Value;
            }
        }

        public IAccountManagementService AccountManagementService
        {
            get
            {
                return _accountManagementService.Value;
            }
        }

        public IProjectManagementService ProjectManagementService
        {
            get
            {
                return _projectManagementService.Value;
            }
        }

	    private readonly LazyDependency<IDpContext> context = new LazyDependency<IDpContext>();

	    private IDpContext Context
	    {
		    get { return context.Value; }
	    }

        public User GetUser(string userName)
        {
            if (IsExistsUser(userName))
            {
                return UsersRepository.GetAll(new Query<User>()
                    .Include(u => u.Student).Include(e => e.Student.Group).Include(u => u.Lecturer).Include(u => u.Membership.Roles))
                    .Single(e => e.UserName == userName);
            }

            return null;
        }

        public User GetUserByName(string firstName, string lastName, string middleName)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var checkPatronymic = !string.IsNullOrEmpty(middleName);

                var lecturers = repositoriesContainer.LecturerRepository.GetAll(
                        new Query<Lecturer>(e =>
                            (e.FirstName == firstName && e.LastName == lastName && !checkPatronymic)
                            || (checkPatronymic && (e.MiddleName == middleName && e.FirstName == firstName && e.LastName == lastName))))
                            .Select(l => l.User).ToList();

                if (lecturers.Any())
                {
                    return lecturers.First();
                }

                var students = repositoriesContainer.StudentsRepository.GetAll(
                        new Query<Student>(e =>
                            (e.FirstName == firstName && e.LastName == lastName && !checkPatronymic)
                            || (checkPatronymic && (e.MiddleName == middleName && e.FirstName == firstName && e.LastName == lastName))))
                            .Select(l => l.User);

                return students.Any() ? students.First() : null;
            }
        }

        public User GetUser(int id)
        {
            return UsersRepository.GetBy(new Query<User>(u => u.Id == id)
                .Include(u => u.Student).Include(u => u.Lecturer).Include(u => u.Membership.Roles));
        }

        public bool IsExistsUser(string userName)
        {
            if (UsersRepository.GetAll().Any(e => e.UserName == userName))
            {
                return true;
            }

            return false;
        }

        public User CurrentUser
        {
            get
            {
                var userName = WebSecurity.CurrentUserName;

                return GetUser(userName);
            }
        }

        public bool DeleteUser(int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var query = new Query<User>().AddFilterClause(u => u.Id == id).Include(u => u.ProjectUsers).Include(u=>u.Student);
                var user = repositoriesContainer.UsersRepository.GetBy(query);

                repositoriesContainer.MessageRepository.DeleteUserMessages(user.Id);

                var projects = user.ProjectUsers.DistinctBy(e => e.ProjectId).Select(e => e.ProjectId);
                foreach (var projectId in projects)
                {
                    ProjectManagementService.DeleteUserFromProject(id, projectId);
                }

	            if (user.Student != null)
	            {
					var acp = user.Student.AssignedCourseProjects.Select(e => e.CourseProjectId);
					foreach (var acpId in acp)
					{
						CPManagementService.DeleteUserFromAcpProject(id, acpId);
					}

		            var subjects = repositoriesContainer.RepositoryFor<SubjectStudent>()
			            .GetAll(new Query<SubjectStudent>(e => e.StudentId == id));

					foreach (var subjectS in subjects)
		            {
			            repositoriesContainer.RepositoryFor<SubjectStudent>().Delete(subjectS);
		            }

		            var diploms = Context.AssignedDiplomProjects.Where(e => e.StudentId == id).ToList();

		            var diplomsRessList = Context.DiplomPercentagesResults.Where(e => e.StudentId == id).ToList();

					foreach (var diplom in diploms)
		            {
						Context.AssignedDiplomProjects.Remove(diplom);
			            Context.SaveChanges();
		            }

					foreach (var diplomsRes in diplomsRessList)
		            {
						Context.DiplomPercentagesResults.Remove(diplomsRes);
			            Context.SaveChanges();
		            }
	            }

                CPManagementService.DeletePercenageAndVisitStatsForUser(id);

	            repositoriesContainer.ApplyChanges();
                var result = AccountManagementService.DeleteAccount(user.UserName);
                repositoriesContainer.ApplyChanges();

                return result;
            }
        }

        public User GetAdmin()
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var admin = Roles.GetUsersInRole(Constants.Roles.Admin);
                if (admin.Length <= 0)
                {
                    return null;
                }

                var adminName = admin[0];
                var user = repositoriesContainer.UsersRepository.GetBy(new Query<User>().AddFilterClause(u => u.UserName == adminName));
                return user;
            }
        }

        public IEnumerable<User> GetUsers(bool includeRole = false)
        {
            var query = new Query<User>();
            if (includeRole)
            {
                query.Include(u => u.Membership.Roles);
            }

            return UsersRepository.GetAll(query).ToList();
        }

        public void UpdateLastLoginDate(string userName)
        {
            var user = GetUser(userName);
            var now = DateTime.Now;
            user.LastLogin = now;
            var attendanceList = user.AttendanceList;
            attendanceList.Add(now);
            user.AttendanceList = attendanceList;
            UsersRepository.Save(user, u => u.LastLogin == now);
        }
    }
}
