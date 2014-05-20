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
    }
}
