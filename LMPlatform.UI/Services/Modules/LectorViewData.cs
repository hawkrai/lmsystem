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
        public string UserName { get; set; }

        [DataMember]
		public string FullName { get; set; }

        public LectorViewData(Lecturer lecturer, bool withUsername = false)
        {
            LectorId = lecturer.Id;
            FullName = lecturer.FullName;
            if (withUsername)
            {
                UserName = lecturer.User.UserName;
            }
        }
    }
}