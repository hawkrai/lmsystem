using System.ComponentModel.DataAnnotations;
using Application.Core;
using Application.Infrastructure.LecturerManagement;
using Application.Infrastructure.StudentManagement;
using Application.Infrastructure.UserManagement;
using WebMatrix.WebData;

namespace LMPlatform.UI.ViewModels.AccountViewModels
{
    public class PersonalDataViewModel
    {
        private readonly LazyDependency<IStudentManagementService> _studentManagementService = new LazyDependency<IStudentManagementService>();
        private readonly LazyDependency<ILecturerManagementService> _lecturerManagementService = new LazyDependency<ILecturerManagementService>();

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

        [Required(ErrorMessage = "Поле Логин обязательно для заполнения")]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

		public string Avatar { get; set; }

		[Display(Name = "Skype")]
		public string SkypeContact { get; set; }

		[Display(Name = "Почта")]
		public string Email { get; set; }

		[Display(Name = "Телефон")]
		public string Phone { get; set; }

		[Display(Name = "О себе")]
		public string About { get; set; }

		public bool IsSecretary { get; set; }

		public bool IsLecturerHasGraduateStudents { get; set; }

        public PersonalDataViewModel()
        {
            if (LecturerManagementService.GetLecturer(WebSecurity.CurrentUserId) != null)
            {
                var user = LecturerManagementService.GetLecturer(WebSecurity.CurrentUserId);
                Name = user.FirstName;
				Surname = user.LastName;
				Patronymic = user.MiddleName;
                UserName = user.User.UserName;
	            Avatar = user.User.Avatar;
	            IsSecretary = user.IsSecretary;
	            IsLecturerHasGraduateStudents = user.IsLecturerHasGraduateStudents;
            }
            else
            {
                var user = StudentManagementService.GetStudent(WebSecurity.CurrentUserId);
                Name = user.FirstName;
				Patronymic = user.MiddleName;
				Surname = user.LastName;
                UserName = user.User.UserName;
				Avatar = user.User.Avatar;
            }
        }
    }
}