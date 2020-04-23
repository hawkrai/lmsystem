using Application.Core.Data;
using LMPlatform.Models.KnowledgeTesting;
using System.Collections.Generic;

namespace LMPlatform.Models
{
	public class ConceptQuestions: ModelBase
    {
        public int Id { get; set; }
        public int ConceptId { get; set; }
        public int QuestionId { get; set; }
        public virtual Concept Concept { get; set; }
        public virtual Question Question { get; set; }
    }

    public class Concept: ModelBase
    {
        public Concept()
        {
            
        }

        public Concept(string name, User author, Subject subject, bool isGroup, bool published)
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

        public string Name { get; set; }

        public string Container { get; set; }

        public virtual Concept Parent { get; set; }

        public virtual int? ParentId { get; set; }

        public virtual ICollection<Concept> Children { get; set; }

        public bool IsGroup { get; set; }

        public bool ReadOnly { get; set; }

        public virtual int? NextConcept { get; set; }

        public virtual int? PrevConcept { get; set; }

        public virtual User Author { get; set; }

        public virtual Subject Subject { get; set; }

        public bool Published { get; set; }

        public int SubjectId { get; set; }

        public int UserId { get; set; }

        public virtual int? LectureId { get; set; }

        public virtual int? PracticalId { get; set; }

        public virtual int? LabId { get; set; }

        public List<Concept> GetAllChildren()
        {
	        var list = new List<Concept>();
	        if (Children != null)
	        {
		        foreach (var child in Children) list.AddRange(child.GetAllChildren());
		        list.AddRange(Children);
	        }

	        return list;
        }
    }
}
