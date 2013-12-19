using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Application.Core;
using Application.Infrastructure.GroupManagement;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.AdministrationViewModels
{
  public class GroupViewModel
  {
    private readonly LazyDependency<IGroupManagementService> _groupManagementService = new LazyDependency<IGroupManagementService>();

    private IGroupManagementService GroupManagementService
    {
      get
      {
        return _groupManagementService.Value;
      }
    }

    [DisplayName("Номер")]
    public string Name { get; set; }

    [DisplayName("Год поступления")]
    public string StartYear { get; set; }

    [DisplayName("Год выпуска")]
    public string GraduationYear { get; set; }

    public int Id { get; set; }

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

    public void AddGroup()
    {
      GroupManagementService.AddGroup(GetGroupFromViewModel());
    }

    private Group GetGroupFromViewModel()
    {
      return new Group()
        {
          Name = Name,
          GraduationYear = GraduationYear,
          StartYear = StartYear
        };
    }
  }
}