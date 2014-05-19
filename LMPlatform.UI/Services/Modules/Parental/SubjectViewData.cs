using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMPlatform.UI.Services.Modules.Parental
{
    using System.Runtime.Serialization;

    using LMPlatform.Models;

    [DataContract]
    public class SubjectViewData
    {
        public SubjectViewData(Subject subject)
        {
            Id = subject.Id;
            Name = subject.Name;
            ShortName = subject.ShortName;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ShortName { get; set; }
    }
}