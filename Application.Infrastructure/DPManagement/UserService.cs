using System.Data.Entity;
using System.Linq;
using Application.Core;
using Application.Infrastructure.DTO;
using LMPlatform.Data.Infrastructure;

namespace Application.Infrastructure.DPManagement
{
    public class UserService : IUserService
    {
        public UserData GetUserInfo(int userId)
        {
            var user = Context.Users.Include(x => x.Student).Include(x => x.Lecturer).Single(x => x.Id == userId);

            return new UserData
            {
                IsLecturer = user.Lecturer != null,
                IsStudent = user.Student != null,
                IsSecretary = user.Lecturer != null && user.Lecturer.IsSecretary
            };
        }

        private readonly LazyDependency<IDpContext> context = new LazyDependency<IDpContext>();

        private IDpContext Context
        {
            get { return context.Value; }
        }
    }
}