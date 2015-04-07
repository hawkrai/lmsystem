using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using LMPlatform.UI.Services.Modules.Materials;

namespace LMPlatform.UI.Services.Materials
{
    using System.Collections;
    using System.Collections.Generic;
    using LMPlatform.UI.Services.Modules;

    [ServiceContract]
    public interface IMaterialsService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/getFoldersMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        FoldersResult GetFolders(string Pid, string subjectId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/backspaceFolderMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        FoldersResult BackspaceFolder(string Pid, string subjectId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/createFolderMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        FoldersResult CreateFolder(string Pid, string subjectId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/deleteFolderMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        FoldersResult DeleteFolder(string IdFolder, string subjectId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/deleteDocumentMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        DocumentsResult DeleteDocument(string IdDocument, string pid, string subjectId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/renameFolderMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        FoldersResult RenameFolder(string id, string pid, string newName, string subjectId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/renameDocumentMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        DocumentsResult RenameDocument(string id, string pid, string newName, string subjectId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/saveTextMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        FoldersResult SaveTextMaterials(string idd, string idf, string name, string text, string subjectId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/getDocumentsMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        DocumentsResult GetDocuments(string Pid, string subjectId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/getTextMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        DocumentsResult GetText(string id);
    }
}
