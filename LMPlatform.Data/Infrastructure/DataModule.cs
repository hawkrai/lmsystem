using Application.Core;
using Application.Core.Data;
using LMPlatform.Data.Repositories;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.Data.Infrastructure
{
	public class DataModule
	{
		public static IUnityContainerWrapper Initialize(IUnityContainerWrapper containerWrapper)
		{
			containerWrapper.Register<IUsersRepository, UsersRepository>();
            containerWrapper.Register<IBugsRepository, BugsRepository>();
            containerWrapper.Register<IGroupsRepository, GroupsRepository>();
            containerWrapper.Register<IProjectsRepository, ProjectsRepository>();
            containerWrapper.Register<IStudentsRepository, StudentsRepository>();
            containerWrapper.Register<ISubjectRepository, SubjectRepository>();
            containerWrapper.Register<ITestsRepository, TestsRepository>();
            containerWrapper.Register<IModulesRepository, ModulesRepository>();
            containerWrapper.Register<IMessageRepository, MessageRepository>();
            containerWrapper.Register<IDpContext, LmPlatformModelsContext>();

			return containerWrapper;
		}
	}
}
