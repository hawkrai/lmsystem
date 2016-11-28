namespace LMPlatform.UI.Services.Modules.CoreModels
{
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	[DataContract]
	public class StudentsMarksResult: ResultViewData
	{
		[DataMember]
		public List<StudentMark> Students { get; set; }
	}
}