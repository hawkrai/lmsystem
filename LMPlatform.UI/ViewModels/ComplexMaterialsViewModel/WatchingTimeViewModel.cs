using Application.Core;
using Application.Core.Data;
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
        public List<ViewsWorm> viewRecords { get; set; }
        private readonly LazyDependency<IWatchingTimeService> _watchingTimeService = new LazyDependency<IWatchingTimeService>();
        private readonly LazyDependency<IUsersManagementService> _userManahementService = new LazyDependency<IUsersManagementService>();


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

        public WatchingTimeViewModel(int conceptId)
        {
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