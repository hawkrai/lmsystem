using System.Collections.Generic;

namespace LMPlatform.Models
{
    using Application.Core.Data;

    public class Practical : ModelBase
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
   
        public ICollection<ScheduleProtectionPractical> ScheduleProtectionPracticals { get; set; } 

        public ICollection<StudentPracticalMark> StudentPracticalMarks { get; set; } 
    }
}