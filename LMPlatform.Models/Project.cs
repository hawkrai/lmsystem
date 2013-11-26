using System;
using System.Collections.Generic;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class Project : ModelBase
    {
        public string Title
        {
            get; 
            set;
        }

        public DateTime CreationDate
        {
            get; 
            set;
        }

        public int CreatorId
        {
            get; 
            set; 
        }

        public int IsChosen
        {
            get; 
            set;
        }

        public User User
        { 
            get; 
            set; 
        }

        public ICollection<ProjectStudent> ProjectStudents
        {
            get; 
            set; 
        }

        public ICollection<Bug> Bugs
        {
            get;
            set;
        }
    }
}
