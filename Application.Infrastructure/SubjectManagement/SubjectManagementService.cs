using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.StudentManagement;
using LMPlatform.Data.Repositories;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace Application.Infrastructure.SubjectManagement
{
    public class SubjectManagementService : ISubjectManagementService
    {
        private readonly LazyDependency<IStudentManagementService> _studentManagementService = new LazyDependency<IStudentManagementService>();

        public IStudentManagementService StudentManagementService
        {
            get
            {
                return _studentManagementService.Value;
            }
        }
        
        public List<Subject> GetUserSubjects(int userId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var user = repositoriesContainer.UsersRepository.GetBy(new Query<User>(e => e.Id == userId)
                    .Include(e => e.Lecturer)
                    .Include(e => e.Student));
                if (user.Student != null)
                {
                    return repositoriesContainer.SubjectRepository.GetSubjects(groupId: user.Student.GroupId);    
                }
                else
                {
                    return repositoriesContainer.SubjectRepository.GetSubjects(lecturerId: user.Lecturer.Id);    
                }
            }
        }

        public Subject GetSubject(int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.SubjectRepository.GetBy(new Query<Subject>(e => e.Id == id).Include(e => e.SubjectModules));
            }
        }

        public IPageableList<Subject> GetSubjectsLecturer(int lecturerId, string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null)
        {
            var query = new PageableQuery<Subject>(pageInfo, e => e.SubjectLecturers.Any(x => x.LecturerId == lecturerId));
            
            if (!string.IsNullOrEmpty(searchString))
            {
                query.AddFilterClause(
                    e => e.Name.ToLower().StartsWith(searchString) || e.Name.ToLower().Contains(searchString));
            }

            query.OrderBy(sortCriterias);
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.SubjectRepository.GetPageableBy(query);
            }
        }

        public Subject SaveSubject(Subject subject)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.SubjectRepository.Save(subject);

                repositoriesContainer.ApplyChanges();
            }

            return subject;
        }
    }
}