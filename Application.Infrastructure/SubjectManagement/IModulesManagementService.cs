using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;
using System;

namespace Application.Infrastructure.SubjectManagement
{
    public interface IModulesManagementService
    {
        ICollection<Module> GetModules();
        IEnumerable<Module> GetModules(Int32 subjectId);
    }
}