using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Application.Core;
using Application.Core.Constants;
using Application.Core.Data;
using Application.Infrastructure.DPManagement;
using Application.Infrastructure.DTO;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.LecturerManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;
using LMPlatform.UI.Attributes;
using WebMatrix.WebData;

namespace LMPlatform.UI.ViewModels.AdministrationViewModels
{
    [PasswordRequiredIfResetAttribute]
    public class ModifyLecturerViewModel
    {
        private readonly LazyDependency<ISubjectManagementService> _subjectManagementService = new LazyDependency<ISubjectManagementService>();
        private readonly LazyDependency<ILecturerManagementService> _lecturerManagementService = new LazyDependency<ILecturerManagementService>();
        private readonly LazyDependency<IGroupManagementService> _groupManagementService = new LazyDependency<IGroupManagementService>();
        private readonly LazyDependency<ICorrelationService> _correlationService = new LazyDependency<ICorrelationService>();

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

        public IGroupManagementService GroupManagementService
        {
            get
            {
                return _groupManagementService.Value;
            }
        }

        public ICorrelationService CorrelationService
        {
            get
            {
                return _correlationService.Value;
            }
        }

        public int LecturerId { get; set; }

        public ModifyLecturerViewModel()
        {
            Groups = new MultiSelectList(new List<Correlation>(CorrelationService.GetCorrelation("Group", null)), "Id", "Name");
        }

		public string Avatar { get; set; }

        public ModifyLecturerViewModel(Lecturer lecturer)
        {
            if (lecturer != null)
            {
                LecturerId = lecturer.Id;
                Name = lecturer.FirstName;
                Surname = lecturer.LastName;
                Patronymic = lecturer.MiddleName;
                UserName = lecturer.User.UserName;
	            Avatar = lecturer.User.Avatar;
                IsSecretary = lecturer.IsSecretary;
                IsLecturerHasGraduateStudents = lecturer.IsLecturerHasGraduateStudents;

                var groups = CorrelationService.GetCorrelation("Group", lecturer.Id);
	            if (lecturer.SecretaryGroups != null)
	            {
					Groups = new MultiSelectList(groups, "Id", "Name", lecturer.SecretaryGroups.Select(x => x.Id).ToList());   
	            }
            }
        }
        
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
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароль и подтвержденный пароль не совпадают.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Эл. почта")]
        public string Email { get; set; }

        public string FullName
        {
            get { return string.Format("{0} {1} {2}", Surname, Name, Patronymic); }
        }

        [Display(Name = "Секретарь")]
        public bool IsSecretary { get; set; }

        [Display(Name = "Для выбора группы секретаря нажмите левой кнопкой мыши по соответствующему элементу в левом списке:")]
        public List<int> SelectedGroupIds { get; set; }

        public MultiSelectList Groups { get; set; }

        [Display(Name = "Руководитель дипломных проектов")]
        public bool IsLecturerHasGraduateStudents { get; set; }

        public IList<SelectListItem> GetSubjects()
        {
            return null;
        }

        public void ModifyLecturer()
        {
            var selectedGroups = SelectedGroupIds != null && SelectedGroupIds.Count > 0 ?
                GroupManagementService.GetGroups(new Query<Group>(x => SelectedGroupIds.Contains(x.Id))) :
                new List<Group>();

            foreach (var group in GroupManagementService.GetGroups(new Query<Group>(x => x.SecretaryId == LecturerId)))
            {
                group.SecretaryId = null;
                GroupManagementService.UpdateGroup(group);
            }

            if (IsSecretary)
            {
                foreach (var group in selectedGroups)
                {
                    group.SecretaryId = LecturerId;
                    GroupManagementService.UpdateGroup(group);
                }
            }

            LecturerManagementService.UpdateLecturer(new Lecturer
            {
                Id = LecturerId,
                FirstName = Name,
                LastName = Surname,
                MiddleName = Patronymic,
                IsSecretary = IsSecretary,
                IsLecturerHasGraduateStudents = IsLecturerHasGraduateStudents,
                SecretaryGroups = selectedGroups,
				User = new User()
				{
					Id = LecturerId,
					Avatar = Avatar,
					UserName = UserName,
				}
            });
        }
    }
}