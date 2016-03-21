using System;
using System.Linq;
using System.Web.Mvc;
using Application.Infrastructure.UserManagement;
using LMPlatform.UI.ViewModels.LmsViewModels;

namespace LMPlatform.UI.Controllers
{
	public class ProfileController : Controller
	{
		[HttpPost]
		public ActionResult GetProfileInfo(string userLogin)
		{
			var model = new ProfileVewModel();

			var service = new UsersManagementService();

			var user = service.GetUser(userLogin);

			model.UserType = user.Lecturer != null ? "1" : "2";
			model.Avatar = user.Avatar;
			model.SkypeContact = user.SkypeContact;
			model.Email = user.Email;
			model.Phone = user.Phone;
			model.About = user.About;

			model.LastLogitData = user.AttendanceList.LastOrDefault().ToString("dd/MM/yyyy hh:mm:ss");
			if (user.Lecturer != null)
			{
				model.Name = user.Lecturer.FirstName + " " + user.Lecturer.LastName;
				model.Skill = user.Lecturer.Skill;
			}
			else
			{
				model.Name = user.Student.FirstName + " " + user.Student.LastName;
				var course = int.Parse(DateTime.Now.Year.ToString()) - int.Parse(user.Student.Group.StartYear);
				if (DateTime.Now.Month >= 9)
				{
					course += 1;
				}

				model.Skill = course > 5 ? "Окончил (-а)" : course + " курс";
			}


			return Json(model);
		}
	}
}