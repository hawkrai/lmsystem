using System.Collections.Generic;

namespace Application.Infrastructure.UserManagement
{
    using System;

    using LMPlatform.Models;

    public interface IUsersManagementService
	{
        User GetUser(string userName);

        User GetUser(int id);

        bool IsExistsUser(string userName);

        User CurrentUser { get; }

        bool DeleteUser(int id);

        User GetAdmin();

        IEnumerable<User> GetUsers(bool includeRole = false);

        void UpdateLastLoginDate(string userName);
	}
}