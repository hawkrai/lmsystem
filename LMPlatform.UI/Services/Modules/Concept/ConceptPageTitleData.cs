using LMPlatform.UI.Services.Modules.Parental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LMPlatform.UI.Services.Modules.Concept
{
    [DataContract]
    public class ConceptPageTitleData
    {
        [DataMember]
        public LectorViewData Lecturer { get; set; }

        [DataMember]
        public SubjectViewData Subject { get; set; }
    }
}