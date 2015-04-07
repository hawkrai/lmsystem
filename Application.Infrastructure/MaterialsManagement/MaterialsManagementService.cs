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
        public List<Folders> GetFolders(int PID, int subjectId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                Subject subject = repositoriesContainer.SubjectRepository.GetBy(new Query<Subject>(e => e.Id == subjectId).Include(e => e.SubjectModules.Select(x => x.Module)));

                SubjectModule submod = subject.SubjectModules.FirstOrDefault(e => e.ModuleId == 9);

                if (PID == 0)
                {
                    List<Folders> fol = repositoriesContainer.FoldersRepository.GetFoldersByPIDandSubId(PID, submod.Id);
                    PID = fol[0].Id;
                }

                List<Folders> folders = repositoriesContainer.FoldersRepository.GetFoldersByPIDandSubId(PID, submod.Id);

                return folders;
            }
        }

        public List<Materials> GetDocumentsByIdFolders(int ID, int subjectId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                Subject subject = repositoriesContainer.SubjectRepository.GetBy(new Query<Subject>(e => e.Id == subjectId).Include(e => e.SubjectModules.Select(x => x.Module)));

                SubjectModule submod = subject.SubjectModules.FirstOrDefault(e => e.ModuleId == 9);

                if (ID == 0)
                {
                    List<Folders> fol = repositoriesContainer.FoldersRepository.GetFoldersByPIDandSubId(ID, submod.Id);
                    List<Materials> materials = repositoriesContainer.MaterialsRepository.GetDocumentsByFolders(fol[0]);

                    return materials;
                }
                else
                {
                    Folders folder = repositoriesContainer.FoldersRepository.GetFolderByPID(ID);
                    List<Materials> materials = repositoriesContainer.MaterialsRepository.GetDocumentsByFolders(folder);

                    return materials;
                }
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

        public Folders CreateFolder(int PID, int subjectid)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                Subject subject = repositoriesContainer.SubjectRepository.GetBy(new Query<Subject>(e => e.Id == subjectid).Include(e => e.SubjectModules.Select(x => x.Module)));

                SubjectModule submod = subject.SubjectModules.FirstOrDefault(e => e.ModuleId == 9);

                if (PID == 0)
                {
                    List<Folders> fol = repositoriesContainer.FoldersRepository.GetFoldersByPIDandSubId(PID, submod.Id);
                    PID = fol[0].Id;
                }

                Folders folder = repositoriesContainer.FoldersRepository.CreateFolderByPID(PID, submod.Id);

                return folder;
            }
        }

        public void CreateRootFolder(int id_subject_module, string name)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.FoldersRepository.CreateRootFolder(id_subject_module, name);
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
     
        public void SaveTextMaterials(int id_document, int id_folder, string name, string text, int subjectid)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                if (id_document == 0 && id_folder == 0)
                {
                    int PID = 0;
                    Subject subject = repositoriesContainer.SubjectRepository.GetBy(new Query<Subject>(e => e.Id == subjectid).Include(e => e.SubjectModules.Select(x => x.Module)));
                    SubjectModule submod = subject.SubjectModules.FirstOrDefault(e => e.ModuleId == 9);

                        List<Folders> fol = repositoriesContainer.FoldersRepository.GetFoldersByPIDandSubId(PID, submod.Id);
                        id_folder = fol[0].Id;

                        repositoriesContainer.MaterialsRepository.SaveTextMaterials(id_folder, name, text);
                }
                else if (id_document == 0)
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
