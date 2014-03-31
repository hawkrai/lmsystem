using System;
using Application.Core.Data;

namespace LMPlatform.Models.KnowledgeTesting
{
    public class TestPassResult : ModelBase
    {
        public int StudentId
        {
            get;
            set;
        }

        public int TestId
        {
            get;
            set;
        }

        public int Points
        {
            get;
            set;
        }

        public DateTime Time
        {
            get;
            set;
        }
    }
}
