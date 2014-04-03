using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
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

        public void CreateStaticLists()
        {
            _bugs = new List<Bug>();
            _statuses = new List<BugStatus>();
        }

        public ProjectsViewModel()
        {
            CreateStaticLists();
            if (GetProjectNames().Count != 0)
            {
                var model = ProjectManagementService.GetProject(Convert.ToInt32(GetProjectNames().First().Value));
                ProjectId = model.Id;
                SetParams(model);
            }
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
            CreatorName = model.Creator.FullName;
            SetBugStatistics(ProjectId);
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

        public int QuentityOfNewBugs { get; set; }

        public int QuentityOfAssignesBugs { get; set; }

        public int QuentityOfResolvedBugs { get; set; }

        public int QuentityOfClosedBugs { get; set; }

        private static List<Bug> _bugs;
        private static List<BugStatus> _statuses;

        public void SetBugStatistics(int id)
        {
            GetProjectBugs(id);
            GetStatusList();
            GetBugQuentity();
            if (BugQuentity != 0)
            {
                GetQuentityOfNewBugs();
                GetQuentityOfAssignesBugs();
                GetQuentityOfResolvedBugs();
                GetQuentityOfClosedBugs();
            }
        }

        private void GetStatusList()
        {
            var context = new LmPlatformModelsContext();
            _statuses = context.BugStatuses.ToList();
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

        private void GetQuentityOfClosedBugs()
        {
            QuentityOfClosedBugs = GetQuentity(3);
        }

        private void GetQuentityOfResolvedBugs()
        {
            QuentityOfResolvedBugs = GetQuentity(2);
        }

        private void GetQuentityOfAssignesBugs()
        {
            QuentityOfAssignesBugs = GetQuentity(1);
        }

        private void GetQuentityOfNewBugs()
        {
            QuentityOfNewBugs = GetQuentity(0);
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
    }
}
