using System.Web.Mvc;
using Application.Core.UI.Controllers;
using LMPlatform.UI.ViewModels.LmsViewModels;
using WebMatrix.WebData;

namespace LMPlatform.UI.Controllers
{
   public class LmsController : BasicController
    {
        public ActionResult Index()
        {
            var model = new LmsViewModel(WebSecurity.CurrentUserId);
            return View(model);
        }
    }
}
