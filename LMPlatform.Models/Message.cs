using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Data;

namespace LMPlatform.Models
{
    public class Message : ModelBase
    {
        public string Text { get; set; }

        public int AuthorId { get; set; }

        public User Author { get; set; }

        public ICollection<User> Recipients { get; set; }

        public ICollection<Attachment> Attachments { get; set; }
    }
}
