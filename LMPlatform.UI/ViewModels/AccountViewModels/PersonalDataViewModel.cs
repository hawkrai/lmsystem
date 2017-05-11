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
		[Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Имя")]
        public string Name
        {
            get;
            set;
        }

        [StringLength(50, ErrorMessage = "Фамилия не может иметь размер больше 50 символов")]
        [DataType(DataType.Text)]
		[Required(ErrorMessage = "Поле Фамилия обязательно для заполнения")]
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

		[Display(Name = "Должность")]
		public string Skill { get; set; }

		public bool IsSecretary { get; set; }

		public bool IsLecturerHasGraduateStudents { get; set; }

        public PersonalDataViewModel()
        {
            if (LecturerManagementService.GetLecturer(WebSecurity.CurrentUserId) != null)
            {
                var user = LecturerManagementService.GetLecturer(WebSecurity.CurrentUserId);
                Name = user.FirstName;
	            Skill = user.Skill;
				Surname = user.LastName;
				Patronymic = user.MiddleName;
                UserName = user.User.UserName;
	            Avatar = user.User.Avatar;
	            IsSecretary = user.IsSecretary;
				About = user.User.About;
				SkypeContact = user.User.SkypeContact;
				Phone = user.User.Phone;
				Email = user.User.Email;
	            IsLecturerHasGraduateStudents = user.IsLecturerHasGraduateStudents;
            }
            else if (StudentManagementService.GetStudent(WebSecurity.CurrentUserId) != null)
            {
                var user = StudentManagementService.GetStudent(WebSecurity.CurrentUserId);
                Name = user.FirstName;
				Patronymic = user.MiddleName;
				Surname = user.LastName;
                UserName = user.User.UserName;
				Avatar = user.User.Avatar;
				About = user.User.About;
				SkypeContact = user.User.SkypeContact;
				Phone = user.User.Phone;
				Email = user.User.Email;
            }
        }
    }
}