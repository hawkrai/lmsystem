using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories
{
    public class FoldersRepository : RepositoryBase<LmPlatformModelsContext, Folders>, IFoldersRepository
    {
        public FoldersRepository(LmPlatformModelsContext dataContext)
            : base(dataContext)
        {  
        }

        public List<Folders> GetFoldersByPIDandSubId(int pid, int idsubjectmodule)
        {
            using (var context = new LmPlatformModelsContext())
            {
                SubjectModule subjectmodule = context.Set<SubjectModule>().FirstOrDefault(e => e.Id == idsubjectmodule);
                var folders = context.Set<Folders>().Where(e => e.Pid == pid && e.SubjectModule.Id == subjectmodule.Id).ToList();
                return folders;
            }
        }

        public List<Folders> GetFoldersByPID(int pid)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var folders = context.Set<Folders>().Where(e => e.Pid == pid && e.SubjectModule.Subject.IsArchive == false).ToList();
                return folders;
            }
        }

        public Folders FolderRootBySubjectModuleId(int id)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var folder = context.Set<Folders>().FirstOrDefault(e => e.SubjectModuleId == id);
                return folder;
            }
        }

        public Folders GetFolderByPID(int id)
        {
            using (var context = new LmPlatformModelsContext())
            {
                Folders folder = context.Set<Folders>().FirstOrDefault(e => e.Id == id);
                return folder;
            }
        }

        public int GetPidById(int id)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var folder = context.Set<Folders>().FirstOrDefault(e => e.Id == id);
                return folder.Pid;
            }
        }

        public Folders CreateFolderByPID(int pid, int idsubjectmodule)
        {
            using (var context = new LmPlatformModelsContext())
            {
                SubjectModule subjectmodule = context.Set<SubjectModule>().FirstOrDefault(e => e.Id == idsubjectmodule);

                Folders folder = new Folders
                {
                    Name = "Новая папка",
                    Pid = pid,
                    SubjectModule = subjectmodule
                };
               
                context.Set<Folders>().Add(folder);
                context.SaveChanges();

                return folder;
            }
        }

        public void CreateRootFolder(int idsubjectmodule, string name)
        {
            using (var context = new LmPlatformModelsContext())
            {
                SubjectModule subjectmodule = context.Set<SubjectModule>().FirstOrDefault(e => e.Id == idsubjectmodule);

                Folders folder = new Folders
                {
                    Name = name,
                    Pid = 0,
                    SubjectModule = subjectmodule
                };

                context.Set<Folders>().Add(folder);
                context.SaveChanges();
            }
        }

        public void DeleteFolderByID(int id)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var model = context.Set<Folders>().FirstOrDefault(e => e.Id == id);
                context.Delete(model);

                context.SaveChanges();
            }            
        }

        public void RenameFolderByID(int id, string name)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var model = context.Set<Folders>().FirstOrDefault(e => e.Id == id);
                model.Name = name;

                context.SaveChanges();
            }
        }
    }
}