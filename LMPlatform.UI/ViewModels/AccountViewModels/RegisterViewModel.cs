using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Application.Core;
using Application.Infrastructure.AccountManagement;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.LecturerManagement;
using Application.Infrastructure.StudentManagement;
using Application.Infrastructure.UserManagement;
using Application.SearchEngine.SearchMethods;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.AccountViewModels
{
	public class RegisterViewModel
	{
		private readonly LazyDependency<IAccountManagementService> _accountRegistrationService =
			new LazyDependency<IAccountManagementService>();

		private readonly LazyDependency<IGroupManagementService> _groupManagementService =
			new LazyDependency<IGroupManagementService>();

		private readonly LazyDependency<ILecturerManagementService> _lecturerManagementService =
			new LazyDependency<ILecturerManagementService>();

		private readonly LazyDependency<IStudentManagementService> _studentManagementService =
			new LazyDependency<IStudentManagementService>();

		private readonly LazyDependency<IStudentsRepository> _studentsRepository =
			new LazyDependency<IStudentsRepository>();

		private readonly LazyDependency<IUsersManagementService> _usersManagementService =
			new LazyDependency<IUsersManagementService>();

		private IStudentsRepository StudentsRepository => _studentsRepository.Value;

		private IStudentManagementService StudentManagementService => _studentManagementService.Value;

		private ILecturerManagementService LecturerManagementService => _lecturerManagementService.Value;

		private IGroupManagementService GroupManagementService => _groupManagementService.Value;

		private IAccountManagementService AccountRegistrationService => _accountRegistrationService.Value;

		private IUsersManagementService UsersManagementService => _usersManagementService.Value;

		[StringLength(50, ErrorMessage = "Имя не может иметь размер больше 50 символов")]
		[DataType(DataType.Text)]
		[Display(Name = "Имя")]
		[Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
		public string Name { get; set; }

		[StringLength(50, ErrorMessage = "Фамилия не может иметь размер больше 50 символов")]
		[DataType(DataType.Text)]
		[Display(Name = "Фамилия")]
		[Required(ErrorMessage = "Поле Фамилия обязательно для заполнения")]
		public string Surname { get; set; }

		[StringLength(50, ErrorMessage = "Отчество не может иметь размер больше 50 символов")]
		[DataType(DataType.Text)]
		[Display(Name = "Отчество")]
		public string Patronymic { get; set; }

		[Display(Name = "Секретарь")] public bool IsSecretary { get; set; }

		[Display(Name = "Руководитель дипломных проектов")]
		public bool IsLecturerHasGraduateStudents { get; set; }

		[Required(ErrorMessage = "Поле Логин обязательно для заполнения")]
		[Display(Name = "Логин")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Поле Пароль обязательно для заполнения")]
		[StringLength(100, ErrorMessage = "{0} должно быть не менее {2} символов.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Подтверждение пароля")]
		[Required(ErrorMessage = "Поле Подтверждение пароля обязательно для заполнения")]
		[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage =
			"Пароль и подтвержденный пароль не совпадают.")]
		public string ConfirmPassword { get; set; }

		[Display(Name = "Группа")]
		public string Group { get; set; }

		[Display(Name = "Код доступа")]
		public string Code { get; set; }

		public IList<SelectListItem> GetGroups()
		{
			var groups = GroupManagementService.GetGroups();

			return groups.Select(v => new SelectListItem
			{
				Text = v.Name,
				Value = v.Id.ToString(CultureInfo.InvariantCulture)
			}).OrderBy(e => e.Text).ToList();
		}

		public void RegistrationUser(IList<string> roles)
		{
			AccountRegistrationService.CreateAccount(UserName, Password, roles);
			if (roles.Contains("student")) SaveStudent();

			if (roles.Contains("lector")) SaveLecturer();
		}

		private void SaveStudent()
		{
			var user = UsersManagementService.GetUser(UserName);
			var student = StudentManagementService.Save(new Student
			{
				Id = user.Id,
				FirstName = Name,
				LastName = Surname,
				MiddleName = Patronymic,
				GroupId = int.Parse(Group),
				Confirmed = false
			});
			student.User = user;
			student.Group = GroupManagementService.GetGroup(student.GroupId);
			new StudentSearchMethod().AddToIndex(student);
		}

		private void SaveLecturer()
		{
			var user = UsersManagementService.GetUser(UserName);

			var lecturer = LecturerManagementService.Save(
				new Lecturer
				{
					Id = user.Id,
					FirstName = Name,
					LastName = Surname,
					MiddleName = Patronymic,
					IsSecretary = IsSecretary,
					IsLecturerHasGraduateStudents = IsLecturerHasGraduateStudents,
					IsActive = true
				});

			lecturer.User = user;

			new LecturerSearchMethod().AddToIndex(lecturer);
		}
	}
}