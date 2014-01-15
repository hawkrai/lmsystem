using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace Application.Infrastructure.SubjectManagement
{
    public interface IModulesManagementService
    {
        ICollection<Module> GetModules();
    }
}