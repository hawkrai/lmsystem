using System.Collections.Generic;
using Application.Core.Data;

namespace LMPlatform.Models.KnowledgeTesting
{
    public class Question : ModelBase
    {
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
    }
}
