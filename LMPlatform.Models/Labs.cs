namespace LMPlatform.Models
{
    using System.Collections.Generic;

    using Application.Core.Data;

    public class Labs : ModelBase
    {
        public string Theme
        {
            get;
            set;
        }

        public int Duration
        {
            get;
            set;
        }

        public int SubjectId
        {
            get;
            set;
        }

        public int Order
        {
            get;
            set;
        }

        public string ShortName
        { 
            get; 
            set;
        }

        public string Attachments
        {
            get;
            set;
        }

        public Subject Subject
        {
            get;
            set;
        }

        public ICollection<ScheduleProtectionLabs> ScheduleProtectionLabs { get; set; } 

        public ICollection<StudentLabMark> StudentLabMarks { get; set; } 
    }
}