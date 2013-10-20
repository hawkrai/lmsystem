using System.ComponentModel.DataAnnotations;
using Application.Core;
using Application.Infrastructure.AccountManagement;

namespace LMPlatform.UI.ViewModels.AccountViewModels
{
	public class LoginViewModel
	{
		private readonly LazyDependency<IAccountManagementService> _accountAuthenticationService = new LazyDependency<IAccountManagementService>();

		[Display(Name = "Логин")]
        [Required(ErrorMessage = "Поле Логин обязательно для заполнения")]
		public string UserName
		{
			get;
			set;
		}

		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
        [Required(ErrorMessage = "Поле Пароль обязательно для заполнения")]
		public string Password
		{
			get;
			set;
		}

		[Display(Name = "Запомнить меня?")]
		public bool RememberMe
		{
			get;
			set;
		}

		public IAccountManagementService AccountAuthenticationService
		{
			get
			{
				return _accountAuthenticationService.Value;
			}
		}

		public bool Login()
		{
			return AccountAuthenticationService.Login(UserName, Password, RememberMe);
		}

		public void LogOut()
		{
			AccountAuthenticationService.Logout();
		}
	}
}