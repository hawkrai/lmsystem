using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models.KnowledgeTesting;

namespace LMPlatform.Models
{
    public class Subject : ModelBase
    {
        public string Name
        {
            get; 
            set;
        }

        public string ShortName
        {
            get; 
            set;
        }

        public ICollection<SubjectGroup> SubjectGroups
        {
            get; 
            set;
        }

        public ICollection<Test> SubjectTests
        {
            get;
            set;
        } 
    }
}