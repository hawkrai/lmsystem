namespace LMPlatform.UI.Services.Modules.CoreModels
{
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	[DataContract]
	public class StudentsResult: ResultViewData
	{
		[DataMember]
		public List<StudentsViewData> Students { get; set; } 
	}
}