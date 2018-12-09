using Application.Core.Data;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace Application.Infrastructure.TinCanManagement
{
    public class TinCanManagementService : ITinCanManagementService
    {
        public List<TinCanObjects> GetAllTinCanObjects()
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.RepositoryFor<TinCanObjects>().GetAll(new Query<TinCanObjects>(e => !e.IsDeleted)).ToList();
            }
        }

        public void SaveTinCanObject(string name, string guid)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.RepositoryFor<TinCanObjects>().Save(new TinCanObjects()
                {
                    Name = name,
                    Path = guid,
                    Enabled = false,
                    IsDeleted = false
                });
                repositoriesContainer.ApplyChanges();
            }
        }

        public void DeleteTinCanObject(int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var data = repositoriesContainer.RepositoryFor<TinCanObjects>().GetBy(new Query<TinCanObjects>(e => e.Id == id));
                data.IsDeleted = true;
                repositoriesContainer.RepositoryFor<TinCanObjects>().Save(data);
                repositoriesContainer.ApplyChanges();
            }
        }

        public string ViewTinCanObject(string TinCanFilePath, int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var data = repositoriesContainer.RepositoryFor<TinCanObjects>().GetBy(new Query<TinCanObjects>(e => e.Id == id));
                var tincanPath = TinCanFilePath + "//" + data.Path + "//" + "tincan.xml";
                var dirName = new FileInfo(TinCanFilePath).Directory.Name;
                XDocument xdoc = XDocument.Load(tincanPath);
                return dirName + "/" + data.Path + "/" + xdoc.Descendants(XName.Get("launch", @"http://projecttincan.com/tincan.xsd")).First().Value;
            }
        }

        public void EditTinCanObject(string name, string path)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var data = repositoriesContainer.RepositoryFor<TinCanObjects>().GetBy(new Query<TinCanObjects>(e => e.Path == path));
                data.Name = name;
                repositoriesContainer.RepositoryFor<TinCanObjects>().Save(data);
                repositoriesContainer.ApplyChanges();
            }
        }

        public void UpdateTinCanObject(bool enable, int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var data = repositoriesContainer.RepositoryFor<TinCanObjects>().GetBy(new Query<TinCanObjects>(e => e.Id == id));
                data.Enabled = enable;
                repositoriesContainer.RepositoryFor<TinCanObjects>().Save(data);
                repositoriesContainer.ApplyChanges();
            }
        }
    }
}
