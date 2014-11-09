using System.Web.Mvc;
using Application.Core;
using Application.Core.UI.Controllers;
using Application.Infrastructure.DPManagement;
using LMPlatform.UI.ViewModels.LmsViewModels;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
    using ViewModels.AdministrationViewModels;

    [Authorize(Roles = "student, lector")]
    public class LmsController : BasicController
    {
        public ActionResult Index()
        {
            if (User.IsInRole("lector") || User.IsInRole("student"))
            {
                var model = new LmsViewModel(WebSecurity.CurrentUserId, User.IsInRole("lector"));
                model.UserActivity = new UserActivityViewModel();

                ViewBag.ShowDpSectionForUser = DpManagementService.ShowDpSectionForUser(WebSecurity.CurrentUserId);
                return View(model);    
            }

            return RedirectToAction("Login", "Account");
        }

        private IDpManagementService DpManagementService
        {
            get { return _diplomProjectManagementService.Value; }
        }

        private readonly LazyDependency<IDpManagementService> _diplomProjectManagementService = new LazyDependency<IDpManagementService>();
    }
}
