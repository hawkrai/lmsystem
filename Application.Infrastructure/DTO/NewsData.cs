using LMPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructure.DTO
{
    public class NewsData
    {
        public int Id { get; set; }

        public int LecturerId { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string DateCreate { get; set; }

        public bool Disabled { get; set; }

        public string PathFile { get; set; }

        public IList<Attachment> Attachments
        {
            get;
            set;
        }
    }
}
