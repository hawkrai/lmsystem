namespace LMPlatform.UI.Services.Modules
{
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	using LMPlatform.UI.Services.Modules.Parental;

	[DataContract]
	public class SubjectResult : ResultViewData
	{
		[DataMember]
		public List<SubjectViewData> Subjects { get; set; } 
	}
}