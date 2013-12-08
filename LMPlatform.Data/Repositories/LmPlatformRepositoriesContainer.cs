using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;

namespace LMPlatform.Data.Repositories
{
    public class LmPlatformRepositoriesContainer : IRepositoriesContainer
    {
        private readonly LmPlatformModelsContext _dataContext = new LmPlatformModelsContext();

        public IRepositoryBase<TModel> RepositoryFor<TModel>() where TModel : ModelBase, new()
        {
            IRepositoryBase<TModel> result = new RepositoryBase<LmPlatformModelsContext, TModel>(_dataContext);

            return result;
        }

        public IUsersRepository UsersRepository { get; set; }

        public IBugsRepository BugsRepository { get; set; }

        public IGroupsRepository GroupsRepository { get; set; }

        public IProjectsRepository ProjectsRepository { get; set; }

        public IStudentsRepository StudentsRepository { get; set; }

        public ISubjectRepository SubjectRepository { get; set; }

        public ITestsRepository TestsRepository { get; set; }

        public IModulesRepository ModulesRepository { get; set; }
		
		public ILecturerRepository LecturerRepository { get; set; }
		
        public void ApplyChanges()
        {
            _dataContext.SaveChanges();
        }

        public void Dispose()
        {
            _dataContext.Dispose();
        }

        public LmPlatformRepositoriesContainer()
        {
            UsersRepository = new UsersRepository(_dataContext);
            BugsRepository = new BugsRepository(_dataContext);
            GroupsRepository = new GroupsRepository(_dataContext);
            ProjectsRepository = new ProjectsRepository(_dataContext);
            StudentsRepository = new StudentsRepository(_dataContext);
            SubjectRepository = new SubjectRepository(_dataContext);
            TestsRepository = new TestsRepository(_dataContext);
            UsersRepository = new UsersRepository(_dataContext);
            ModulesRepository = new ModulesRepository(_dataContext);
			LecturerRepository = new LecturerRepository(_dataContext);
        }
    }
}
