using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LMPlatform.Models;

namespace LMPlatform.UI.Services.Modules.Lectures
{
    [DataContract]
    public class LecturesMarkVisitingViewData
    {
        [DataMember]
        public int StudentId { get; set; }

        [DataMember]
        public string StudentName { get; set; }

        [DataMember]
        public List<MarkViewData> Marks { get; set; }
    }
}