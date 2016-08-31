namespace LMPlatform.UI.Services.Modules
{
	using System.Runtime.Serialization;

	[DataContract]
	public class LectorViewData
	{
		[DataMember]
		public int LectorId { get; set; }

		[DataMember]
		public string FullName { get; set; }
	}
}