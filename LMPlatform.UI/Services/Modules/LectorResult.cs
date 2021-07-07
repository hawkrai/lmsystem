using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules
{
	[DataContract]
	public class LectorResult : ResultViewData
	{
		[DataMember]
		public List<LectorViewData> Lectors { get; set; } 
	}
}
