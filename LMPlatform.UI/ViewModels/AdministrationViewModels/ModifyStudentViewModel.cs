using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.StudentManagement;
using LMPlatform.Models;
using LMPlatform.UI.Attributes;
using WebMatrix.WebData;

namespace LMPlatform.UI.ViewModels.AdministrationViewModels
{
    [PasswordRequiredIfResetAttribute]
    public class ModifyStudentViewModel
    {
        private readonly LazyDependency<IGroupManagementService> _groupManagementService = new LazyDependency<IGroupManagementService>();
        private readonly LazyDependency<IStudentManagementService> _studentManagementService = new LazyDependency<IStudentManagementService>();

        public IStudentManagementService StudentManagementService
        {
            get
            {
                return _studentManagementService.Value;
            }
        }

        public IGroupManagementService GroupManagementService
        {
            get
            {
                return _groupManagementService.Value;
            }
        }

        [StringLength(50, ErrorMessage = "Имя не может иметь размер больше 50 символов")]
        [DataType(DataType.Text)]
        [Display(Name = "Имя")]
        public string Name
        {
            get;
            set;
        }

        [StringLength(50, ErrorMessage = "Фамилия не может иметь размер больше 50 символов")]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия")]
        public string Surname
        {
            get;
            set;
        }

        [StringLength(50, ErrorMessage = "Отчество не может иметь размер больше 50 символов")]
        [DataType(DataType.Text)]
        [Display(Name = "Отчество")]
        public string Patronymic
        {
            get;
            set;
        }

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
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароль и подтвержденный пароль не совпадают.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Группа")]
        public string Group
        {
            get;
            set;
        }

        [Display(Name = "Эл. почта")]
        public string Email
        {
            get;
            set;
        }

        public string FullName
        {
            get { return string.Format("{0} {1} ({2})", Name, Surname, UserName); }
        }

        public IList<SelectListItem> GetGroups()
        {
            var groups = GroupManagementService.GetGroups();

            return groups.Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString(CultureInfo.InvariantCulture)
            }).ToList();
        }

        public void ModifyStudent(int studentId)
        {
            StudentManagementService.UpdateStudent(new Student
              {
                  Id = studentId,
                  FirstName = Name,
                  LastName = Surname,
                  MiddleName = Patronymic,
                  Email = Email,
                  GroupId = int.Parse(Group),
              });
        }

        public bool ResetPassword()
        {
            var token = WebSecurity.GeneratePasswordResetToken(UserName, 1);
            var result = WebSecurity.ResetPassword(token, Password);
            return result;
        }
    }
}