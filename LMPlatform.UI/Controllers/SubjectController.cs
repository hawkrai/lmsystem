using System.Web.Mvc;
using Application.Core.UI.Controllers;
using LMPlatform.UI.ViewModels.SubjectViewModels;

namespace LMPlatform.UI.Controllers
{
    public class SubjectController : BasicController 
    {
        public ActionResult Index(int subjectId)
        {
            var model = new SubjectManagementViewModel(subjectId);
            return View(model);
        }
    }
}