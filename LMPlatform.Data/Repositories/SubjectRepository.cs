using System.Collections.Generic;
using System.Data.Entity;
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
    }
}