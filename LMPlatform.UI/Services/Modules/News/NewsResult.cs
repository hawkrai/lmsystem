namespace LMPlatform.UI.Services.Modules.News
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class NewsResult : ResultViewData
    {
        [DataMember]
        public List<NewsViewData> News { get; set; }
    }
}