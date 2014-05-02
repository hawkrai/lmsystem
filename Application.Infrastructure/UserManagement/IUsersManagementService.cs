using System.Collections.Generic;

namespace Application.Infrastructure.UserManagement
{
    using LMPlatform.Models;

    public interface IUsersManagementService
	{
        User GetUser(string userName);

        bool IsExistsUser(string userName);

        User CurrentUser { get; }

        void DeleteUser(int id);

        User GetAdmin();

        void UpdateLastLoginDate(string userName);
	}
}