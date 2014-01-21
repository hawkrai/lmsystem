using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories
{
    public class SubjectRepository : RepositoryBase<LmPlatformModelsContext, Subject>, ISubjectRepository
    {
        public SubjectRepository(LmPlatformModelsContext dataContext)
            : base(dataContext)
        {
        }

        public List<Subject> GetSubjects(int groupId = 0, int lecturerId = 0)
        {
            using (var context = new LmPlatformModelsContext())
            {
                if (groupId != 0)
                {
                    var subjectGroup = context.Set<SubjectGroup>().Include(e => e.Subject).Where(e => e.GroupId == groupId).ToList();
                    return subjectGroup.Select(e => e.Subject).ToList(); 
                }
                else
                {
                    var subjectLecturer =
                        context.Set<SubjectLecturer>().Include(e => e.Subject).Where(e => e.LecturerId == lecturerId);
                    return subjectLecturer.Select(e => e.Subject).ToList();
                }
            } 
        }

        public SubjectNews SaveNews(SubjectNews news)
        {
            using (var context = new LmPlatformModelsContext())
            {
                if (news.Id != 0)
                {
                    context.Update(news);    
                }
                else
                {
                    context.Add(news);
                }
                
                context.SaveChanges();
            }

            return news;
        }

        protected override void PerformAdd(Subject model, LmPlatformModelsContext dataContext)
        {
            base.PerformAdd(model, dataContext);

            foreach (var subjectModule in model.SubjectModules)
            {
                subjectModule.SubjectId = model.Id;
                dataContext.Set<SubjectModule>().Add(subjectModule);
            }

            model.SubjectLecturers.FirstOrDefault().SubjectId = model.Id;
            dataContext.Set<SubjectLecturer>().Add(model.SubjectLecturers.FirstOrDefault());

            dataContext.SaveChanges();
        }

        protected override void PerformUpdate(Subject newValue, LmPlatformModelsContext dataContext)
        {
            var subjectModules = dataContext.Set<SubjectModule>().Where(e => e.SubjectId == newValue.Id).ToList();

            foreach (var subjectModule in subjectModules)
            {
                if (!newValue.SubjectModules.Any(e => e.ModuleId == subjectModule.ModuleId))
                {
                    dataContext.Set<SubjectModule>().Remove(subjectModule);
                }
            }

            foreach (var subjectModule in newValue.SubjectModules)
            {
                if (!subjectModules.Any(e => e.ModuleId == subjectModule.ModuleId))
                {
                    dataContext.Set<SubjectModule>().Add(subjectModule);
                }
            }

            dataContext.SaveChanges();

            base.PerformUpdate(newValue, dataContext);
        }
    }
}