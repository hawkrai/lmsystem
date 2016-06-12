using System.Collections.Generic;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;
using System;

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

        public IEnumerable<Module> GetModules(Int32 subjectId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.ModulesRepository.GetAll().Where(
                    s=>
                        s.SubjectModules.Any(sm=>sm.SubjectId==subjectId)
                        ).ToList();
            }
        }
    }
}