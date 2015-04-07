using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    public interface IFoldersRepository : IRepositoryBase<Folders>
    {
        List<Folders> GetFoldersByPID(int id);

        List<Folders> GetFoldersByPIDandSubId(int pid, int subjectId);

        Folders FolderRootBySubjectModuleId(int id);

        Folders GetFolderByPID(int id);

        int GetPidById(int id);

        Folders CreateFolderByPID(int id, int idsubjectmodule);

        void CreateRootFolder(int idsubjectmodule, string name);

        void DeleteFolderByID(int id);

        void RenameFolderByID(int id, string name);
    }
}
