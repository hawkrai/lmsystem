using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Application.Core;
using Application.Infrastructure.ProjectManagement;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;
using LMPlatform.Models.BTS;
using WebMatrix.WebData;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    public class ProjectsViewModel
    {
        private readonly LazyDependency<IProjectsRepository> _projectsRepository = new LazyDependency<IProjectsRepository>();
        private readonly LazyDependency<IProjectManagementService> _projectManagementService = new LazyDependency<IProjectManagementService>(); 

        public IProjectsRepository ProjectsRepository
        {
            get
            {
                return _projectsRepository.Value;
            }
        }

        public IProjectManagementService ProjectManagementService
        {
            get
            {
                return _projectManagementService.Value;
            }
        }

        public int ProjectId { get; set; }

        [DisplayName("Тема проекта")]
        public string Title { get; set; }

        [DisplayName("Дата создания")]
        public DateTime CreationDate { get; set; }

        [DisplayName("Создатель")]
        public int CreatorId { get; set; }

        [DisplayName("Создатель")]
        public User Creator { get; set; }

        [DisplayName("Создатель")]
        public string CreatorName { get; set; }

        public string CommentText { get; set; }

        public void CreateStaticLists()
        {
            _bugs = new List<Bug>();
            _statuses = new List<BugStatus>();
            _severities = new List<BugSeverity>();
        }

        public ProjectsViewModel(int projectId)
        {
            CreateStaticLists();
            var model = ProjectManagementService.GetProject(projectId);
            ProjectId = projectId;
            SetParams(model);
        }

        public void SetParams(Project model)
        {
            Title = model.Title;
            CreationDate = model.CreationDate;

            //CreatorName = model.Creator.FullName;
            GetCreatorName(model.CreatorId);
            SetBugStatistics(ProjectId);
        }

        public void GetCreatorName(int id)
        {
            CreatorName = ProjectManagementService.GetCreatorName(id);
        }

        public IList<SelectListItem> GetProjectNames()
        {
            var projects = ProjectManagementService.GetProjects();

            return projects.Select(e => new SelectListItem
            {
                Text = e.Title,
                Value = e.Id.ToString(CultureInfo.InvariantCulture)
            }).ToList();
        }

        public int BugQuentity { get; set; }

        public List<BugQuentity> QuentityOfBugsByType { get; set; }

        public List<BugQuentity> QuentityOfBugsBySeverity { get; set; }

        public string BugStatusesJson { get; set; }

        private static List<Bug> _bugs;
        private static List<BugStatus> _statuses;
        private static List<BugSeverity> _severities;

        public List<ProjectComment> GetProjectComments()
        {
            var commentList = ProjectManagementService.GetProjectComments(ProjectId).ToList();
            return commentList;
        }

        public void SetBugStatistics(int id)
        {
            GetProjectBugs(id);
            GetBugPropertyList();
            GetBugQuentity();

            if (BugQuentity != 0)
            {
                GetQuentityOfBugsByType();
                GetQuentityOfBugsBySeverity();

                var dictionary = new Dictionary<string, int>();
                foreach (var quentity in QuentityOfBugsByType)
                {
                    dictionary.Add(quentity.Name, quentity.Quentity);
                }

                var jsonSerialiser = new JavaScriptSerializer();

                BugStatusesJson = jsonSerialiser.Serialize(dictionary);
            }
        }

        private void GetBugPropertyList()
        {
            var context = new LmPlatformModelsContext();
            _statuses = context.BugStatuses.ToList();
            _severities = context.BugSeverities.ToList();
        }

        private int GetStatusIdByName(string name)
        {
            var context = new LmPlatformModelsContext();
            var statuses = context.BugStatuses.ToList();
            var id = 0;
            foreach (var n in statuses)
            {
                if (n.Name == name)
                {
                    id = n.Id;
                }
            }

            return id;
        }

        private int GetSeverityIdByName(string name)
        {
            var context = new LmPlatformModelsContext();
            var severities = context.BugSeverities.ToList();
            var id = 0;
            foreach (var n in severities)
            {
                if (n.Name == name)
                {
                    id = n.Id;
                }
            }

            return id;
        }

        private int GetQuentity(int n)
        {
            var quentity = 0;
            foreach (var b in _bugs)
            {
                if (b.StatusId == GetStatusIdByName(_statuses[n].Name))
                {
                    quentity++;
                }
            }

            return quentity;
        }

        private int GetQuentityBySeverity(int n)
        {
            var quentity = 0;
            foreach (var b in _bugs)
            {
                if (b.SeverityId == GetSeverityIdByName(_severities[n].Name))
                {
                    quentity++;
                }
            }

            return quentity;
        }

        private void GetQuentityOfBugsByType()
        {
            QuentityOfBugsByType = new List<BugQuentity>();

            for (int i = 0; i < _statuses.Count; i++)
            {
                QuentityOfBugsByType.Add(new BugQuentity
                {
                    Name = _statuses[i].Name,
                    Quentity = GetQuentity(i)
                });
            }
        }

        public void GetQuentityOfBugsBySeverity()
        {
            QuentityOfBugsBySeverity = new List<BugQuentity>();

            for (int i = 0; i < _severities.Count; i++)
            {
                QuentityOfBugsBySeverity.Add(new BugQuentity
                {
                    Name = _severities[i].Name,
                    Quentity = GetQuentityBySeverity(i)
                });
            }
        }

        private void GetBugQuentity()
        {
            if (_bugs != null)
            {
                BugQuentity = _bugs.Count();
            }
            else
            {
                BugQuentity = 0;
            }
        }

        public void GetProjectBugs(int id)
        {
            var context = new LmPlatformRepositoriesContainer().BugsRepository.GetAll();
            foreach (var b in context)
            {
                if (b.ProjectId == id)
                {
                    _bugs.Add(b);
                }
            }
        }

        public void SaveComment(string comment)
        {
            var currentUserId = WebSecurity.CurrentUserId;
            var newComment = new ProjectComment
            {
                CommentText = comment,
                ProjectId = ProjectId,
                UserId = currentUserId,
                CommentingDate = DateTime.Now
            };
            ProjectManagementService.SaveComment(newComment);
        }
    }
}
