namespace LMPlatform.UI.Services.Modules
{
	using System.Runtime.Serialization;
    using Models;


    [DataContract]
	public class LectorViewData
	{
		[DataMember]
		public int LectorId { get; set; }

		[DataMember]
		public string FullName { get; set; }

        public LectorViewData(Lecturer lecturer)
        {
            LectorId = lecturer.Id;
            FullName = lecturer.FullName;
        }
    }
}