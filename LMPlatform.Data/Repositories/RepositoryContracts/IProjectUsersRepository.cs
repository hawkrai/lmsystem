using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Data;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    public interface IProjectUsersRepository : IRepositoryBase<ProjectUser>
    {
        void DeleteProjectUser(ProjectUser projectUser);
    }
}
