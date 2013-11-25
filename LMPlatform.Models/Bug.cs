using System;
using Application.Core.Data;
using LMPlatform.Models.BTS;

namespace LMPlatform.Models
{
    public class Bug : ModelBase
    {
        public Project ProjectID
        {
            get; 
            set;
        }

        public User UserID
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

        public BugSymptom SymptomID
        {
            get; 
            set;
        }

        public BugSeverity SeverityID
        {
            get; 
            set;
        }

        public BugStatus StatusID
        {
            get; 
            set;
        }
    }
}
