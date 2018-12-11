using System.Collections.Generic;
using LMPlatform.Models;

namespace Application.Infrastructure.TinCanManagement
{
    public interface ITinCanManagementService
    {
        List<TinCanObjects> GetAllTinCanObjects();
        void SaveTinCanObject(string name, string guid);
        void DeleteTinCanObject(int id);
        string ViewTinCanObject(string TinCanFilePath, int id);
        void EditTinCanObject(string name, string path);
        void UpdateTinCanObject(bool enable, int id);
    }
}
