using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    public interface IMaterialsRepository : IRepositoryBase<Materials>
    {
        List<Materials> GetMaterials(int id);

        void SaveTextMaterials(int idfolder, string name, string text);

        void SaveTextMaterials(int iddocument, int idfolder, string name, string text);

        List<Materials> GetDocumentsByFolders(Folders folder);

        Materials GetDocumentById(int id);

        void RenameDocumentByID(int id, string name);

        void DeleteDocumentByID(int id);
    }
}
