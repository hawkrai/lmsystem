using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Application.Core.Data;
using Application.Infrastructure.UserManagement;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Models;
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

		[HttpGet]
		public ActionResult GetAccessCode()
		{
			var repository = new RepositoryBase<LmPlatformModelsContext, AccessCode>(new LmPlatformModelsContext());

			var code = repository.GetAll().OrderBy(e => e.Id).FirstOrDefault();

			return Json(code != null ? code.Number : string.Empty, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GenerateCode()
		{
			var repository = new RepositoryBase<LmPlatformModelsContext, AccessCode>(new LmPlatformModelsContext());

			var model = new AccessCode()
			{
				Date = DateTime.Now,
				Number = RandomString(10,false)

			};

			repository.Save(model);

			return Json(model.Number, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Generates random strings with a given length
		/// </summary>
		/// <param name="size">Size of the string</param>
		/// <param name="lowerCase">If true, generate lowercase string</param>
		/// <returns>Random string</returns>
		private string RandomString(int size, bool lowerCase)
		{
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			char ch;
			for (int i = 1; i < size + 1; i++)
			{
				ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
				builder.Append(ch);
			}
			if (lowerCase)
				return builder.ToString().ToLower();
			else
				return builder.ToString();
		}
	}
}