using Application.Infrastructure.AccountManagement;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.KnowledgeTestsManagement;
using Application.Infrastructure.StudentManagement;
using Application.Infrastructure.SubjectManagement;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace LMPlatform.UI
{
    using Application.Infrastructure.UserManagement;

    using LMPlatform.Data.Repositories;
    using LMPlatform.Data.Repositories.RepositoryContracts;

    public static class Bootstrapper
    {
        public static void Initialize()
        {
            var locator = new UnityServiceLocator(_ConfigureUnityContainer());

            ServiceLocator.SetLocatorProvider(() => locator);
        }

        public static IUnityContainer RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IAccountManagementService, AccountManagementService>();
            container.RegisterType<IGroupsRepository, GroupsRepository>();
            container.RegisterType<IStudentsRepository, StudentsRepository>();
            container.RegisterType<IUsersRepository, UsersRepository>();
            container.RegisterType<ITestsRepository, TestsRepository>();
            container.RegisterType<IUsersManagementService, UsersManagementService>();
            container.RegisterType<ISubjectRepository, SubjecRepository>();
            container.RegisterType<IStudentManagementService, StudentManagementService>();
            container.RegisterType<ISubjectManagementService, SubjectManagementService>();
            container.RegisterType<ITestsManagementService, TestsManagementService>();
            container.RegisterType<IGroupManagementService, GroupManagementService>();
            container.RegisterType<IBugsRepository, BugsRepository>();
            container.RegisterType<IProjectsRepository, ProjectsRepository>();
            return container;
        }

        private static IUnityContainer _ConfigureUnityContainer()
        {
            var container = new UnityContainer();

            RegisterTypes(container);

            return container;
        }
    }
}