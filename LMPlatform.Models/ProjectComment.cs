using System;
using Application.Core.Data;

namespace LMPlatform.Models
{
	public class ProjectComment : ModelBase
	{
		public string CommentText { get; set; }

		public int UserId { get; set; }

		public DateTime CommentingDate { get; set; }

		public int ProjectId { get; set; }

		public User User { get; set; }

		public Project Project { get; set; }
	}
}