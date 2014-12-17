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

        public List<Folders> GetFoldersByPID(int pid)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var students = context.Set<Folders>().Where(e => e.Pid == pid).ToList();
                return students;
            }
        }

        public Folders CreateFolderByPID(int pid)
        {
            using (var context = new LmPlatformModelsContext())
            {   
                Folders folder = new Folders
                {
                    Name = "Новая папка",
                    Pid = pid
                };
               
                context.Set<Folders>().Add(folder);
                context.SaveChanges();

                return folder;
            }
        }
    }
}