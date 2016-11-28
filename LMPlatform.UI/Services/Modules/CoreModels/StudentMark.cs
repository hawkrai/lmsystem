namespace LMPlatform.UI.Services.Modules.CoreModels
{
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	[DataContract]
	public class StudentMark
	{
		[DataMember]
		public int StudentId { get; set; }

		[DataMember]
		public string FullName { get; set; }

		[DataMember]
		public List<StudentLabMarkViewData> Marks { get; set; }

		[DataMember]
		public string LabsMarkTotal { get; set; }

		[DataMember]
		public string TestMark { get; set; }
	}
}