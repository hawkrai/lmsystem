using System.Collections.Generic;

namespace Application.Infrastructure.UserManagement
{
    using LMPlatform.Models;

    public interface IUsersManagementService
	{
        User GetUser(string userName);

        bool IsExistsUser(string userName);
	}
}