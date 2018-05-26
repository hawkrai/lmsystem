using System;
using Application.Core.Data;

namespace LMPlatform.Models.KnowledgeTesting
{
    public class AnswerOnTestQuestion : ModelBase
    {
        public int Number
        {
            get;
            set;
        }

        public int TestId
        {
            get; 
            set;
        }

        public int UserId
        {
            get;
            set;
        }

        public int? AnswerId
        {
            get;
            set;
        }

        public int QuestionId
        {
            get;
            set;
        }

        public int Points
        {
            get;
            set;
        }

        public bool TestEnded
        {
            get;
            set;
        }

        public DateTime? Time
        {
            get; 
            set;
        }

        public User User
        {
            get;
            set;
        }

        public Answer Answer
        {
            get;
            set;
        }
    }
}
