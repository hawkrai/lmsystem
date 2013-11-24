using Application.Core.Data;

namespace LMPlatform.Models.KnowledgeTesting
{
    public class Test : ModelBase
    {
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

        public int TimeForCompleting
        {
            get;
            set;
        }

        public bool SetTimeForAllTest
        {
            get;
            set;
        }

        public int SubjectId
        {
            get;
            set;
        }

        public virtual Subject Subject
        {
            get;
            set;
        }
    }
}
