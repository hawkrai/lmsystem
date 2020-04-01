using System.Collections.Generic;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;
using System.Data.Entity;

namespace LMPlatform.Data.Repositories
{
    public class BugsRepository : RepositoryBase<LmPlatformModelsContext, Bug>, IBugsRepository
    {
        public BugsRepository(LmPlatformModelsContext dataContext) : base(dataContext)
        {      
        }

        public List<Bug> GetUserBugs(int userId, int limit, int offset, string searchString, string sortedPropertyName, bool desc = false)
        {
	        using var context = new LmPlatformModelsContext();
	        var query = GetUserBugsQuery(context, userId, searchString);
	        return GetPagebleSortedBugs(query, sortedPropertyName, desc, limit, offset);
        }

        public int GetUserBugsCount(int userId, string searchString)
        {
	        using var context = new LmPlatformModelsContext();
	        return GetUserBugsQuery(context, userId, searchString).Count();
        }

        public List<Bug> GetProjectBugs(int projectId, int limit, int offset, string searchString, string sortedPropertyName, bool desc = false)
        {
            using(var context = new LmPlatformModelsContext())
            {
                var query = GetProjectBugsQuery(context, projectId, searchString);
                return GetPagebleSortedBugs(query, sortedPropertyName, desc, limit, offset);
            }
        }

        public int GetProjectBugsCount(int projectId, string searchString)
        {
	        using var context = new LmPlatformModelsContext();
	        return GetProjectBugsQuery(context, projectId, searchString).Count();
        }

        public void DeleteBug(Bug bug)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var model = context.Set<Bug>().FirstOrDefault(e => e.Id == bug.Id);
                context.Delete(model);

                context.SaveChanges();
            }
        }

        public Bug SaveBug(Bug bug)
        {
            DataContext.Set<Bug>().Add(bug);

            return bug;
        }

        private List<Bug> GetPagebleSortedBugs(IQueryable<Bug> query, string sortedPropertyName, bool desc, int limit, int offset)
        {
            return GetSortedBugsQuery(query, sortedPropertyName, desc)
                .Skip(offset)
                .Take(limit)
                .ToList();
        }

        private IQueryable<Bug> GetUserBugsQuery(LmPlatformModelsContext context, int userId, string searchString)
        {
            return GetBugsQuery(context, searchString).Where(e => e.Project.ProjectUsers.Any(e2 => e2.UserId == userId));
        }

        private IQueryable<Bug> GetProjectBugsQuery(LmPlatformModelsContext context, int projectId, string searchString)
        {
            return GetBugsQuery(context, searchString).Where(e => e.Project.Id == projectId);
        }

        private IQueryable<Bug> GetBugsQuery(LmPlatformModelsContext context, string searchString)
        {
            return context.Set<Bug>()
                .Include(e => e.Severity)
                .Include(e => e.Status)
                .Include(e => e.Project.Creator.Lecturer)
                .Include(e => e.Project.Creator.Student)
                .Include(e => e.Project)
                .Include(e => e.Reporter.Student)
                .Where(e => searchString == null 
                            || e.Summary.Contains(searchString) 
                            || e.Project.Title.Contains(searchString) 
                            || e.Reporter.Student.LastName.Contains(searchString)
                );
        }

        private IQueryable<Bug> GetSortedBugsQuery(IQueryable<Bug> query, string sortingPropertyName, bool desc)
        {
	        return (sortingPropertyName, desc) switch
	        {
		        ("Id", true) => query.OrderByDescending(e => e.Id),
		        ("Id", false) => query.OrderBy(e => e.Id),
		        ("Summary", true) => query.OrderByDescending(e => e.Summary),
		        ("Summary", false) => query.OrderBy(e => e.Summary),
		        ("ProjectTitle", true) => query.OrderByDescending(e => e.Project.Title),
		        ("ProjectTitle", false) => query.OrderBy(e => e.Project.Title),
		        ("Severity", true) => query.OrderByDescending(e => e.Severity.Name),
		        ("Severity", false) => query.OrderBy(e => e.Project.Title),
		        ("Status", true) => query.OrderByDescending(e => e.Status.Name),
		        ("Status", false) => query.OrderBy(e => e.Status.Name),
		        (_, true) => query.OrderByDescending(e => e.ModifyingDate),
		        (_, false) => query.OrderBy(e => e.ModifyingDate)
	        };
        }
    }
}
