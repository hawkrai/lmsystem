using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Application.Core.UI.HtmlHelpers;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    public class ProjectUserListViewModel : BaseNumberedGridItem
    {
        [DisplayName("Участник")]
        public string UserName { get; set; }

        [DisplayName("Роль")]
        public string RoleName { get; set; }

        [DisplayName("Действие")]
        public HtmlString Action { get; set; }

        public int Id { get; set; }

        public int ProjectId { get; set; }

        private static LmPlatformModelsContext context = new LmPlatformModelsContext();

        public static ProjectUserListViewModel FromProjectUser(ProjectUser projectUser, string htmlLinks)
        {
            var model = FromProjectUser(projectUser);
            model.Action = new HtmlString(htmlLinks);

            return model;
        }

        public static ProjectUserListViewModel FromProjectUser(ProjectUser projectUser)
        {
            return new ProjectUserListViewModel
            {
                Id = projectUser.Id,
                UserName = projectUser.User.FullName,
                RoleName = GetRoleName(projectUser.ProjectRoleId),
                ProjectId = projectUser.ProjectId
            };
        }

        public static string GetProjectCreatorName(int projectId)
        {
            var project = context.Projects.Find(projectId);
            var creator = context.Users.Find(project.CreatorId);
            return creator.FullName;
        }

        public static string GetRoleName(int id)
        {
            var role = context.ProjectRoles.Find(id);
            return role.Name;
        }
    }
}