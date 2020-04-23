using System.Collections.Generic;
using System.Linq;
using Application.Core.Data;
using Application.SearchEngine.SearchMethods;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;
using System;
using Application.Infrastructure.FilesManagement;
using LMPlatform.Models.BTS;
using WebMatrix.WebData;

namespace Application.Infrastructure.ProjectManagement
{
    using Core;
    using BugManagement;
    using UserManagement;

    public class ProjectManagementService : IProjectManagementService
    {
        public List<Project> GetUserProjects(int userId, int pageSize, int pageNumber, string sortingPropertyName, bool desc, string searchString)
        {
			if (searchString != null && searchString.Length < 3)
                searchString = null;
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			return repositoriesContainer.ProjectsRepository.GetUserProjects(userId, pageSize, (pageNumber - 1) * pageSize, searchString, sortingPropertyName, desc);
        }

        public int GetUserProjectsCount(int userId, string searchString)
        {
            if(searchString != null && searchString.Length < 3)
                searchString = null;
            using var repositoriesContainer = new LmPlatformRepositoriesContainer();
            return repositoriesContainer.ProjectsRepository.GetUserProjectsCount(userId, searchString);
        }

        public Project GetProjectWithData(int id, bool withBugsAndMembers = false)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        return repositoriesContainer.ProjectsRepository.GetProjectWithData(id, withBugsAndMembers);
        }

