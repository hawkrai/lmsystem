using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using LMPlatform.Models;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class ProjectBugViewData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Severity { get; set; }

        [DataMember]
        public string Status { get; set; }

        public ProjectBugViewData(Bug bug)
        {
            Id = bug.Id;
            Status = bug.Status.Name;
            Severity = bug.Severity.Name;
        }
    }
}