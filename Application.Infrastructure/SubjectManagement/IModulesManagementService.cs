using System.Collections.Generic;
using LMPlatform.Models;

namespace Application.Infrastructure.SubjectManagement
{
    public interface IModulesManagementService
    {
        ICollection<Module> GetModules();
    }
}