using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories
{
    public class MaterialsRepository : RepositoryBase<LmPlatformModelsContext, Materials>, IMaterialsRepository
    {
        public MaterialsRepository(LmPlatformModelsContext dataContext)
            : base(dataContext)
        {
        }

        public void SaveTextMaterials(int idfolder, string name, string text)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var folder = context.Set<Folders>().FirstOrDefault(e => e.Id == idfolder);

                Materials material = new Materials
                {
                    Folders = folder,
                    Name = name,
                    Text = text
                };

                context.Set<Materials>().Add(material);
                context.SaveChanges();
            }
        }
    }
}
