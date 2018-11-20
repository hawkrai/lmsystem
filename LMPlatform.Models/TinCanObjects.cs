using Application.Core.Data;

namespace LMPlatform.Models
{
	public class TinCanObjects : ModelBase
	{
		public string Name { get; set; }

		public string Path { get; set; }

		public bool IsDeleted { get; set; }

		public bool Enabled { get; set; }
	}
}
