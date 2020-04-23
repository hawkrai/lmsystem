using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.DPManagement;
using Application.Infrastructure.DTO;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.LecturerManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.AdministrationViewModels
{
    public class ModifyLecturerViewModel
    {
        private readonly LazyDependency<ISubjectManagementService> _subjectManagementService = new LazyDependency<ISubjectManagementService>();
        private readonly LazyDependency<ILecturerManagementService> _lecturerManagementService = new LazyDependency<ILecturerManagementService>();
        private readonly LazyDependency<IGroupManagementService> _groupManagementService = new LazyDependency<IGroupManagementService>();
        private readonly LazyDependency<ICorrelationService> _correlationService = new LazyDependency<ICorrelationService>();

        private ILecturerManagementService LecturerManagementService => _lecturerManagementService.Value;

        private ISubjectManagementService SubjectManagementService => _subjectManagementService.Value;

        private IGroupManagementService GroupManagementService => _groupManagementService.Value;

        private ICorrelationService CorrelationService => _correlationService.Value;

        public int LecturerId { get; set; }

        public ModifyLecturerViewModel()
        {
            Groups = new MultiSelectList(new List<Correlation>(CorrelationService.GetCorrelation("Group", null)), "Id", "Name");
        }

		public string Avatar { get; set; }

		public string SkypeContact { get; set; }

		public string Phone { get; set; }

		public string Skill { get; set; }

		public string About { get; set; }

        public ModifyLecturerViewModel(Lecturer lecturer)
        {
            if (lecturer != null)
            {

                LecturerId = lecturer.Id;
				Name = lecturer.FirstName;
	            Skill = lecturer.Skill;
                Surname = lecturer.LastName;
                Patronymic = lecturer.MiddleName;
                UserName = lecturer.User.UserName;
	            Avatar = lecturer.User.Avatar;
	            SkypeContact = lecturer.User.SkypeContact;
				Phone = lecturer.User.Phone;
				About = lecturer.User.About;
				Email = lecturer.User.Email;

                IsSecretary = lecturer.IsSecretary;
                IsLecturerHasGraduateStudents = lecturer.IsLecturerHasGraduateStudents;
	            IsActive = lecturer.IsActive;

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

        [Display(Name = "Эл. почта")]
        public string Email { get; set; }

        public string FullName => $"{this.Surname} {this.Name} {this.Patronymic}";

        [Display(Name = "Секретарь")]
        public bool IsSecretary { get; set; }

        [Display(Name = "Для выбора группы секретаря нажмите левой кнопкой мыши по соответствующему элементу в левом списке:")]
        public List<int> SelectedGroupIds { get; set; }

	    public bool IsActive { get; set; }

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
				Skill = Skill,
                MiddleName = Patronymic,
                IsSecretary = IsSecretary,
                IsLecturerHasGraduateStudents = IsLecturerHasGraduateStudents,
                SecretaryGroups = selectedGroups,
				IsActive = IsActive,
				User = new User()
				{
					Id = LecturerId,
					Avatar = Avatar,
					UserName = UserName,
					About = About,
					SkypeContact = SkypeContact,
					Phone = Phone,
					Email = Email,
				}
            });
        }
    }
}
