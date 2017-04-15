using Application.Core.Data;
using LMPlatform.Models.KnowledgeTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMPlatform.Models
{

    public class ConceptQuestions: ModelBase
    {
        public Int32 Id { get; set; }
        public Int32 ConceptId { get; set; }
        public Int32 QuestionId { get; set; }

        public virtual Concept Concept { get; set; }

        public virtual Question Question { get; set; }
    }
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

        public virtual ICollection<ConceptQuestions> ConceptQuestions { get; set; }
  
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
