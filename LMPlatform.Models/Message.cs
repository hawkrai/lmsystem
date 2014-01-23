using System.Collections.Generic;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class Message : ModelBase
    {
        public Message()
        {
        }

        public Message(string text)
        {
            Text = text;
        }

        public string Text { get; set; }

        public ICollection<Attachment> Attachments { get; set; }
    }
}
