using Application.Core.Data;
using WebMatrix.WebData;

namespace Application.Infrastructure.UserManagement
{
    using System.Linq;
    using System.Web.Security;

    using Application.Core;
    using Application.Core.Constants;
    using Application.Infrastructure.AccountManagement;

    using LMPlatform.Data.Repositories;
    using LMPlatform.Data.Repositories.RepositoryContracts;
    using LMPlatform.Models;

    public class UsersManagementService : IUsersManagementService
    {
        private readonly LazyDependency<IUsersRepository> _usersRepository = new LazyDependency<IUsersRepository>();
        private readonly LazyDependency<IAccountManagementService> _accountManagementService = new LazyDependency<IAccountManagementService>();

        public IUsersRepository UsersRepository
        {
            get
            {
                return _usersRepository.Value;
            }
        }

        public IAccountManagementService AccountManagementService
        {
            get
            {
                return _accountManagementService.Value;
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

        public void DeleteUser(int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var user = repositoriesContainer.UsersRepository.GetBy(new Query<User>().AddFilterClause(u => u.Id == id));
                repositoriesContainer.MessageRepository.DeleteUserMessage(user.Id);
                AccountManagementService.DeleteAccount(user.UserName);
                repositoriesContainer.ApplyChanges();
            }
        }

        public User GetAdmin()
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var admin = Roles.GetUsersInRole(Constants.Roles.Admin);
                if (admin.Length <= 0)
                {
                    return null;
                }

                var adminName = admin[0];
                var user = repositoriesContainer.UsersRepository.GetBy(new Query<User>().AddFilterClause(u => u.UserName == adminName));
                return user;
            }
        }
    }
}
