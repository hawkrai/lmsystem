namespace LMPlatform.UI.Services.Modules
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ResultViewData
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string Code { get; set; }
    }
}