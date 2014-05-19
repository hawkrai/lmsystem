namespace LMPlatform.UI.Services.Modules.Parental
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using LMPlatform.UI.Services.Modules.Messages;

    [DataContract]
    public class SubjectListResult : ResultViewData
    {
        [DataMember]
        public List<SubjectViewData> Subjects { get; set; }
    }
}