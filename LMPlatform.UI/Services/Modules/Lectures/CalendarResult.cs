using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules.Lectures
{
    [DataContract]
    public class CalendarResult : ResultViewData
    {
        [DataMember]
        public List<CalendarViewData> Calendar { get; set; } 
    }
}