        public List<Project> GetUserProjectParticipations(int userId, int pageSize, int pageNumber, string sortingPropertyName, bool desc, string searchString)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        return repositoriesContainer.ProjectsRepository.GetUserProjectParticipations(userId, pageSize, (pageNumber - 1) * pageSize, searchString, sortingPropertyName, desc);
        }
        public int GetUserProjectParticipationsCount(int userId, string searchString)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        return repositoriesContainer.ProjectsRepository.GetUserProjectParticipationsCount(userId, searchString);
        }

        public List<Student> GetStudentsGroupParticipations(int groupId, int pageSize, int pageNumber)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        return repositoriesContainer.ProjectsRepository.GetStudentsGroupParticipations(groupId, pageSize, (pageNumber - 1) * pageSize);
        }

        public int GetStudentsGroupParticipationsCount(int groupId)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        return repositoriesContainer.ProjectsRepository.GetStudentsGroupParticipationsCount(groupId);
        }

        public Project GetProject(int projectId, bool includeBugs = false, bool includeUsers = false)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var query = new Query<Project>(e => e.Id == projectId).Include(e => e.Creator);
                if (includeBugs)
                {
                    query.Include(e => e.Bugs);
                }

                if (includeUsers)
                {
                    query.Include(e => e.ProjectUsers);
                }

                return repositoriesContainer.ProjectsRepository.GetBy(query);
            }
        }

        public List<ProjectUser> GetProjectUsers(int projectId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return
                    repositoriesContainer.ProjectUsersRepository.GetAll(new Query<ProjectUser>(e => e.ProjectId == projectId).Include(e => e.User))
                        .ToList();
            }
        }

        public List<ProjectUser> GetProjectsOfUser(int userId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.ProjectUsersRepository.GetAll(new Query<ProjectUser>(e => e.UserId == userId).Include(e => e.Project).Include(e => e.User).Include(e => e.ProjectRole)).ToList();
            }
        }

        public ProjectUser GetProjectUser(int projectUserId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.ProjectUsersRepository.GetBy(new Query<ProjectUser>(e => e.Id == projectUserId).Include(e => e.Project).Include(e => e.User).Include(e => e.ProjectRole));
            }
        }

        public List<ProjectComment> GetProjectComments(int projectId)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        return repositoriesContainer.ProjectCommentsRepository.GetAll(
		        new Query<ProjectComment>(e => e.ProjectId == projectId)
			        .Include(e => e.User)
			        .Include(e => e.User.Lecturer)
			        .Include(e => e.User.Student)).ToList();
        }

        public IPageableList<ProjectUser> GetProjectUsers(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null)
        {
            var query = new PageableQuery<ProjectUser>(pageInfo);
            query.Include(e => e.User);
            if (!string.IsNullOrEmpty(searchString))
            {
                query.AddFilterClause(
                    e => e.User.FullName.ToLower().StartsWith(searchString) || e.User.FullName.ToLower().Contains(searchString));
            }

            query.OrderBy(sortCriterias);
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.ProjectUsersRepository.GetPageableBy(query);
            }
        }

        public List<Project> GetProjects()
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.ProjectsRepository.GetAll(new Query<Project>().Include(e => e.Creator)).ToList();
            }
        }

        public IPageableList<Project> GetProjects(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null)
        {
            var query = new PageableQuery<Project>(pageInfo);
            query.Include(e => e.Creator.Lecturer)
                .Include(e => e.Creator.Student)
                .Include(e => e.ProjectUsers);
            if(!string.IsNullOrEmpty(searchString))
            {
                query.AddFilterClause(
                    e => e.Title.ToLower().StartsWith(searchString) || e.Title.ToLower().Contains(searchString));
            }

            query.OrderBy(sortCriterias);
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.ProjectsRepository.GetPageableBy(query);
            }
        }

        public IPageableList<Project> GetUserProjects(int userId, string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null)
        {
            var query = new PageableQuery<Project>(pageInfo);
            query.Include(e => e.Creator.Lecturer)
                .Include(e => e.Creator.Student)
                .Include(e => e.ProjectUsers)
                .AddFilterClause(e => e.ProjectUsers.Any(e2 => e2.UserId == userId));
            if(!string.IsNullOrEmpty(searchString))
            {
                query.AddFilterClause(
                    e => e.Title.ToLower().StartsWith(searchString) || e.Title.ToLower().Contains(searchString));
            }

            query.OrderBy(sortCriterias);
            using(var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.ProjectsRepository.GetPageableBy(query);
            }
        }

        //public void AssingRole(int userId, int projectId, int roleId)
        public void AssingRole(ProjectUser projectUser)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.ProjectUsersRepository.Save(projectUser);
                repositoriesContainer.ApplyChanges();
            }
        }

        public void SaveComment(ProjectComment comment)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.ProjectCommentsRepository.Save(comment);
                repositoriesContainer.ApplyChanges();
            }
        }

        public Project SaveProject(Project project)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.ProjectsRepository.Save(project);
                repositoriesContainer.ApplyChanges();
            }
            new ProjectSearchMethod().AddToIndex(project);
            return project;
        }

        public ProjectMatrixRequirement CreateMatrixRequirement(ProjectMatrixRequirement requirement)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        repositoriesContainer.ProjectMatrixRequirementsRepository.Save(requirement);
	        repositoriesContainer.ApplyChanges();
	        return requirement;
        }

        public void UpdateProject(Project project)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.ProjectsRepository.Save(project);
                repositoriesContainer.ApplyChanges();
            }
            new ProjectSearchMethod().UpdateIndex(project);
        }

        public void DeleteProject(int projectId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var project =
                    repositoriesContainer.ProjectsRepository.GetBy(
                        new Query<Project>().AddFilterClause(u => u.Id == projectId));
                repositoriesContainer.ProjectsRepository.DeleteProject(project);
                repositoriesContainer.ApplyChanges();
            }
            new ProjectSearchMethod().DeleteIndex(projectId);
        }

        public void ClearProject(int projectId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var project = repositoriesContainer.ProjectsRepository.GetBy(new Query<Project>(e => e.Id == projectId));
                var projectUserList =
                    repositoriesContainer.ProjectUsersRepository.GetAll(
                        new Query<ProjectUser>().AddFilterClause(u => u.ProjectId == projectId));
                foreach (var projectUser in projectUserList)
                {
                    if (projectUser.UserId != project.CreatorId)
                    {
                        repositoriesContainer.ProjectUsersRepository.DeleteProjectUser(projectUser);   
                    }
                }

                var commentList =
                    repositoriesContainer.ProjectCommentsRepository.GetAll(
                        new Query<ProjectComment>().AddFilterClause(u => u.ProjectId == projectId));
                foreach (var comment in commentList)
                {
                    repositoriesContainer.ProjectCommentsRepository.DeleteComment(comment);
                }

                var bugList =
                    repositoriesContainer.BugsRepository.GetAll(
                        new Query<Bug>().AddFilterClause(u => u.ProjectId == projectId));
                foreach (var bug in bugList)
                {
                    repositoriesContainer.BugsRepository.DeleteBug(bug);   
                }
            }
        }

        public void DeleteProjectUser(int projectUserId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var projectUser = repositoriesContainer.ProjectUsersRepository.GetBy(new Query<ProjectUser>().AddFilterClause(u => u.Id == projectUserId));
                repositoriesContainer.ProjectUsersRepository.DeleteProjectUser(projectUser);
                repositoriesContainer.ApplyChanges();
            }
        }

        public bool IsUserAssignedOnProject(int userId, int projectId)
        {
            var isAssigned = false;
            var projectUsers = new ProjectManagementService().GetProjectUsers(projectId);
            foreach (var user in projectUsers)
            {
                if (user.UserId == userId)
                {
                    isAssigned = true;
                }
            }

            return isAssigned;
        }

        public string GetCreatorName(int id)
        {
            return UserManagementService.GetUser(id).FullName;
        }

        public void DeleteUserFromProject(int userId, int projectId)
        {
            var project = this.GetProject(projectId, includeBugs: true, includeUsers: true);

            if (project.CreatorId == userId)
            {
                this.DeleteProject(projectId);
                return;
            }

            var projectUsers = project.ProjectUsers.Where(e => e.UserId == userId).ToList();

            foreach (var projectUser in projectUsers)
            {
                this.DeleteProjectUser(projectUser.Id);
            }

            var reportedBugs = project.Bugs.Where(e => e.ReporterId == userId);

            foreach (var bug in reportedBugs)
            {
                bug.ReporterId = project.CreatorId;
                bug.EditorId = WebSecurity.CurrentUserId;

                BugManagementService.SaveBug(bug);
            }

            var bugs = project.Bugs.Where(e => e.AssignedDeveloperId == userId || e.EditorId == userId);

            foreach (var bug in bugs)
            {
                if (bug.EditorId == userId)
                {
                    bug.EditorId = 0;
                }
                else
                {
                    bug.AssignedDeveloperId = 0;
                    bug.EditorId = WebSecurity.CurrentUserId;
                }

                BugManagementService.SaveBug(bug);
            }
        }

        public Attachment SaveAttachment(int projectId, Attachment attachment)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        var query = new Query<Project>(e => e.Id == projectId);
	        var project = repositoriesContainer.ProjectsRepository.GetBy(query);

	        if (string.IsNullOrEmpty(project.Attachments))
	        {
		        project.Attachments = GetGuidFileName();
	        }

	        FilesManagementService.SaveFile(attachment, project.Attachments);

	        attachment.PathName = project.Attachments;
	        repositoriesContainer.AttachmentRepository.Save(attachment);
	        return attachment;
        }

        public List<Attachment> GetAttachments(int projectId)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        var query = new Query<Project>(e => e.Id == projectId);
	        var project = repositoriesContainer.ProjectsRepository.GetBy(query);

	        if (string.IsNullOrEmpty(project.Attachments))
	        {
		        return new List<Attachment>();
	        }
	        return FilesManagementService.GetAttachments(project.Attachments).ToList();
        }

        public Attachment GetAttachment(int projectId, string fileName)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        var query = new Query<Project>(e => e.Id == projectId);
	        var project = repositoriesContainer.ProjectsRepository.GetBy(query);

	        if (string.IsNullOrEmpty(project.Attachments))
	        {
		        return null;
	        }
	        return FilesManagementService.GetAttachments(project.Attachments).First(e => e.FileName == fileName);
        }

        public bool DeleteAttachment(int projectId, string fileName)
        {
	        using var repositoriesContainer = new LmPlatformRepositoriesContainer();
	        var query = new Query<Project>(e => e.Id == projectId);
	        var project = repositoriesContainer.ProjectsRepository.GetBy(query);
            var attachment = repositoriesContainer.AttachmentRepository.GetBy(new Query<Attachment>(e => e.PathName == project.Attachments && e.FileName == fileName));
            FilesManagementService.DeleteFileAttachment(attachment);
	        return true;
        }

        private readonly LazyDependency<IBugManagementService> _bugManagementService = new LazyDependency<IBugManagementService>();
        private readonly LazyDependency<IUsersManagementService> _userManagementService = new LazyDependency<IUsersManagementService>();
        private readonly LazyDependency<IFilesManagementService> _filesManagementService = new LazyDependency<IFilesManagementService>();
        
        public IBugManagementService BugManagementService
        {
            get { return _bugManagementService.Value; }
        }

        public IUsersManagementService UserManagementService
        {
            get { return _userManagementService.Value; }
        }

        public IFilesManagementService FilesManagementService
        {
            get { return _filesManagementService.Value; }
        }

        private string GetGuidFileName()
        {
            return $"P{Guid.NewGuid().ToString("N").ToUpper()}";
        }
    }
}
