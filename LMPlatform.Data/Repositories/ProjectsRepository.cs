using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Application.Core.Data;
using Application.Core.Extensions;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;
using System.Linq.Expressions;
using System;
using System.Reflection;

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

        public Project GetProjectWithData(int id)
        {
            using(var context = new LmPlatformModelsContext())
            {   
                return context.Set<Project>()
                    .Include(e => e.Creator.Lecturer)
                    .Include(e => e.Creator.Student)
                    .Include(e => e.ProjectUsers)
                    .First(e => e.Id == id);
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
