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
	        using var context = new LmPlatformModelsContext();
	        var query = GetUserProjectsQuery(context, userId, searchString);
	        return GetUserProjectsSortedQuery(query, sortingPropertyName, desc)
		        .Skip(offset)
		        .Take(limit)
		        .ToList();
        }

        public int GetUserProjectsCount(int userId, string searchString)
        {
	        using var context = new LmPlatformModelsContext();
	        return GetUserProjectParticipationsCountQuery(context, userId, searchString).Count();
        }

        public List<Project> GetUserProjectParticipations(int userId, int limit, int offset, string searchString, string sortingPropertyName, bool desc = false)
        {
	        using var context = new LmPlatformModelsContext();
	        var query = GetUserProjectParticipationsQuery(context, userId, searchString);
	        var sortedQuery = GetUserProjectsSortedQuery(query, sortingPropertyName, desc);
	        return GetUserProjectsSortedQuery(sortedQuery, sortingPropertyName, desc)
		        .Skip(offset)
		        .Take(limit)
		        .ToList();
        }

        public int GetUserProjectParticipationsCount(int userId, string searchString)
        {
	        using var context = new LmPlatformModelsContext();
	        return GetUserProjectParticipationsCountQuery(context, userId, searchString).Count();
        }

        public List<Student> GetStudentsGroupParticipations(int groupId, int limit, int offset)
        {
	        using var context = new LmPlatformModelsContext();
	        var query = GetStudentsGroupParticipationsQuery(context, groupId);
	        return query
		        .OrderBy(e => e.LastName)
		        .Skip(offset)
		        .Take(limit)
		        .ToList();
        }

        public int GetStudentsGroupParticipationsCount(int groupId)
        {
	        using var context = new LmPlatformModelsContext();
	        return GetStudentsGroupParticipationsCountQuery(context, groupId).Count();
        }

        public Project GetProjectWithData(int id, bool withBugsAndMembers = false)
        {
	        using var context = new LmPlatformModelsContext();
	        var query = context.Set<Project>()
		        .Include(e => e.Creator.Lecturer) //needed
                .Include(e => e.Creator.Student) //needed
                .Include(e => e.ProjectUsers) //needed
                .Include(e => e.Bugs)  //needed
                .Include(e => e.Bugs.Select(e2 => e2.Severity)) //needed
                .Include(e => e.Bugs.Select(e2 => e2.Status)); //needed

            return AddBugsAndMembersToProjectQuery(query, withBugsAndMembers).First(e => e.Id == id);
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
	            .Where(e => e.ProjectUsers.Any(e2 => e2.UserId == userId))
                .Where(e => searchString == null || e.Title.Contains(searchString))
	            .Include(e => e.Creator.Lecturer)  //needed
                .Include(e => e.Creator.Student) //needed
                .Include(e => e.ProjectUsers);  //needed
        }

        private IQueryable<Project> GetUserProjectParticipationsQuery(LmPlatformModelsContext context, int userId, string searchString)
        {
            return GetUserProjectsQuery(context, userId, searchString)
                .Include(e => e.ProjectUsers.Select(e2 => e2.ProjectRole)); //needed
        }

        private IQueryable<Project> GetUserProjectParticipationsCountQuery(LmPlatformModelsContext context, int userId, string searchString)
        {
	        return context.Set<Project>()
		        .Where(e => e.ProjectUsers.Any(e2 => e2.UserId == userId))
		        .Where(e => searchString == null || e.Title.Contains(searchString));
        }

        private IQueryable<Student> GetStudentsGroupParticipationsQuery(LmPlatformModelsContext context, int groupId)
        {
            return context.Set<Student>()
                .Where(e => e.GroupId == groupId)
                .Where(e => e.User.ProjectUsers.Count > 0)
                .Include(e => e.User.ProjectUsers.Select(p => p.Project)) //needed
                .Include(e => e.User.ProjectUsers.Select(p => p.Project.Creator/*.Lecturer*/)) //needed
                .Include(e => e.User.ProjectUsers.Select(e2 => e2.ProjectRole)); //needed
        }

        private IQueryable<Student> GetStudentsGroupParticipationsCountQuery(LmPlatformModelsContext context, int groupId)
        {
            return context.Set<Student>()
                .Where(e => e.GroupId == groupId)
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
            }

            return query;
        }

        private IQueryable<Project> GetUserProjectsSortedQuery(IQueryable<Project> query, string sortingPropertyName, bool desc)
        {
	        return (sortingPropertyName, desc) switch
	        {
				("UserQuentity", true) => query.OrderByDescending(e => e.ProjectUsers.Count),
				("UserQuentity", false) => query.OrderBy(e => e.ProjectUsers.Count),
                ("Title", true) => query.OrderByDescending(e => e.Title),
				("Title", false) => query.OrderBy(e => e.Title),
                ("CreatorName", true) => query.OrderByDescending(e => e.Creator.Student == null ? e.Creator.Lecturer.LastName : e.Creator.Student.LastName),
				("CreatorName", false) => query.OrderBy(e => e.Creator.Student == null ? e.Creator.Lecturer.LastName : e.Creator.Student.LastName),
                (_, true) => query.OrderByDescending(e => e.DateOfChange),
                (_, false) => query.OrderBy(e => e.DateOfChange)
	        };
        }
    }
}
