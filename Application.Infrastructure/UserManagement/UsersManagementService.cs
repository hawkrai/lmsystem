using System.Collections.Generic;

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
            return UsersRepository.GetAll().Single(e => e.UserName == userName);
        }

        public bool IsExistsUser(string userName)
        {
            if (UsersRepository.GetAll().Any(e => e.UserName == userName))
            {
                return true;
            }
            return false;
        }
    }
}
