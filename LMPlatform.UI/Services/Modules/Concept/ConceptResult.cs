using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules.Concept
{
    [DataContract]
    public class ConceptResult : ResultViewData
    {
        [DataMember]
        public ConceptViewData Concept { get; set; }

        [DataMember]
        public List<ConceptViewData> Concepts { get; set; }

        [DataMember]
        public IEnumerable<ConceptViewData> Children { get; set; }

        [DataMember]
        public string SubjectName { get; set; }
    }
}
