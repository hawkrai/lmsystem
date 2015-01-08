namespace LMPlatform.UI.Services.Modules.Materials
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class FoldersResult : ResultViewData
    {
        [DataMember]
        public int Pid { get; set; }

        [DataMember]
        public List<FoldersViewData> Folders { get; set; } 
    }
}