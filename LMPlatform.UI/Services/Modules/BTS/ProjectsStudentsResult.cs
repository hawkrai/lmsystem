using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class ProjectsStudentsResult : ResultViewData
    {
        [DataMember]
        public List<ProjectsStudentViewData> ProjectsStudents { get; set; }

        [DataMember]
        public int TotalCount { get; set; }
    }
}