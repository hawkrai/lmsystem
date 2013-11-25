using System;
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

        public User CreatorID
        {
            get; 
            set; 
        }

        public int IsChosen
        {
            get; 
            set;
        }
    }
}
