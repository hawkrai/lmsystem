using LMPlatform.UI.Services.Modules.Concept;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace LMPlatform.UI.Services.Concept
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IConceptService" in both code and config file together.
    [ServiceContract]
    public interface IConceptService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/CreateRootConcept")]
        ConceptResult SaveRootConcept(string name, string container, int subject);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/AttachSiblings")]
        ConceptResult AttachSiblings(int source, int left, int right);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetConcept?elementId={elementId}")]
        ConceptViewData GetConcept(int elementId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetRootConcepts")]
        ConceptResult GetRootConcepts(int subjectId);

        [OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetRootConceptsMobile")]
		ConceptResult GetRootConceptsMobile(int subjectId, int userId, string identityKey);

		[OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetConceptTitleInfo?subjectId={subjectId}")]
        ConceptPageTitleData GetConceptTitleInfo(int subjectId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetConcepts")]
        ConceptResult GetConcepts(int parentId);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetConceptTree?elementId={elementId}")]
        ConceptViewData GetConceptTree(int elementId);

        [OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetConceptTreeMobile?elementId={elementId}")]
		ConceptViewData GetConceptTreeMobile(int elementId);

		[OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Remove")]
        ConceptResult Remove(int id);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetNextConceptData?elementId={elementId}")]
        AttachViewData GetNextConceptData(int elementId);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetPrevConceptData?elementId={elementId}")]
        AttachViewData GetPrevConceptData(int elementId);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetConceptViews?conceptId={conceptId}&groupId={groupId}")]
        ConceptService.MonitoringData GetConceptViews(int conceptId, int groupId);
    }
}
