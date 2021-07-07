using System.ServiceModel;
using System.ServiceModel.Web;
using LMPlatform.UI.Services.Modules.Materials;

namespace LMPlatform.UI.Services.Materials
{
    [ServiceContract]
    public interface IMaterialsService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/getFoldersMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        FoldersResult GetFolders(int Pid, int subjectId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/backspaceFolderMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        FoldersResult BackspaceFolder(int Pid, int subjectId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/createFolderMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        FoldersResult CreateFolder(int Pid, int subjectId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/deleteFolderMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        FoldersResult DeleteFolder(int IdFolder);

        [OperationContract]
        [WebInvoke(UriTemplate = "/deleteDocumentMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        DocumentsResult DeleteDocument(int IdDocument, int pid, int subjectId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/renameFolderMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        FoldersResult RenameFolder(int id, int pid, string newName, int subjectId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/renameDocumentMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        DocumentsResult RenameDocument(int id, int pid, string newName, int subjectId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/saveTextMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        FoldersResult SaveTextMaterials(int idd, int idf, string name, string text, int subjectId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/getDocumentsMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        DocumentsResult GetDocuments(int Pid, int subjectId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/getTextMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        DocumentsResult GetText(int id);
    }
}
