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

        public List<Materials> GetMaterials(int id)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var folder = context.Set<Folders>().FirstOrDefault(e => e.Id == id);
                var students = context.Set<Materials>().Where(e => e.Folders == folder).ToList();
                return students;
            }
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

        public void SaveTextMaterials(int iddocument, int idfolder, string name, string text)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var materials = context.Set<Materials>().FirstOrDefault(e => e.Id == iddocument);

                materials.Text = text;

                context.SaveChanges();
            }
        }

        public List<Materials> GetDocumentsByFolders(Folders folder)
        {
            using (var context = new LmPlatformModelsContext())
            {
                if (folder == null)
                {
                     folder = new Folders
                    {
                        Id = 0
                    };
                }

                List<Materials> documents = context.Set<Materials>().Include(x => x.Folders).Where(e => e.Folders.Id == folder.Id).ToList();
                return documents;
            }
        }

        public Materials GetDocumentById(int id)
        {
            using (var context = new LmPlatformModelsContext())
            {
                Materials document = context.Set<Materials>().FirstOrDefault(e => e.Id == id);
                return document;
            }
        }

        public void RenameDocumentByID(int id, string name)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var model = context.Set<Materials>().FirstOrDefault(e => e.Id == id);
                model.Name = name;

                context.SaveChanges();
            }
        }

        public void DeleteDocumentByID(int id)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var model = context.Set<Materials>().FirstOrDefault(e => e.Id == id);
                context.Delete(model);

                context.SaveChanges();
            }
        }
    }
}
