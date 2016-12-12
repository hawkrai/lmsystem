namespace LMPlatform.UI.Services.Modules.Labs
{
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	[DataContract]
	public class ScheduleProtectionLabResult : ResultViewData
	{
		[DataMember]
		public List<ScheduleProtectionLab> ScheduleProtectionLabRecomended { get; set; } 
	}
}