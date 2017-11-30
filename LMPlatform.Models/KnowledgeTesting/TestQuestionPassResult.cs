using Application.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMPlatform.Models.KnowledgeTesting
{
    public class TestQuestionPassResults : ModelBase
    {
        public int StudentId { get; set; }

        public int TestId { get; set; }

        public int QuestionNumber { get; set; }

        public int Result { get; set; }
    }
}
