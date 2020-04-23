using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules.Lectures
{
    [DataContract]
    public class LecturesResult : ResultViewData
    {
        [DataMember]
        public List<LecturesViewData> Lectures { get; set; } 
    }
}
