using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core.Data;

namespace LMPlatform.Models.KnowledgeTesting
{
    public class Question : ModelBase, ICloneable
    {
        public int? QuestionNumber
        {
            get;
            set;
        }

        public Test Test
        {
            get; 
            set;
        }

        public int TestId
        {
            get; 
            set;
        }

        public string Title
        {
            get; 
            set;
        }

        public string Description
        {
            get; 
            set;
        }

        public int ComlexityLevel
        {
            get;
            set;
        }

        public int? ConceptId
        {
            get;
            set;
        }

        public QuestionType QuestionType
        {
            get;
            set;
        }

        public ICollection<Answer> Answers
        {
            get;
            set;
        }

        public virtual ICollection<ConceptQuestions> ConceptQuestions { get; set; }

        public object Clone()
        {
            return new Question
            {
                Title = Title,
                Description = Description,
                ConceptId = ConceptId,
                ComlexityLevel = ComlexityLevel,
                QuestionType = QuestionType,
                Answers = Answers == null ? null : Answers.Select(answer => (Answer)answer.Clone()).ToList()
            };
        }
    }
}
