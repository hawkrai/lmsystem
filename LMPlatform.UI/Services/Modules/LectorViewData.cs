using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules
{
    using Models;

    /// <summary>
    /// Simple <see cref="Lecturer"/> if second constructor param is false else with <see cref="User"/> prop included.
    /// </summary>
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