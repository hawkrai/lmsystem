using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules.Lectures
{
	[DataContract]
	public class LecturesMarkVisitingViewData
	{
		[DataMember]
		public int StudentId { get; set; }

		[DataMember]
		public string StudentName { get; set; }

		[DataMember]
		public string Login { get; set; }

		[DataMember]
		public List<MarkViewData> Marks { get; set; }
	}
}
