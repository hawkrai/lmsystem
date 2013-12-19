using System.ComponentModel;
using System.Web;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.AdministrationViewModels
{
  public class LecturerViewModel
  {
    [DisplayName("Номер")]
    public int Id { get; set; }

    [DisplayName("Логин")]
    public string Login { get; set; }

    [DisplayName("Полное имя")]
    public string FullName
    {
      get { return string.Format("{0} {1} {2}", FirstName, LastName, MiddleName); }
    }

    private string FirstName { get; set; }

    private string LastName { get; set; }

    private string MiddleName { get; set; }

    [DisplayName("")]
    public HtmlString HtmlLinks { get; set; }

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
        };
    }
  }
}