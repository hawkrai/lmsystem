using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Application.Core;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.StudentManagement;
using LMPlatform.Models;
using LMPlatform.UI.Attributes;
using WebMatrix.WebData;

namespace LMPlatform.UI.ViewModels.AdministrationViewModels
{
	[PasswordRequiredIfReset]
	public class ModifyStudentViewModel
	{
		private readonly LazyDependency<IGroupManagementService> _groupManagementService =
			new LazyDependency<IGroupManagementService>();

		private readonly LazyDependency<IStudentManagementService> _studentManagementService =
			new LazyDependency<IStudentManagementService>();

		private IStudentManagementService StudentManagementService => this._studentManagementService.Value;

		private IGroupManagementService GroupManagementService => this._groupManagementService.Value;

		public ModifyStudentViewModel() { }

		public ModifyStudentViewModel(Student student)
		{
			if (student != null)
			{
				this.Group = student.GroupId == 0
					? this.StudentManagementService.GetStudent(student.Id).GroupId
						.ToString(CultureInfo.InvariantCulture)
					: student.GroupId.ToString(CultureInfo.InvariantCulture);

				this.Id = student.Id;
				this.Name = student.FirstName;
				this.Surname = student.LastName;
				this.Patronymic = student.MiddleName;
				this.Email = student.Email;
				this.UserName = student.User.UserName;
				this.Avatar = student.User.Avatar;
				this.SkypeContact = student.User.SkypeContact;
				this.Phone = student.User.Phone;
				this.About = student.User.About;
				this.Email = student.User.Email;
			}
		}

		public string Avatar { get; set; }

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

		[Editable(false)]
		[Display(Name = "Логин")]
		public string UserName { get; set; }

		[Display(Name = "Сбросить пароль")]
		public bool IsPasswordReset { get; set; }

		[StringLength(100, ErrorMessage = "Пароль должен быть не менее {2} символов.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Новый пароль")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Подтверждение пароля")]
		[System.ComponentModel.DataAnnotations.Compare("Password",
			ErrorMessage = "Пароль и подтвержденный пароль не совпадают.")]
		public string ConfirmPassword { get; set; }

		[Display(Name = "Группа")]
		public string Group { get; set; }

		[Display(Name = "Эл. почта")]
		public string Email { get; set; }

		[Editable(false)]
		public int Id { get; set; }

		public string FullName => $"{this.Name} {this.Surname} ({this.UserName})";

		public string SkypeContact { get; set; }

		public string Phone { get; set; }

		public string About { get; set; }

		public IList<SelectListItem> GetGroups()
		{
			var groups = this.GroupManagementService.GetGroups();

			return groups.Select(v => new SelectListItem
			{
				Text = v.Name,
				Value = v.Id.ToString(CultureInfo.InvariantCulture)
			}).ToList();
		}

		public void ModifyStudent()
		{
			var groupId = int.Parse(this.Group);

			this.StudentManagementService.UpdateStudent(
				new Student
				{
					Id = this.Id,
					FirstName = this.Name,
					LastName = this.Surname,
					MiddleName = this.Patronymic,
					Email = this.Email,
					Confirmed = true,
					GroupId = groupId,
					Group = this.GroupManagementService.GetGroup(groupId),
					User = new User()
					{
						Avatar = this.Avatar,
						UserName = this.UserName,
						About = this.About,
						SkypeContact = this.SkypeContact,
						Phone = this.Phone,
						Email = this.Email,
						Id = this.Id
					}
				});
		}

		public bool ResetPassword()
		{
			var token = WebSecurity.GeneratePasswordResetToken(this.UserName, 1);
			var result = WebSecurity.ResetPassword(token, this.Password);
			return result;
		}
	}
}