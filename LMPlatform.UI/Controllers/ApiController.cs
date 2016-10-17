using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMPlatform.UI.Controllers
{
	using Application.Core;
	using Application.Infrastructure.SubjectManagement;
	using Application.Infrastructure.UserManagement;

	using WebMatrix.WebData;

	public class RemoteApiController : Controller
    {

		private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();

		public ISubjectManagementService SubjectManagementService
		{
			get
			{
				return subjectManagementService.Value;
			}
		}

	    [HttpPost]
	    public ActionResult Login(string userName, string password)
	    {
		    if (WebSecurity.Login(userName, password))
		    {
				var _context = new UsersManagementService();
			    var user = _context.GetUser(userName);

			    return Json(new
				{
					UserName = userName,
					UserId = user.Id
				});
		    }
				
			Response.StatusCode = 401;

			return Json(new
		    {
			    Error = "Введите корректные данные"
		    });
	    }

		[HttpGet]
		public ActionResult GetSubjectByUserId(string userId)
		{
			var data = this.SubjectManagementService.GetUserSubjects(int.Parse(userId));

			var result = data.Where(x => !x.IsArchive).Select(e => new { Id = e.Id, ShortName = e.ShortName, Name = e.Name });

			return Json(result, JsonRequestBehavior.AllowGet);
		}
    }
}
