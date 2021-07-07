using System.Web.Mvc;
using Application.Core.Data;
using Application.Core.UI.Controllers;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Models;
using LMPlatform.UI.Attributes;

namespace LMPlatform.UI.Controllers
{
    [JwtAuth]
    public class HomeController : BasicController
    {
        public ActionResult Index()
        {
            if (this.User.IsInRole("admin")) return this.RedirectToAction("Index", "Administration");

            if (this.User.IsInRole("student"))
            {
                var repository = new RepositoryBase<LmPlatformModelsContext, Student>(new LmPlatformModelsContext());
                var student =
                    repository.GetBy(
                        new PageableQuery<Student>(e => e.User.UserName == this.User.Identity.Name)
                            .Include(e => e.User));

                if (student.Confirmed != null && !student.Confirmed.Value)
                {
                    this.TempData["ComfirmedError"] =
                        @"Ваш аккаунт не подтвержден. Обратитесь к преподавателю для подтверждения аккаунта";
                    return this.RedirectToAction("Login", "Account");
                }
            }

            return this.RedirectToAction("Index", "Lms");
        }
    }
}