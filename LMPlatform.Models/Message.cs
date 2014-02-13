using System;
using System.Collections.Generic;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class Message : ModelBase
    {
        public Message()
        {
        }

        public Message(string text, string subject = "Новая тема")
        {
            Text = text;
            Subject = subject;
        }

        public string Text { get; set; }

        public string Subject { get; set; }

        public Guid AttachmentsPath { get; set; }

        public ICollection<Attachment> Attachments { get; set; }
    }
}
