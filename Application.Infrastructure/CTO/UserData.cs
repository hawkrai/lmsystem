using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructure.CTO
{
    public class UserData
    {
     
            public int UserId { get; set; }

            public bool IsStudent { get; set; }

            public bool IsLecturer { get; set; }

            public bool IsSecretary { get; set; }

            public bool HasChosenDiplomProject { get; set; }

            public bool HasAssignedDiplomProject { get; set; }
        
    }
}
