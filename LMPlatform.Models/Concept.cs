using Application.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMPlatform.Models
{
    public class Concept: ModelBase
    {
        public Concept()
        {

        }

        public Concept(String name, User author, Subject subject, Boolean isGroup, Boolean published)
        {
            Name = name;
            Author = author;
            Subject = subject;
            IsGroup = isGroup;
            Published = published;
            UserId = author.Id;
            SubjectId = subject.Id;
        }


  
        public string Name
        {
            get;
            set;
        }

        public string Container
        {
            get;
            set;
        }

        public virtual Concept Parent { get; set; }
        public virtual Int32? ParentId { get; set; }
        public virtual ICollection<Concept> Children { get; set; }
        
        public Boolean IsGroup { get; set; }

        public Boolean ReadOnly { get; set; }

        public virtual Int32? NextConcept { get; set; }

        public virtual Int32? PrevConcept { get; set; }

        public virtual User Author { get; set; }

        public virtual Subject Subject { get; set; }

        public Boolean Published { get; set; }

        public int SubjectId
        {
            get;
            set;
        }

        public int UserId
        {
            get;
            set;
        }

        public virtual Int32? LectureId { get; set; }

        public virtual Int32? PracticalId { get; set; }

        public virtual Int32? LabId { get; set; }

    }
}
