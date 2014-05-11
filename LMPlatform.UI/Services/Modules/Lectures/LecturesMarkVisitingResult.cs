using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules.Lectures
{
    [DataContract]
    public class LecturesMarkVisitingResult : ResultViewData
    {
    public List<LecturesGroupsVisitingViewData> GroupsVisiting { get; set; } 
    }
}