using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.AdministrationViewModels
{
  public class GroupViewModel
  {
    public int Id { get; set; }

    [DisplayName("Номер")]
    public string Name { get; set; }

    [DisplayName("Год поступления")]
    public string StartYear { get; set; }

    [DisplayName("Год выпуска")]
    public string GraduationYear { get; set; }

    public static GroupViewModel FormGroup(Group group)
    {
      return new GroupViewModel
      {
        Id = group.Id,
        Name = group.Name,
        StartYear = group.StartYear,
        GraduationYear = group.GraduationYear
      };
    }
  }
}