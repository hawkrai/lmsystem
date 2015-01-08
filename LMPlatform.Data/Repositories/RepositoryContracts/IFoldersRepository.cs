using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    public interface IFoldersRepository : IRepositoryBase<Folders>
    {
        List<Folders> GetFoldersByPID(int id);

        int GetPidById(int id);

        Folders CreateFolderByPID(int id);

        void DeleteFolderByID(int id);

        void RenameFolderByID(int id, string name);
    }
}
