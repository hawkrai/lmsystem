using System.Collections.Generic;

namespace LMPlatform.Models.KnowledgeTesting
{
    public class RealTimePassingResult
    {
        public string StudentName
        {
            get;
            set;
        }

        public Dictionary<int, PassedQuestionResult> PassResults
        {
            get;
            set;
        }
    }
}
