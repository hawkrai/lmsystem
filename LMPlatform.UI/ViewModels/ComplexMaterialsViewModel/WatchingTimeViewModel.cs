using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.ConceptManagement;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.UserManagement;
using Application.Infrastructure.WatchingTimeManagement;
using LMPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMPlatform.UI.ViewModels.ComplexMaterialsViewModel
{
    public class WatchingTimeViewModel
    {
        public int conceptId;
        public List<ViewsWorm> viewRecords { get; set; }
        public List<Group> groups { get; set; }
        private readonly LazyDependency<IWatchingTimeService> _watchingTimeService = new LazyDependency<IWatchingTimeService>();
        private readonly LazyDependency<IUsersManagementService> _userManahementService = new LazyDependency<IUsersManagementService>();
        private readonly LazyDependency<IGroupManagementService> _groupManahementService = new LazyDependency<IGroupManagementService>();
        private readonly LazyDependency<IConceptManagementService> _conceptManahementService = new LazyDependency<IConceptManagementService>();

        public IWatchingTimeService WatchingTimeService
        {
            get
            {
                return _watchingTimeService.Value;
            }
        }

        public IUsersManagementService UsersManagementService
        {
            get
            {
                return _userManahementService.Value;
            }
        }

        public IGroupManagementService GroupManagementService
        {
            get
            {
                return _groupManahementService.Value;
            }
        }

        public IConceptManagementService ConceptManagementService
        {
            get
            {
                return _conceptManahementService.Value;
            }
        }

        public WatchingTimeViewModel(int conceptId)
        {
            this.conceptId = conceptId;
            var concept = ConceptManagementService.GetById(conceptId);
            groups = GroupManagementService.GetGroups(new Query<Group>(e => e.SubjectGroups.Any(x => x.SubjectId == concept.SubjectId)));
            viewRecords = new List<ViewsWorm>();
            var list = new List<WatchingTime>();
            list = WatchingTimeService.GetAllRecords(conceptId);
            foreach(var item in list)
            {
                viewRecords.Add(new ViewsWorm { Name = UsersManagementService.GetUser(item.UserId).FullName,Seconds = item.Time});
            }

        }

        public class ViewsWorm
        {
            public string Name { get; set; }
            public int Seconds { get; set; }
        }
    }
}