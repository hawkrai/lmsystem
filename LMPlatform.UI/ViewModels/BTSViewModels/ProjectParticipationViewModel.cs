using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Infrastructure.GroupManagement;
using LMPlatform.Data.Infrastructure;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    public class ProjectParticipationViewModel
    {
        private static LmPlatformModelsContext context = new LmPlatformModelsContext();

        [DisplayName("Группа")]
        public string GroupId { get; set; }

        public ProjectParticipationViewModel()
        {   
        }

        public ProjectParticipationViewModel(string groupId)
        {
        }

        public IList<SelectListItem> GetGroups()
        {
            var groups = new GroupManagementService().GetGroups();

            return groups.Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString(CultureInfo.InvariantCulture)
            }).ToList();
        }
    }
}