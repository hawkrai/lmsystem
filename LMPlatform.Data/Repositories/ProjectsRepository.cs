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

        public List<Project> GetUserProjects(int userId, int limit, int offset, string searchString, string sortingPropertyName, bool desc)
        {
            using(var context = new LmPlatformModelsContext())
            {
                var query = GetUserProjectsQuery(context, userId, searchString);
                return GetUserProjectsSortedQuery(query, sortingPropertyName, desc)
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

        public List<Project> GetUserProjectParticipations(int userId, int limit, int offset, string searchString, string sortingPropertyName, bool desc = false)
        {
            using(var context = new LmPlatformModelsContext())
            {
                var query = GetUserProjectParticipationsQuery(context, userId, searchString);
                var sortedQuery = GetUserProjectsSortedQuery(query, sortingPropertyName, desc);
                return GetUserProjectsSortedQuery(sortedQuery, sortingPropertyName, desc)
                    .Skip(offset)
                    .Take(limit)
                    .ToList();
            }
        }

        public int GetUserProjectParticipationsCount(int userId, string searchString)
        {
            using(var context = new LmPlatformModelsContext())
            {
                return GetUserProjectParticipationsQuery(context, userId, searchString).Count();
            }
        }

        public List<Student> GetStudentsGroupParticipations(int groupId, int limit, int offset)
        {
            using(var context = new LmPlatformModelsContext())
            {
                var query = GetStudentsGroupParticipationsQuery(context, groupId);
                return query
                    .OrderBy(e => e.LastName)
                    .Skip(offset)
                    .Take(limit)
                    .ToList();
            }
        }

        public int GetStudentsGroupParticipationsCount(int groupId)
        {
            using(var context = new LmPlatformModelsContext())
            {
                return GetStudentsGroupParticipationsQuery(context, groupId).Count();
            }
        }

        public Project GetProjectWithData(int id, bool withBugsAndMembers = false)
        {
            using(var context = new LmPlatformModelsContext())
            {   
                var query = context.Set<Project>()
                    .Include(e => e.Creator.Lecturer)
                    .Include(e => e.Creator.Student)
                    .Include(e => e.ProjectUsers)
                    .Include(e => e.Bugs)
                    .Include(e => e.Bugs.Select(e2 => e2.Severity))
                    .Include(e => e.Bugs.Select(e2 => e2.Status));

                return AddBugsAndMembersToProjectQuery(query, withBugsAndMembers).First(e => e.Id == id);
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
                .Where(e => searchString == null ? true : e.Title.Contains(searchString));
        }

        private IQueryable<Project> GetUserProjectParticipationsQuery(LmPlatformModelsContext context, int userId, string searchString)
        {
            return GetUserProjectsQuery(context, userId, searchString)
                .Include(e => e.ProjectUsers.Select(e2 => e2.ProjectRole));
        }

        private IQueryable<Student> GetStudentsGroupParticipationsQuery(LmPlatformModelsContext context, int groupId)
        {
            return context.Set<Student>()
                .Where(e => e.GroupId == groupId)
                .Include(e => e.User.ProjectUsers.Select(p => p.Project))
                .Include(e => e.User.ProjectUsers.Select(p => p.Project.Creator.Lecturer))
                .Include(e => e.User.ProjectUsers.Select(e2 => e2.ProjectRole))
                .Where(e => e.User.ProjectUsers.Count > 0);
        }

        private IQueryable<Project> AddBugsAndMembersToProjectQuery(IQueryable<Project> query, bool withData)
        {
            if (withData)
            {
                return query
                    .Include(e => e.ProjectUsers.Select(e2 => e2.User).Select(e2 => e2.Lecturer))
                    .Include(e => e.ProjectUsers.Select(e2 => e2.User).Select(e2 => e2.Student))
                    .Include(e => e.ProjectUsers.Select(e2 => e2.ProjectRole));
            } else
            {
                return query;
            }
        }

        private IQueryable<Project> GetUserProjectsSortedQuery(IQueryable<Project> query, string sortingPropertyName, bool desc)
        {
            switch(sortingPropertyName)
            {
                case "UserQuentity":
                    if(desc)
                        return query.OrderByDescending(e => e.ProjectUsers.Count);
                    else
                        return query.OrderBy(e => e.ProjectUsers.Count);
                case "Title":
                    if(desc)
                        return query.OrderByDescending(e => e.Title);
                    else
                        return query.OrderBy(e => e.Title);
                case "CreatorName":
                    if(desc)
                        return query.OrderByDescending(e => e.Creator.Student == null ? e.Creator.Lecturer.LastName : e.Creator.Student.LastName);
                    else
                        return query.OrderBy(e => e.Creator.Student == null ? e.Creator.Lecturer.LastName : e.Creator.Student.LastName);
                default:
                    if(desc)
                        return query.OrderByDescending(e => e.DateOfChange);
                    else
                        return query.OrderBy(e => e.DateOfChange);
            }
        }
    }
}
