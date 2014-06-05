using System.Collections.Generic;

namespace Application.Infrastructure.AccountManagement
{
	public interface IAccountManagementService
	{
		bool Login(string login, string password, bool remembeMe);

		void Logout();

		void CreateAccount(string login, string password, IList<string> roles, object properties = null, bool requireConfirmationToken = false);

		bool ChangePassword(string userName, string oldPassword, string newPassword);

	    bool DeleteAccount(string login);
	}
}