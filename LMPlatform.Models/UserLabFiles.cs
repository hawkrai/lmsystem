using Application.Core.Data;

namespace LMPlatform.Models
{
    using System;

    public class UserLabFiles : ModelBase
    {
         public string Comments { get; set; }

        public string Attachments { get; set; }

		public int UserId { get; set; }

		public int SubjectId { get; set; }

        public DateTime? Date { get; set; }
    }
}