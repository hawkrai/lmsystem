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
        FoldersResult GetFolders(string Pid);

        [OperationContract]
        [WebInvoke(UriTemplate = "/createFolderMaterials", RequestFormat = WebMessageFormat.Json, Method = "POST")]
        FoldersResult CreateFolder(string Pid);
    }
}
