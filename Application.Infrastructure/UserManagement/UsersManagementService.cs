using Application.Core.Data;
using WebMatrix.WebData;

namespace Application.Infrastructure.UserManagement
{
    using System.Linq;

    using Application.Core;

    using LMPlatform.Data.Repositories.RepositoryContracts;
    using LMPlatform.Models;

    public class UsersManagementService : IUsersManagementService
    {
        private readonly LazyDependency<IUsersRepository> _usersRepository = new LazyDependency<IUsersRepository>();

        public IUsersRepository UsersRepository
        {
            get
            {
                return _usersRepository.Value;
            }
        }

        public User GetUser(string userName)
        {
            if (IsExistsUser(userName))
            {
                return UsersRepository.GetAll(new Query<User>()
                    .Include(u => u.Student).Include(u => u.Lecturer).Include(u => u.Membership.Roles))
                    .Single(e => e.UserName == userName);
            }

            return null;
        }

        public bool IsExistsUser(string userName)
        {
            if (UsersRepository.GetAll().Any(e => e.UserName == userName))
            {
                return true;
            }

            return false;
        }

        public User CurrentUser
        {
            get
            {
                var userName = WebSecurity.CurrentUserName;

                return GetUser(userName);
            }
        }
    }
}
