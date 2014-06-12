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
                return repositoriesContainer.LecturerRepository.GetBy(new Query<Lecturer>(e => e.Id == userId).Include(e => e.SubjectLecturers).Include(e => e.User));
            }
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
