using System.Collections.Generic;
using System.Runtime.Serialization;
using Application.Infrastructure.FilesManagement;
using LMPlatform.UI.Services.Modules.Lectures;

namespace LMPlatform.UI.Services.Modules.Labs
{
    [DataContract]
    public class UserLabFilesResult : ResultViewData
    {
        [DataMember]
        public List<UserlabFilesViewData> UserLabFiles { get; set; } 
    }
}