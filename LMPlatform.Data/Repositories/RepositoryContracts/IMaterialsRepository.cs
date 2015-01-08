using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
    public interface IMaterialsRepository : IRepositoryBase<Materials>
    {
        void SaveTextMaterials(int idfolder, string name, string text);
    }
}
