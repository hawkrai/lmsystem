using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using LMPlatform.Models;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class ProjectsViewData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string CreationDate { get; set; }

        [DataMember]
        public string CreatorName { get; set; }

        [DataMember]
        public int UserQuentity { get; set; }

        public ProjectsViewData(Project project)
        {
            Id = project.Id;
            Title = project.Title;
            CreatorName = project.Creator.FullName;
            CreationDate = project.DateOfChange.ToShortDateString();
            UserQuentity = project.ProjectUsers.Count;
        }
    }
}