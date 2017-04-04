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
            using(var context = new LmPlatformModelsContext())
            {
                var query = GetUserBugsQuery(context, userId, searchString);
                return GetUserBugsSortedQuery(query, sortedPropertyName, desc)
                    .Skip(offset)
                    .Take(limit)
                    .ToList();
            }
        }

        public int GetUserBugsCount(int userId, string searchString)
        {
            using(var context = new LmPlatformModelsContext())
            {
                return GetUserBugsQuery(context, userId, searchString).Count();
            }
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

        private IQueryable<Bug> GetUserBugsQuery(LmPlatformModelsContext context, int userId, string searchString)
        {
            return context.Set<Bug>()
                .Include(e => e.Severity)
                .Include(e => e.Status)
                .Include(e => e.Project.Creator.Lecturer)
                .Include(e => e.Project.Creator.Student)
                .Include(e => e.Project.Title)
                .Where(e => e.Project.ProjectUsers.Any(e2 => e2.UserId == userId))
                .Where(e => searchString == null ? true : e.Summary.Contains(searchString));
        }

        private IQueryable<Bug> GetUserBugsSortedQuery(IQueryable<Bug> query, string sortingPropertyName, bool desc)
        {
            switch(sortingPropertyName)
            {
                //case "UserQuentity":
                //if(desc)
                //    return query.OrderByDescending(e => e.ProjectUsers.Count);
                //else
                //    return query.OrderBy(e => e.ProjectUsers.Count);
                //case "Title":
                //if(desc)
                //    return query.OrderByDescending(e => e.Title);
                //else
                //    return query.OrderBy(e => e.Title);
                //case "CreatorName":
                //if(desc)
                //    return query.OrderByDescending(e => e.Creator.Student == null ? e.Creator.Lecturer.LastName : e.Creator.Student.LastName);
                //else
                //    return query.OrderBy(e => e.Creator.Student == null ? e.Creator.Lecturer.LastName : e.Creator.Student.LastName);
                default:
                    if(desc)
                        return query.OrderByDescending(e => e.ModifyingDate);
                    else
                        return query.OrderBy(e => e.ModifyingDate);
            }
        }
    }
}
