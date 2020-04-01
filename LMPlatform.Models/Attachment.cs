using Application.Core.Data;

namespace LMPlatform.Models
{
	public class Attachment : ModelBase
	{
		public string Name { get; set; }

		public string FileName { get; set; }

		public string PathName { get; set; }

		public AttachmentType AttachmentType { get; set; }
	}
}