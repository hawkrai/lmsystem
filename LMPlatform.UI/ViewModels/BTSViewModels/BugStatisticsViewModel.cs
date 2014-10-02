using System.Collections.Generic;
using System.Linq;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;
using LMPlatform.Models.BTS;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    public class BugStatisticsViewModel
    {
        public int BugQuentity { get; set; }

        public int QuentityOfNewBugs { get; set; }

        public int QuentityOfAssignesBugs { get; set; }

        public int QuentityOfResolvedBugs { get; set; }

        public int QuentityOfClosedBugs { get; set; }

        private List<Bug> _bugs = new List<Bug>();
        private List<BugStatus> _statuses;

        public BugStatisticsViewModel(int id)
        {
            GetProjectBugs(id);
            GetStatusList();
            GetBugQuentity();
            GetQuentityOfNewBugs();
            GetQuentityOfAssignesBugs();
            GetQuentityOfResolvedBugs();
            GetQuentityOfClosedBugs();
        }

        private void GetStatusList()
        {
            var context = new LmPlatformModelsContext();
            _statuses = context.BugStatuses.ToList();
        }

        private int GetQuentity(int n)
        {
            var quentity = 0;
            foreach (var b in _bugs)
            {
                if (b.Status.Name == _statuses[n].Name)
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
            BugQuentity = _bugs.Count;
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