using System.ComponentModel;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.AdministrationViewModels
{
  public class StudentViewModel
  {
    [DisplayName("Номер")]
    public int Id { get; set; }

    [DisplayName("Полное имя")]
    public string FullName { get; set; }

    [DisplayName("Номер группы")]
    public string Group { get; set; }

    public static StudentViewModel FromStudent(Student student)
    {
      return new StudentViewModel
      {
        Id = student.Id,
        FullName = string.Format("{0} {1}", student.FirstName, student.LastName),
        Group = student.Group.Name,
      };
    }
  }
}