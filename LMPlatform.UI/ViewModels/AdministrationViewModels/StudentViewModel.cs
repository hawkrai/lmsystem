using System.ComponentModel;
using System.Web;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.AdministrationViewModels
{
    using Application.Core.UI.HtmlHelpers;

    public class StudentViewModel : BaseNumberedGridItem
  {
    [DisplayName("Номер группы")]
    public string Group { get; set; }

    [DisplayName("Полное имя")]
    public string FullName 
    {
        get { return string.Format("{0} {1} {2}", LastName, FirstName, MiddleName); } 
    }

    [DisplayName("Логин")]
    public string Login { get; set; }

    [DisplayName("Последний вход")]
    public string LastLogin { get; set; }

    [DisplayName("Действие")]
    public HtmlString HtmlLinks { get; set; }

    public int Id { get; set; }

    private string FirstName { get; set; }

    private string LastName { get; set; }

    private string MiddleName { get; set; }

    public static StudentViewModel FromStudent(Student student, string htmlLinks)
    {
      return new StudentViewModel
      {
        Id = student.Id,
        FirstName = student.FirstName,
        LastName = student.LastName,
        MiddleName = student.MiddleName,
        Group = student.Group.Name,
        Login = student.User.UserName,
        HtmlLinks = new HtmlString(htmlLinks),
        LastLogin = student.User.LastLogin.HasValue ? student.User.LastLogin.ToString() : "-"
      };
    }
  }
}