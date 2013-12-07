using System.Collections.Generic;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;

namespace Application.Infrastructure.SubjectManagement
{
    public class ModulesManagementService : IModulesManagementService
    {
        public ICollection<Module> GetModules()
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.ModulesRepository.GetAll().ToList();
            }
        }
    }
}