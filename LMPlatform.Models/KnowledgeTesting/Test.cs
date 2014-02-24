using System.Collections.Generic;
using System.Linq;
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

        public bool Unlocked
        {
            get
            {
                return TestUnlocks != null && TestUnlocks.Any();
            }
        }

        public virtual Subject Subject
        {
            get;
            set;
        }

        public ICollection<Question> Questions
        {
            get;
            set;
        }

        public ICollection<TestUnlock> TestUnlocks
        {
            get;
            set;
        }
    }
}
