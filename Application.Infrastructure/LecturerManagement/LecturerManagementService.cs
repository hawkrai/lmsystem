using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Data;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;

namespace Application.Infrastructure.LecturerManagement
{
  public class LecturerManagementService : ILecturerManagementService
  {
    public Lecturer GetLecturer(int userId)
    {
      using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
      {
        return repositoriesContainer.LecturerRepository.GetBy(new Query<Lecturer>(e => e.Id == userId).Include(e => e.SubjectLecturers).Include(e => e.User));
      }
    }

    public List<Lecturer> GetLecturers()
    {
      using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
      {
        return repositoriesContainer.LecturerRepository.GetAll(new Query<Lecturer>().Include(e => e.SubjectLecturers).Include(e => e.User)).ToList();
      }
    }

    public void Save(Lecturer lecturer)
    {
      using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
      {
        repositoriesContainer.LecturerRepository.SaveLecturer(lecturer);
        repositoriesContainer.ApplyChanges();
      }
    }

    public void UpdateLecturer(Lecturer lecturer)
    {
      using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
      {
        repositoriesContainer.LecturerRepository.Save(lecturer);
        repositoriesContainer.ApplyChanges();
      }
    }
  }
}
