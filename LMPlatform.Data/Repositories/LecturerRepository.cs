using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories
{
  public class LecturerRepository : RepositoryBase<LmPlatformModelsContext, Lecturer>, ILecturerRepository
  {
    public LecturerRepository(LmPlatformModelsContext dataContext) : base(dataContext)
    {
    }

    public void SaveLecturer(Lecturer lecturer)
    {
      using (var context = new LmPlatformModelsContext())
      {
        context.Set<Lecturer>().Add(lecturer);
        context.SaveChanges();
      }
    }
  }
}
