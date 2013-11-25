using System;
using Application.Core.Data;
using LMPlatform.Models.BTS;

namespace LMPlatform.Models
{
    public class Bug : ModelBase
    {
        public int ProjectId
        {
            get; 
            set;
        }

        public int UserId
        {
            get; 
            set;
        }

        public string Summary
        {
            get; 
            set;
        }

        public string Description
        {
            get; 
            set;
        }

        public string Steps
        {
            get; 
            set;
        }

        public DateTime CreationDate
        {
            get; 
            set;
        }

        public DateTime ModifiedDate
        {
            get; 
            set;
        }

        public Project Project
        {
            get; 
            set;
        }

        public int SymptomId
        {
            get; 
            set;
        }

        public int SeverityId
        {
            get; 
            set;
        }

        public int StatusId
        {
            get; 
            set;
        }

        public BugSymptom BugSymptom
        {
            get;
            set;
        }

        public BugSeverity BugSeverity
        {
            get;
            set;
        }

        public BugStatus BugStatus
        {
            get;
            set;
        }
    }
}
