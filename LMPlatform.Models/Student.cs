using System.Collections.Generic;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class Student : ModelBase
    {
        public string Email
        {
            get;
            set;
        }

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

        public int GroupId
        {
            get;
            set;
        }

        public Group Group
        {
            get;
            set;
        }

        public ICollection<ProjectStudent> ProjectStudents
        {
            get;
            set;
        }
    }
}