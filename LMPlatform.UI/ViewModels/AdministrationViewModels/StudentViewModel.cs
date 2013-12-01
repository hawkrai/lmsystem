using System.ComponentModel;
using System.Web;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.AdministrationViewModels
{
  public class StudentViewModel
  {
    [DisplayName("Номер")]
    public int Id { get; set; }

    [DisplayName("Полное имя")]
    public string FullName 
    {
      get { return string.Format("{0} {1} {2}", FirstName, LastName, MiddleName); } 
    }

    private string FirstName { get; set; }

    private string LastName { get; set; }

    private string MiddleName { get; set; }

    [DisplayName("Номер группы")]
    public string Group { get; set; }

    [DisplayName("")]
    public HtmlString HtmlLinks { get; set; }

    public static StudentViewModel FromStudent(Student student, string htmlLinks)
    {
      return new StudentViewModel
      {
        Id = student.Id,
        FirstName = student.FirstName,
        LastName = student.LastName,
        MiddleName = student.MiddleName,
        Group = student.Group.Name,
        HtmlLinks = new HtmlString(htmlLinks),
      };
    }
  }
}