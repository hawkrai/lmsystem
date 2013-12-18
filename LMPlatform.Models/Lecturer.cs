using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        [NotMapped]
        public string FullName
        {
          get { return string.Format("{0} {1} {2}", FirstName, MiddleName, LastName); }
        }

        public ICollection<SubjectLecturer> SubjectLecturers
        {
            get;
            set;
        }
    }
}