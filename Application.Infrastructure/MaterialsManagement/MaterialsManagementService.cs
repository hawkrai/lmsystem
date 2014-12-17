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

namespace Application.Infrastructure.MaterialsManagement
{
    using System.Web.Mvc;

    public class MaterialsManagementService : IMaterialsManagementService
    {
        public List<Folders> GetFolders(int PID)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                List<Folders> folders = repositoriesContainer.FoldersRepository.GetFoldersByPID(PID);

                return folders;
            }
        }

        public Folders CreateFolder(int PID)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                Folders folder = repositoriesContainer.FoldersRepository.CreateFolderByPID(PID);

                return folder;
            }
        }
    }
}
