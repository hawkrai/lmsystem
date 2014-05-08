namespace LMPlatform.UI.Services.Modules.CoreModels
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class GroupsResult : ResultViewData
    {
        [DataMember]
        public List<GroupsViewData> Groups { get; set; } 
    }
}