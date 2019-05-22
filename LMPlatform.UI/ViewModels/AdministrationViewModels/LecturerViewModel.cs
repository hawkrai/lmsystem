using System.ComponentModel;
using System.Web;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.AdministrationViewModels
{
    using System.Linq;

    using Application.Core.UI.HtmlHelpers;

    public class LecturerViewModel : BaseNumberedGridItem
    {
        [DisplayName("Полное имя")]
        public string FullName
        {
            get { return string.Format("{0} {1} {2}", LastName, FirstName, MiddleName); }
        }

        [DisplayName("Логин")]
        public string Login { get; set; }

        [DisplayName("Последний вход")]
        public string LastLogin { get; set; }

        [DisplayName("Предметы")]
        public string Subjects { get; set; }

        private string FirstName { get; set; }

        private string LastName { get; set; }

        private string MiddleName { get; set; }

        [DisplayName("Действие")]
        public HtmlString HtmlLinks { get; set; }

        public int Id { get; set; }

        public static LecturerViewModel FormLecturers(Lecturer lecturer, string htmlLinks)
        {
            return new LecturerViewModel
              {
                  Id = lecturer.Id,
                  FirstName = lecturer.FirstName,
                  LastName = lecturer.LastName,
                  MiddleName = lecturer.MiddleName,
                  Login = lecturer.User.UserName,
                  HtmlLinks = new HtmlString(htmlLinks),
                  LastLogin = lecturer.User.LastLogin.HasValue ? lecturer.User.LastLogin.ToString() : "-",
                  Subjects = (lecturer.SubjectLecturers != null && lecturer.SubjectLecturers.Count > 0)
                                ? lecturer.SubjectLecturers.Count.ToString()
				  Subjects = (lecturer.SubjectLecturers != null && lecturer.SubjectLecturers.Count > 0 && lecturer.SubjectLecturers.Any(e => !e.Subject.IsArchive))
                                ? lecturer.SubjectLecturers.Count(e => !e.Subject.IsArchive).ToString()
                                : "-",
              };
        }
    }
}