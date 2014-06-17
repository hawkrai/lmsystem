using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class BugLog : ModelBase
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public DateTime LogDate { get; set; }

        public int BugId { get; set; }

        public int PrevStatusId { get; set; }

        public int CurrStatusId { get; set; }

        public Bug Bug
        {
            get;
            set;
        }

        public User User
        {
            get;
            set;
        }
    }
}
