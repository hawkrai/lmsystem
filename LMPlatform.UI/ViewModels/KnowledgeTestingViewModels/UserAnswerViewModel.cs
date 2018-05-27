using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMPlatform.UI.ViewModels.KnowledgeTestingViewModels
{
    public class UserAnswerViewModel
    {
        public string QuestionTitle { get; set; }

        public string QuestionDescription { get; set; }

        public int Points { get; set; }

        public string AnswerString { get; set; }

        public int Number { get; set; }
    }
}