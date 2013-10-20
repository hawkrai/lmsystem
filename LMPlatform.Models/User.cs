using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    }
}
