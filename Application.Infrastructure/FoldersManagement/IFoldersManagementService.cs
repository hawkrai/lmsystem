using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace Application.Infrastructure.FoldersManagement
{
    using System.Net;

    public interface IFoldersManagementService
    {
        List<Folders> GetAllFolders();
    }
}
