using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class User : ModelBase
    {
        public string UserName
        {
            get;
            set;
        }

        public Membership Membership
        {
            get;
            set;
        }

        public bool? IsServiced
        {
            get;
            set;
        }

        public Student Student
        {
            get;
            set;
        }

        public Lecturer Lecturer
        {
            get;
            set;
        }

        public ICollection<Project> Projects
        {
            get;
            set;
        }

        public ICollection<UserMessages> Messages
        {
            get;
            set;
        }

        [NotMapped]
        public string FullName
        {
            get
            {
                if (Student != null)
                {
                    return Student.FullName.Trim(' ');
                }
                else if (Lecturer != null)
                {
                    return Lecturer.FullName.Trim(' ');
                }

                return UserName;
            }
        }
    }
}
