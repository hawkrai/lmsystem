using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructure.BTS
{
    public interface IMatrixManagmentService
    {
        void Fillrequirements(int projectId, string requirementsFileName);
    }
}
