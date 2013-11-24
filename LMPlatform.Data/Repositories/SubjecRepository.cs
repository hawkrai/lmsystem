using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories
{
    public class SubjecRepository : RepositoryBase<LmPlatformModelsContext, Subject>, ISubjectRepository
    {
        public SubjecRepository(LmPlatformModelsContext dataContext) : base(dataContext)
        {
        }

        public List<Subject> GetSubjects(int gorupId)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var subjectGroup = context.Set<SubjectGroup>().Include(e => e.Subject).Where(e => e.GroupId == gorupId).ToList();
                var subjects = subjectGroup.Select(e => e.Subject).ToList();
                return subjects;
            } 
        }
    }
}