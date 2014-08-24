using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.AccountManagement;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.LecturerManagement;
using Application.Infrastructure.StudentManagement;

namespace LMPlatform.UI.ViewModels.AccountViewModels
{
    using System.Globalization;
    using System.Web.Mvc;

    using Application.Infrastructure.UserManagement;

    using LMPlatform.Data.Repositories.RepositoryContracts;
    using LMPlatform.Models;

    using WebMatrix.WebData;

    public class RegisterViewModel
    {
        private readonly LazyDependency<IAccountManagementService> _accountRegistrationService = new LazyDependency<IAccountManagementService>();
        private readonly LazyDependency<IGroupManagementService> _groupManagementService = new LazyDependency<IGroupManagementService>();
        private readonly LazyDependency<IStudentsRepository> _studentsRepository = new LazyDependency<IStudentsRepository>();
        private readonly LazyDependency<IStudentManagementService> _studentManagementService = new LazyDependency<IStudentManagementService>();
        private readonly LazyDependency<IUsersManagementService> _usersManagementService = new LazyDependency<IUsersManagementService>();
        private readonly LazyDependency<ILecturerManagementService> _lecturerManagementService = new LazyDependency<ILecturerManagementService>();

        public IStudentsRepository StudentsRepository
        {
            get
            {
                return _studentsRepository.Value;
            }
        }

        public IStudentManagementService StudentManagementService
        {
            get
            {
                return _studentManagementService.Value;
            }
        }

        public ILecturerManagementService LecturerManagementService
        {
            get
            {
                return _lecturerManagementService.Value;
            }
        }

        public IGroupManagementService GroupManagementService
        {
            get
            {
                return _groupManagementService.Value;
            }
        }

        public IAccountManagementService AccountRegistrationService
        {
            get
            {
                return _accountRegistrationService.Value;
            }
        }

        public IUsersManagementService UsersManagementService
        {
            get
            {
                return _usersManagementService.Value;
            }
        }

        [StringLength(50, ErrorMessage = "Имя не может иметь размер больше 50 символов")]
        [DataType(DataType.Text)]
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        public string Name
        {
            get;
            set;
        }

        [StringLength(50, ErrorMessage = "Фамилия не может иметь размер больше 50 символов")]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Поле Фамилия обязательно для заполнения")]
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

        [Display(Name = "Секретарь")]
        public bool IsSecretary { get; set; }

        [Display(Name = "Руководитель дипломных проектов")]
        public bool IsLecturerHasGraduateStudents { get; set; }

        [Required(ErrorMessage = "Поле Логин обязательно для заполнения")]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Поле Пароль обязательно для заполнения")]
        [StringLength(100, ErrorMessage = "T{0} должно быть не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
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

        public IList<SelectListItem> GetGroups()
        {
            var groups = GroupManagementService.GetGroups();

            return groups.Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString(CultureInfo.InvariantCulture)
            }).ToList();
        }

        public void RegistrationUser(IList<string> roles)
        {
            AccountRegistrationService.CreateAccount(UserName, Password, roles);
            if (roles.Contains("student"))
            {
                SaveStudent();
            }

            if (roles.Contains("lector"))
            {
                SaveLecturer();
            }
        }

        private void SaveStudent()
        {
            var user = UsersManagementService.GetUser(UserName);
            StudentManagementService.Save(new Student
            {
                Id = user.Id,
                FirstName = Name,
                LastName = Surname,
                MiddleName = Patronymic,
                GroupId = int.Parse(Group)
            });
        }

        private void SaveLecturer()
        {
            var user = UsersManagementService.GetUser(UserName);
            LecturerManagementService.Save(new Lecturer
            {
                Id = user.Id,
                FirstName = Name,
                LastName = Surname,
                MiddleName = Patronymic,
                IsSecretary = IsSecretary,
                IsLecturerHasGraduateStudents = IsLecturerHasGraduateStudents
            });
        }
    }
}