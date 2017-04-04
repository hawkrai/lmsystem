using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using LMPlatform.Models;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class BugsViewData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Summary { get; set; }

        [DataMember]
        public string Severity { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string AssignedDeveloper { get; set; }

        [DataMember]
        public string ProjectTitle { get; set; }

        [DataMember]
        public string ModifyingDate { get; set; }

        public BugsViewData(Bug bug)
        {
            Id = bug.Id;
            Summary = bug.Summary;
            Severity = bug.Severity.Name;
            Status = bug.Status.Name;
            AssignedDeveloper = GetAssignedDeveloper(bug);
        }

        private string GetAssignedDeveloper(Bug bug)
        {
            if (bug.AssignedDeveloperId == 0)
                return "отсутствует";
            return bug.AssignedDeveloper.FullName;
        }
    }
}