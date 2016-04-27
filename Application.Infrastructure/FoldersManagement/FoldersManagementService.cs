using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Core.Constants;
using Application.Core.Data;
using Application.Infrastructure.FilesManagement;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;

namespace Application.Infrastructure.FoldersManagement
{
    using System.Web.Mvc;

    public class FoldersManagementService : IFoldersManagementService
    {
        public List<Folders> GetAllFolders()
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var currentUser = repositoriesContainer.FoldersRepository.GetAll(new Query<Folders>(u => u.Pid == 0)).ToList();
                return currentUser;
            }
        }

        public Folders FolderRootBySubjectModuleId(int submodid)
        {
             using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var currentUser = repositoriesContainer.FoldersRepository.FolderRootBySubjectModuleId(submodid);
                return currentUser;
            }
        }
    }
}