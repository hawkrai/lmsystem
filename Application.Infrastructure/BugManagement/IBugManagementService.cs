using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMPlatform.Models;

namespace Application.Infrastructure.BugManagement
{
    public interface IBugManagementService
    {
        Bug GetBug(int bugId);

        List<Bug> GetBugs();
    }
}
