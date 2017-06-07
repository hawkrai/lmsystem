using LMPlatform.UI.Services.Modules.Concept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LMPlatform.UI.Services.Concept
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IConceptService" in both code and config file together.
    [ServiceContract]
    public interface IConceptService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/CreateRootConcept")]
        ConceptResult SaveRootConcept(string name, string container, string subject);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/AttachSiblings")]
        ConceptResult AttachSiblings(string source, string left, string right);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetConcept?elementId={elementId}")]
        ConceptViewData GetConcept(String elementId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetRootConcepts")]
        ConceptResult GetRootConcepts(String subjectId);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetConceptTitleInfo?subjectId={subjectId}")]
        ConceptPageTitleData GetConceptTitleInfo(String subjectId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetConcepts")]
        ConceptResult GetConcepts(String parentId);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetConceptTree?elementId={elementId}")]
        ConceptViewData GetConceptTree(String elementId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Remove")]
        ConceptResult Remove(String id);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetNextConceptData?elementId={elementId}")]
        AttachViewData GetNextConceptData(String elementId);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetPrevConceptData?elementId={elementId}")]
        AttachViewData GetPrevConceptData(String elementId);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetConceptViews?conceptId={conceptId}&groupId={groupId}")]
        ConceptService.MonitoringData GetConceptViews(int conceptId, int groupId);
    }
}
