using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Application.Core;
using Application.Infrastructure.LecturerManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;
using LMPlatform.UI.Attributes;

namespace LMPlatform.UI.ViewModels.AdministrationViewModels
{
    [PasswordRequiredIfResetAttribute]
    public class ModifyLecturerViewModel
    {
        private readonly LazyDependency<ISubjectManagementService> _subjectManagementService = new LazyDependency<ISubjectManagementService>();
        private readonly LazyDependency<ILecturerManagementService> _lecturerManagementService = new LazyDependency<ILecturerManagementService>();

        public ILecturerManagementService LecturerManagementService
        {
            get
            {
                return _lecturerManagementService.Value;
            }
        }

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return _subjectManagementService.Value;
            }
        }

        public int LecturerId { get; set; }

        public ModifyLecturerViewModel()
        {
        }

        public ModifyLecturerViewModel(Lecturer lecturer)
        {
            if (lecturer != null)
            {
                LecturerId = lecturer.Id;
                Name = lecturer.FirstName;
                Surname = lecturer.LastName;
                Patronymic = lecturer.MiddleName;
                UserName = lecturer.User.UserName;
            }
        }

        [StringLength(50, ErrorMessage = "Имя не может иметь размер больше 50 символов")]
        [DataType(DataType.Text)]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Фамилия не может иметь размер больше 50 символов")]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия")]
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
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароль и подтвержденный пароль не совпадают.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Эл. почта")]
        public string Email { get; set; }

        public string FullName
        {
            get { return string.Format("{0} {1} {2}", Surname, Name, Patronymic); }
        }

        public IList<SelectListItem> GetSubjects()
        {
            return null;
        }

        public void ModifyLecturer()
        {
            LecturerManagementService.UpdateLecturer(new Lecturer
            {
                Id = LecturerId,
                FirstName = Name,
                LastName = Surname,
                MiddleName = Patronymic,
            });
        }
    }
}