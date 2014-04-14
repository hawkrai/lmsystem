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

        public IProjectUsersRepository ProjectUsersRepository { get; set; }

        public IStudentsRepository StudentsRepository { get; set; }

        public ISubjectRepository SubjectRepository { get; set; }

        public ITestsRepository TestsRepository { get; set; }

        public ITestUnlocksRepository TestUnlocksRepository { get; set; }

        public IQuestionsRepository QuestionsRepository { get; set; }

        public IModulesRepository ModulesRepository { get; set; }
		
		public ILecturerRepository LecturerRepository { get; set; }

        public IMessageRepository MessageRepository { get; set; }

		public ISubGroupRepository SubGroupRepository { get; set; }

        public IAttachmentRepository AttachmentRepository { get; set; }

        public ILecturesRepository LecturesRepository { get; set; }

        public ILabsRepository LabsRepository { get; set; }
		
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
            ProjectUsersRepository = new ProjectUsersRepository(_dataContext);
            StudentsRepository = new StudentsRepository(_dataContext);
            SubjectRepository = new SubjectRepository(_dataContext);
            TestsRepository = new TestsRepository(_dataContext);
            TestUnlocksRepository = new TestUnlockRepository(_dataContext);
            QuestionsRepository = new QuestionsRepository(_dataContext);
            UsersRepository = new UsersRepository(_dataContext);
            ModulesRepository = new ModulesRepository(_dataContext);
			LecturerRepository = new LecturerRepository(_dataContext);
            MessageRepository = new MessageRepository(_dataContext);
			SubGroupRepository = new SubGroupRepository(_dataContext);
            AttachmentRepository = new AttachmentRepository(_dataContext);
            LecturesRepository = new LecturesRepository(_dataContext);
            LabsRepository = new LabsRepository(_dataContext);
            ProjectUsersRepository = new ProjectUsersRepository(_dataContext);
        }
    }
}
