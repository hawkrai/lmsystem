using System.Collections.Generic;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class Lecturer : ModelBase
    {
        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string MiddleName
        {
            get;
            set;
        }

        public User User
        {
            get;
            set;
        }

        public ICollection<SubjectLecturer> SubjectLecturers
        {
            get;
            set;
        }
    }
}