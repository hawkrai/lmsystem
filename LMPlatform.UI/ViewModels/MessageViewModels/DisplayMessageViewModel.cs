using System;

namespace LMPlatform.UI.ViewModels.MessageViewModels
{
    public class DisplayMessageViewModel
    {
        public string AuthorName { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public bool IsReaded { get; set; }

        public int MessageId { get; set; }

        public int AuthorId { get; set; }
    }
}