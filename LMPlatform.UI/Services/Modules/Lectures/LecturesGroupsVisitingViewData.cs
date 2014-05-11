using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules.Lectures
{
    [DataContract]
    public class LecturesGroupsVisitingViewData
    {
        [DataMember]
        public int GroupId { get; set; }

        [DataMember]
        public List<LecturesMarkVisitingViewData> LecturesMarksVisiting { get; set; } 
    }
}