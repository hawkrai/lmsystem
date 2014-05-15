using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMPlatform.UI.ViewModels.AdministrationViewModels
{
    using System.Web.Script.Serialization;

    using Application.Core;
    using Application.Core.Constants;
    using Application.Infrastructure.UserManagement;

    using LMPlatform.Models;

    public class AttendanceViewModel
    {
        private readonly LazyDependency<IUsersManagementService> _userManagementService =
           new LazyDependency<IUsersManagementService>();

        public IUsersManagementService UserManagementService
        {
            get { return _userManagementService.Value; }
        }

        public AttendanceViewModel()
        {
            InitializeActivity();
        }

        private void InitializeActivity()
        {
        }
    }
}