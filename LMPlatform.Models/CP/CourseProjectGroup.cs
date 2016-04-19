using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMPlatform.Models.CP
{
    public class CourseProjectGroup
    {
        public int CourseProjectGroupId { get; set; }

        public int CourseProjectId { get; set; }

        public int GroupId { get; set; }

        public virtual CourseProject CourseProject { get; set; }

        public virtual Group Group { get; set; }
    }
}
