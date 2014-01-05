using Application.Core.Data;

namespace LMPlatform.Models.KnowledgeTesting
{
    public class Answer : ModelBase
    {
        public int QuestionId
        {
            get; 
            set;
        }

        public Question Question
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }

        public int СorrectnessIndicator
        {
            get;
            set;
        }
    }
}
