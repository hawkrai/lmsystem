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

        public List<Materials> GetDocumentsByIdFolders(int ID)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                Folders folder = repositoriesContainer.FoldersRepository.GetFolderByPID(ID);

                List<Materials> materials = repositoriesContainer.MaterialsRepository.GetDocumentsByFolders(folder);

                return materials;
            }
        }

        public Materials GetTextById(int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                Materials materials = repositoriesContainer.MaterialsRepository.GetDocumentById(id);

                return materials;
            }
        }

        public int GetPidById(int PID)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                int id = 0;

                if (PID != 0)
                {
                   id = repositoriesContainer.FoldersRepository.GetPidById(PID);
                }
               
                return id;
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

        public void DeleteFolder(int ID)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.FoldersRepository.DeleteFolderByID(ID);
            }
        }

        public void DeleteDocument(int ID)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.MaterialsRepository.DeleteDocumentByID(ID);
            }
        }

        public void RenameFolder(int id, string name)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.FoldersRepository.RenameFolderByID(id, name);
            }
        }

        public void RenameDocument(int id, string name)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.MaterialsRepository.RenameDocumentByID(id, name);
            }
        }
     
        public void SaveTextMaterials(int id_document, int id_folder, string name, string text)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                if (id_document == 0)
                {
                    repositoriesContainer.MaterialsRepository.SaveTextMaterials(id_folder, name, text);
                }
                else
                {
                    repositoriesContainer.MaterialsRepository.SaveTextMaterials(id_document, id_folder, name, text);
                }
            }
        }
    }
}
