using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    public class ProjectUserListViewModel
    {
        [DisplayName("Участник")]
        public string UserName { get; set; }

        [DisplayName("Роль")]
        public string RoleName { get; set; }

        public static ProjectUserListViewModel FromProjectUser(ProjectUser projectUser)
        {
            return new ProjectUserListViewModel
            {
                UserName = projectUser.User.FullName,
                RoleName = projectUser.Role.Name
            };
        }
    }
}