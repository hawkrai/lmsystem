using Application.Core;
using Application.Infrastructure.AccountManagement;
using Application.Infrastructure.BugManagement;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.KnowledgeTestsManagement;
using Application.Infrastructure.LecturerManagement;
using Application.Infrastructure.ProjectManagement;
using Application.Infrastructure.StudentManagement;
using Application.Infrastructure.SubjectManagement;
using Application.Infrastructure.UserManagement;

namespace Application.Infrastructure
{
	public class ApplicationServiceModule
	{
		public static IUnityContainerWrapper Initialize(IUnityContainerWrapper containerWrapper)
		{
			containerWrapper.Register<IAccountManagementService, AccountManagementService>();
			containerWrapper.Register<IUsersManagementService, UsersManagementService>();
            containerWrapper.Register<IBugManagementService, BugManagementService>();
            containerWrapper.Register<IGroupManagementService, GroupManagementService>();
            containerWrapper.Register<ITestsManagementService, TestsManagementService>();
            containerWrapper.Register<IProjectManagementService, ProjectManagementService>();
            containerWrapper.Register<IStudentManagementService, StudentManagementService>();
            containerWrapper.Register<ISubjectManagementService, SubjectManagementService>();
            containerWrapper.Register<IModulesManagementService, ModulesManagementService>();
			containerWrapper.Register<ILecturerManagementService, LecturerManagementService>();

			return containerWrapper;
		}
	}
}
