using Application.Core.Data;

namespace LMPlatform.Models.KnowledgeTesting
{
    public class TestUnlock : ModelBase
    {
        public int TestId
        {
            get; 
            set;
        }

        public int StudentId
        {
            get;
            set;
        }

        public virtual Student Student
        {
            get; 
            set;
        }

        public Test Test
        {
            get; 
            set;
        }
    }
}
