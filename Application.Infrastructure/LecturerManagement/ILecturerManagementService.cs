using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMPlatform.Models;

namespace Application.Infrastructure.LecturerManagement
{
  public interface ILecturerManagementService
  {
    Lecturer GetLecturer(int userId);

    List<Lecturer> GetLecturers();

    void Save(Lecturer lecturer);

    void UpdateLecturer(Lecturer lecturer);
  }
}
