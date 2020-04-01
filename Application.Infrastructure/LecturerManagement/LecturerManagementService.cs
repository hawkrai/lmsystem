using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.UserManagement;
using Application.SearchEngine.SearchMethods;
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
                    .Include(e => e.SubjectLecturers.Select(x => x.Subject))
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

        public List<Lecturer> GetLecturers(Expression<Func<Lecturer, string>> orderBy = null, bool lite = false)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();

	        var lecturers = lite ? repositoriesContainer.LecturerRepository.GetAll() 
		        : repositoriesContainer.LecturerRepository.GetAll(new Query<Lecturer>().Include(e => e.SubjectLecturers).Include(e => e.User));
	        if (orderBy is { })
	        {
		        lecturers = lecturers.OrderBy(orderBy);
	        }
	        return lecturers.ToList();
		}

        public IPageableList<Lecturer> GetLecturersPageable(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null)
        {
            var query = new PageableQuery<Lecturer>(pageInfo);
            query.Include(l => l.SubjectLecturers.Select(t => t.Subject)).Include(e => e.User);

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.Replace(" ", string.Empty);

                //search by full name, login
                query.AddFilterClause(
                    e => (e.LastName + e.FirstName + e.MiddleName).Contains(searchString)
                    || e.User.UserName.Contains(searchString));
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
            new LecturerSearchMethod().UpdateIndex(lecturer);
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
                    //var subjects = lecturer.SubjectLecturers.ToList();
                    //repositoriesContainer.RepositoryFor<SubjectLecturer>().Delete(subjects);
                    //repositoriesContainer.ApplyChanges();
                    repositoriesContainer.LecturerRepository.DeleteLecturer(lecturer);
                }
            }
            new LecturerSearchMethod().DeleteIndex(id);
            return true;//UserManagementService.DeleteUser(id);
        }

        private readonly LazyDependency<IUsersManagementService> _userManagementService =
            new LazyDependency<IUsersManagementService>();

        public IUsersManagementService UserManagementService
        {
            get { return _userManagementService.Value; }
        }

	    public bool Join(int subjectId, int lectorId, int owner)
	    {
		    using var repositoriesContainer = new LmPlatformRepositoriesContainer();
		    repositoriesContainer.RepositoryFor<SubjectLecturer>().Save(new SubjectLecturer
		    {
			    LecturerId = lectorId,
			    SubjectId = subjectId,
			    Owner = owner
		    });
		    repositoriesContainer.ApplyChanges();

		    return true;
	    }

	    public List<Lecturer> GetJoinedLector(int subjectId, int owner)
	    {
		    using var repositoriesContainer = new LmPlatformRepositoriesContainer();
		    return repositoriesContainer.RepositoryFor<SubjectLecturer>().GetAll(
			    new Query<SubjectLecturer>(e => e.SubjectId == subjectId && e.Owner == owner)
				    .Include(e => e.Lecturer)).Select(e => e.Lecturer).ToList();
	    }

	    public void DisjoinLector(int subjectId, int lectorId, int? owner)
	    {
		    using var repositoriesContainer = new LmPlatformRepositoriesContainer();
		    var relation = repositoriesContainer.RepositoryFor<SubjectLecturer>().GetBy(
				    new Query<SubjectLecturer>(e => e.Owner == owner && e.LecturerId == lectorId && e.SubjectId == subjectId));

		    repositoriesContainer.RepositoryFor<SubjectLecturer>().Delete(relation);
		    repositoriesContainer.ApplyChanges();
	    }

	    public void DisjoinOwnerLector(int subjectId, int lectorId)
	    {
		    using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
		    {
			    var relation =
				    repositoriesContainer.RepositoryFor<SubjectLecturer>().GetBy(
					    new Query<SubjectLecturer>(e => e.Owner.HasValue == false && e.LecturerId == lectorId && e.SubjectId == subjectId));

			    repositoriesContainer.RepositoryFor<SubjectLecturer>().Delete(relation);
			    repositoriesContainer.ApplyChanges();
		    }
	    }

    }
}
