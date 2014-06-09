using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Application.Core.UI.HtmlHelpers;
using Application.Infrastructure.ProjectManagement;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Models;
using WebMatrix.WebData;

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
    }
}