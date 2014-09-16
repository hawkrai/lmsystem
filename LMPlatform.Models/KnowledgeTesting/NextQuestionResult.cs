using System.Collections.Generic;

namespace LMPlatform.Models.KnowledgeTesting
{
    using System.Dynamic;

    public class NextQuestionResult
    {
        public Question Question
        {
            get;
            set;
        }

        public int Number
        {
            get;
            set;
        }

        public Dictionary<int, PassedQuestionResult> QuestionsStatuses
        {
            get;
            set;
        }

        public int Mark
        {
            get;
            set;
        }

        public double Seconds
        {
            get; 
            set;
        }

        public bool SetTimeForAllTest
        {
            get;
            set;
        }
    }
}
