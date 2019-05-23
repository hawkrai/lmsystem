using System.Web.Mvc;
using Application.Core.UI.Controllers;

namespace LMPlatform.UI.Controllers
{
	using Application.Core.Data;

	using LMPlatform.Data.Infrastructure;
	using LMPlatform.Models;

	public class HomeController : BasicController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                //return RedirectToAction("Login", "Account");
				return RedirectToAction("Index", "Main");
            }

            if (User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Administration");
            }

			if (User.IsInRole("student"))
			{
				var repository = new RepositoryBase<LmPlatformModelsContext, Student>(new LmPlatformModelsContext());
				var student = repository.GetBy(new PageableQuery<Student>(e => e.User.UserName == User.Identity.Name).Include(e => e.User));

				//if (student.Confirmed != null && !student.Confirmed.Value)
				//{
				//	TempData["ComfirmedError"] = @"Ваш аккаунт не подтвержден. Обратитесь к преподавателю для подтверждения аккаунта";
				//	return RedirectToAction("Login", "Account");
				//}
			}

            return RedirectToAction("Index", "Lms");
        }
    }
}
