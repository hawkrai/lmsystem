using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMPlatform.UI.Controllers
{
    using Application.Core.UI.Controllers;
    using Application.Infrastructure.GroupManagement;
    using Application.Infrastructure.UserManagement;

    [AllowAnonymous]
    public class ParentalController : BasicController
    {
        [AllowAnonymous]
        public ActionResult Index(string id)
        {
            return View();
        }

        public ActionResult Plan(string id)
        {
            return View();
        }

        public ActionResult Statistics(string id)
        {
            return View();
        }

        public bool IsValidGroup(string groupName)
        {
            return GroupManagementService.GetGroupByName(groupName) != null;
        }

        public IGroupManagementService GroupManagementService
        {
            get
            {
                return ApplicationService<IGroupManagementService>();
            }
        }
    }
}
