using System.Data.Entity;
using System.Linq;
using Application.Core;
using Application.Infrastructure.CTO;
using LMPlatform.Data.Infrastructure;

namespace Application.Infrastructure.CPManagement
{
    public class CPUserService : ICPUserService
    {
        public UserData GetUserInfo(int userId)
        {
            var user = Context.Users.Include(x => x.Student).Include(x => x.Lecturer).Single(x => x.Id == userId);

            return new UserData
            {
                UserId = user.Id,
                IsLecturer = user.Lecturer != null,
                IsStudent = user.Student != null,
                IsSecretary = user.Lecturer != null && user.Lecturer.IsSecretary,
                HasChosenDiplomProject = user.Student != null 
                    && Context.AssignedCourseProjects.Any(x => x.StudentId == user.Student.Id && !x.ApproveDate.HasValue),
                HasAssignedDiplomProject = user.Student != null 
                    && Context.AssignedCourseProjects.Any(x => x.StudentId == user.Student.Id && x.ApproveDate.HasValue)
            };
        }

        private readonly LazyDependency<ICpContext> context = new LazyDependency<ICpContext>();

        private ICpContext Context
        {
            get { return context.Value; }
        }
    }
}