using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Application.Core.Data;
using Application.Core.Extensions;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories
{
    public class ProjectsRepository : RepositoryBase<LmPlatformModelsContext, Project>, IProjectsRepository
    {
        public ProjectsRepository(LmPlatformModelsContext dataContext)
            : base(dataContext)
        {
        }

        public List<Project> GetUserProjects(int userId, int limit, int offset, string searchString)
        {
            using(var context = new LmPlatformModelsContext())
            {
                return GetUserProjectsQuery(context, userId, searchString)
                    .Skip(offset)
                    .Take(limit)
                    .ToList();
            }
        }

        public int GetUserProjectsCount(int userId, string searchString)
        {
            using(var context = new LmPlatformModelsContext())
            {
                return GetUserProjectsQuery(context, userId, searchString).Count();
            }
        }

        public void DeleteProject(Project project)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var model = context.Set<Project>().FirstOrDefault(e => e.Id == project.Id);
                context.Delete(model);

                context.SaveChanges();
            }
        }

        public List<Group> GetGroups(int lecturerId = 0)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var subjectLecturer =
                    context.Set<SubjectLecturer>().Include(e => e.Subject).Where(e => e.LecturerId == lecturerId);
                var subjects = subjectLecturer.Select(e => e.Subject);
                var groupList = subjects.SelectMany(e => e.SubjectGroups);
                var groups = groupList.Select(e => e.Group).DistinctBy(g => g.Name);
                return groups.ToList();
            }
        }

        private IQueryable<Project> GetUserProjectsQuery(LmPlatformModelsContext context, int userId, string searchString)
        {
            return context.Set<Project>()
                .Include(e => e.Creator.Lecturer)
                .Include(e => e.Creator.Student)
                .Include(e => e.ProjectUsers)
                .Where(e => e.ProjectUsers.Any(e2 => e2.UserId == userId))
                .Where(e => searchString == null ? true : e.Title.Contains(searchString))
                .OrderBy(e => e.Id);
        }
    }
}
