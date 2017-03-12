namespace LMPlatform.UI.Services.Modules.CoreModels
{
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	using LMPlatform.UI.Services.Modules.Labs;

	[DataContract]
	public class StudentMark
	{
		[DataMember]
		public int StudentId { get; set; }

		[DataMember]
		public string FullName { get; set; }

		[DataMember]
		public string Login { get; set; }

		[DataMember]
		public List<StudentLabMarkViewData> Marks { get; set; }

		[DataMember]
		public List<LabVisitingMarkViewData> LabVisitingMark { get; set; }

		[DataMember]
        public List<UserlabFilesViewData> FileLabs { get; set; }

		[DataMember]
		public string LabsMarkTotal { get; set; }

		[DataMember]
		public string TestMark { get; set; }

		[DataMember]
		public int SubGroup { get; set; }

	}
}