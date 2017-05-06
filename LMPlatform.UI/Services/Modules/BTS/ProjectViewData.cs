using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using LMPlatform.Models;

namespace LMPlatform.UI.Services.Modules.BTS
{
    [DataContract]
    public class ProjectViewData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string DateOfChange { get; set; }

        [DataMember]
        public string CreatorName { get; set; }

        [DataMember]
        public int UserQuentity { get; set; }

        [DataMember]
        public List<ProjectmemberViewData> Members { get; set; }

        public ProjectViewData(Project project, bool extended = true, bool withBugs = false, bool withMembers = false)
        {
            Id = project.Id;
            Title = project.Title;
            CreatorName = project.Creator.FullName;
            if (extended)
            {
                Description = project.Details;
                DateOfChange = project.DateOfChange.ToShortDateString();
                UserQuentity = project.ProjectUsers.Count;
            }

            if (withMembers)
            {
                Members = new List<ProjectmemberViewData>();
                foreach(var projectUser in project.ProjectUsers)
                {
                    Members.Add(new ProjectmemberViewData(projectUser));
                }
            }
        }
    }
}