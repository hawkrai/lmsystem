namespace LMPlatform.UI.Services.Modules
{
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	[DataContract]
	public class LectorResult : ResultViewData
	{
		[DataMember]
		public List<LectorViewData> Lectors { get; set; } 
	}
}