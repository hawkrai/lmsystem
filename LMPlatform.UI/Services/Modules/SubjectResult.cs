namespace LMPlatform.UI.Services.Modules
{
	using System.Runtime.Serialization;
	using LMPlatform.UI.Services.Modules.Parental;

	[DataContract]
	public class SubjectResult : ResultViewData
	{
        [DataMember]
        public SubjectViewData Subject { get; set; }
    }
}
