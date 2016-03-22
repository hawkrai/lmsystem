using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.UserManagement;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;

namespace Application.Infrastructure.LecturerManagement
{
    public class LecturerManagementService : ILecturerManagementService
    {
        public Lecturer GetLecturer(int userId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.LecturerRepository.GetBy(
                    new Query<Lecturer>(e => e.Id == userId)
                    .Include(e => e.SubjectLecturers)
                    .Include(e => e.User)
                    .Include(e => e.SecretaryGroups));
            }
        }

		public List<List<string>> GetLecturesScheduleMarks(int subjectId, int groupId)
		{
			var data = new List<List<string>>();
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var group = repositoriesContainer.GroupsRepository.GetBy(new Query<Group>(e => e.Id == groupId).Include(e => e.Students.Select(x => x.LecturesVisitMarks.Select(t => t.LecturesScheduleVisiting))));

				foreach (var student in group.Students)
				{
					var row = new List<string> {student.FullName};


					row.AddRange(student.LecturesVisitMarks.OrderBy(e => e.LecturesScheduleVisiting.Date).Select(lecturesVisitMark => lecturesVisitMark.Mark));

					data.Add(row);
				}
			}
			return data;
		}

	    public List<string> GetLecturesScheduleVisitings(int subjectId)
	    {
		    var data = new List<string>();
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var subject =
					repositoriesContainer.SubjectRepository.GetBy(
						new Query<Subject>(e => e.Id == subjectId).Include(e => e.LecturesScheduleVisitings));

				data.AddRange(subject.LecturesScheduleVisitings.OrderBy(e => e.Date).Select(lecturesScheduleVisiting => lecturesScheduleVisiting.Date.ToString("dd/MM/yyyy")));
			}
		    return data;
	    }

        public List<Lecturer> GetLecturers()
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.LecturerRepository.GetAll(new Query<Lecturer>().Include(e => e.SubjectLecturers).Include(e => e.User)).ToList();
            }
        }

        public IPageableList<Lecturer> GetLecturersPageable(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null)
        {
            var query = new PageableQuery<Lecturer>(pageInfo);
            query.Include(l => l.SubjectLecturers).Include(e => e.User);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.Replace(" ", string.Empty);

                //search by full name
                query.AddFilterClause(
                    e => (e.LastName + e.FirstName + e.MiddleName).Contains(searchString));
            }

            query.OrderBy(sortCriterias);
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var lecturers = repositoriesContainer.LecturerRepository.GetPageableBy(query);
                return lecturers;
            }
        }

        public Lecturer Save(Lecturer lecturer)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.LecturerRepository.SaveLecturer(lecturer);
                repositoriesContainer.ApplyChanges();
            }

            return lecturer;
        }

        public Lecturer UpdateLecturer(Lecturer lecturer)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.LecturerRepository.Save(lecturer);
	            var user = repositoriesContainer.UsersRepository.GetBy(new Query<User>(e => e.Id == lecturer.User.Id));
	            user.Avatar = lecturer.User.Avatar;
				user.SkypeContact = lecturer.User.SkypeContact;
				user.Email = lecturer.User.Email;
				user.About = lecturer.User.About;
				user.Phone = lecturer.User.Phone;
				repositoriesContainer.UsersRepository.Save(user);
                repositoriesContainer.ApplyChanges();
            }

            return lecturer;
        }

        public bool DeleteLecturer(int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var lecturer = repositoriesContainer.LecturerRepository.GetBy(
                     new Query<Lecturer>(e => e.Id == id).Include(e => e.SubjectLecturers));

                if (lecturer != null && lecturer.SubjectLecturers != null)
                {
                    var subjects = lecturer.SubjectLecturers.ToList();
                    repositoriesContainer.RepositoryFor<SubjectLecturer>().Delete(subjects);
                    repositoriesContainer.ApplyChanges();
                }
            }

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
