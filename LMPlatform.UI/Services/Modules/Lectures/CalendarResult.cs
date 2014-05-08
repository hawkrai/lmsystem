namespace LMPlatform.UI.Services.Modules.Lectures
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class CalendarResult : ResultViewData
    {
        [DataMember]
        public List<CalendarViewData> Calendar { get; set; } 
    }
}