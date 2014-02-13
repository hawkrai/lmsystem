using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core.Data;
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

            if (!string.IsNullOrEmpty(searchString))
            {
                query.AddFilterClause(
                    e => e.LastName.ToLower().StartsWith(searchString) || e.LastName.ToLower().Contains(searchString));
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
    }
}
