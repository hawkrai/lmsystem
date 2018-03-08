using System;
using System.Diagnostics.CodeAnalysis;
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
    using System.Collections.Generic;

    using Application.Core;
    using Application.Infrastructure.DPManagement;
    using Application.Infrastructure.ProjectManagement;
    using Application.Infrastructure.SubjectManagement;

    using LMPlatform.UI.ViewModels.AdministrationViewModels;

    using WebMatrix.WebData;
	using Application.Infrastructure.CPManagement;


    public class ProfileController : Controller
	{
		[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
		private readonly LazyDependency<ICPManagementService> _cpManagementService = new LazyDependency<ICPManagementService>();

		private ICPManagementService CpManagementService
		{
			get { return _cpManagementService.Value; }
		}

        private IDpManagementService DpManagementService
        {
            get { return _diplomProjectManagementService.Value; }
        }

        private readonly LazyDependency<IDpManagementService> _diplomProjectManagementService = new LazyDependency<IDpManagementService>();

        private IProjectManagementService ProjectManagementService
        {
            get { return projectManagementService.Value; }
        }

        private readonly LazyDependency<IProjectManagementService> projectManagementService = new LazyDependency<IProjectManagementService>();

        public ActionResult Page(string userName = "")
        {
            if (User.IsInRole("lector") || User.IsInRole("student"))
            {
                var model = new LmsViewModel(WebSecurity.CurrentUserId, User.IsInRole("lector"));
                model.UserActivity = new UserActivityViewModel();

                ViewBag.ShowDpSectionForUser = DpManagementService.ShowDpSectionForUser(WebSecurity.CurrentUserId);

                ViewData["userName"] = string.IsNullOrEmpty(userName) || WebSecurity.CurrentUserName == userName ? WebSecurity.CurrentUserName : userName;

                return View(model);
            }
            else if (User.IsInRole("admin"))
 {
                var model = new LmsViewModel(WebSecurity.GetUserId(userName), User.IsInRole("lector"));
                model.UserActivity = new UserActivityViewModel();
                ViewData["userName"] = string.IsNullOrEmpty(userName) || WebSecurity.CurrentUserName == userName ? WebSecurity.CurrentUserName : userName;
                return View(model);
            }


            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult GetProfileStatistic(string userLogin)
        {
            var userService = new UsersManagementService();

            var subjectService = new SubjectManagementService();

            var user = userService.GetUser(userLogin);

            var labs = 0;
            var lects = 0;

            if (user.Lecturer == null)
            {
                labs = subjectService.LabsCountByStudent(user.Id);
            }
            else
            {
                labs = subjectService.LabsCountByLector(user.Id);
            }

            return Json(new
            {
                Labs = labs,
                Lects = lects
            });
        }

		[HttpPost]
        public ActionResult GetProfileInfoCalendar(string userLogin)
		{
			var userService = new UsersManagementService();

            var subjectService = new SubjectManagementService();
           
            var user = userService.GetUser(userLogin);

			var labsEvents = new List<ProfileCalendarViewModel>();
			var lectEvents = new List<ProfileCalendarViewModel>();

			if (user.Lecturer == null)
			{
				labsEvents = subjectService.GetGroupsLabEvents(user.Student.GroupId, user.Id).Select(
					e => new ProfileCalendarViewModel() { color = e.Color, title = e.Title, start = e.Start, subjectId = e.SubjectId }).ToList();

				lectEvents =
				subjectService.GetLecturesEvents(user.Student.GroupId, user.Id)
					.Select(e => new ProfileCalendarViewModel() { color = e.Color, title = e.Title, start = e.Start, subjectId = e.SubjectId })
					.ToList();
			}
			else
			{
				labsEvents =
					subjectService.GetLabEvents(user.Id)
								.Select(e => new ProfileCalendarViewModel() { color = e.Color, title = e.Title, start = e.Start, subjectId = e.SubjectId })
								.ToList();

				lectEvents =
				subjectService.GetLecturesEvents(user.Id)
					.Select(e => new ProfileCalendarViewModel() { color = e.Color, title = e.Title, start = e.Start, subjectId = e.SubjectId })
					.ToList();
			}            

			return Json(new
			                {
			                    Labs = labsEvents,
                                Lect = lectEvents
			                });
		}

        [HttpPost]
        public ActionResult GetProfileInfoSubjects(string userLogin)
        {
            var userService = new UsersManagementService();

            var subjectService = new SubjectManagementService();

            var user = userService.GetUser(userLogin);

            List<Subject> model;

            if (user.Lecturer == null)
            {
                model = subjectService.GetSubjectsByStudent(user.Id);
            }
            else
            {
                model = subjectService.GetSubjectsByLector(user.Id);
            }


			var returnModel = new List<object>();

	        foreach (var subject in model)
	        {
		        returnModel.Add(new
                                              {
                                                  Name = subject.Name,
                                                  Id = subject.Id,
                                                  ShortName = subject.ShortName,
												  Color = subject.Color,
												  Completing = subjectService.GetSubjectCompleting(subject.Id, user.Lecturer != null ? "L" : "S", user.Student)
                                              });
	        }

            return Json(returnModel);
        }

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
				model.Name = user.Lecturer.LastName + " " + user.Lecturer.FirstName + " " + user.Lecturer.MiddleName;
                model.Skill = user.Lecturer.Skill;
            }
            else
            {
				model.Name = user.Student.LastName + " " + user.Student.FirstName + " " + user.Student.MiddleName;
                var course = int.Parse(DateTime.Now.Year.ToString()) - int.Parse(user.Student.Group.StartYear);
                if (DateTime.Now.Month >= 9)
                {
                    course += 1;
                }

                model.Skill = course > 5 ? "Окончил (-а)" : course + " курс";

                model.Group = user.Student.Group.Name;
				model.GroupId = user.Student.Group.Id;
            }


            return Json(model);
        }

        [HttpPost]
        public ActionResult GetUserProject(string userLogin)
        {
            var service = new UsersManagementService();

            var user = service.GetUser(userLogin);
            var project = ProjectManagementService.GetProjectsOfUser(user.Id);

            return Json(project.Select(e => new
                                                {
                                                    Name = e.Project.Title
                                                }));
        }


        //[HttpPost]
        //public ActionResult GetStudentAttendance(string userLogin)
        //{
        //    var service = new UsersManagementService();

        //    var user = service.GetUser(userLogin);

        //    if (user.Lecturer != null)
        //    {
        //        return null;
        //    }

        //    var subjectService = new SubjectManagementService();

        //    var attendance = subjectService.StudentAttendance(user.Id);
        //}
        
		[HttpGet]
		public ActionResult GetAccessCode()
		{
			var repository = new RepositoryBase<LmPlatformModelsContext, AccessCode>(new LmPlatformModelsContext());

			var code = repository.GetAll().OrderBy(e => e.Id).ToList().LastOrDefault();

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

		[HttpPost]
		public ActionResult GetNews(string userLogin)
		{
			var service = new UsersManagementService();

			var subjectService = new SubjectManagementService();

			var user = service.GetUser(userLogin);

			var news  = new List<SubjectNews>();

			if (user.Lecturer != null)
			{
				news = subjectService.GetNewsByLector(user.Id);
			}
			else
			{
				news = subjectService.GetNewsByGroup(user.Student.GroupId);
			}

			return Json(news.OrderByDescending(e => e.EditDate).ToList());
		}

		[HttpPost]
		public ActionResult GetMiniInfoCalendar(string userLogin)
		{
			var userService = new UsersManagementService();

			var subjectService = new SubjectManagementService();

			var user = userService.GetUser(userLogin);

			var labsEvents =
				subjectService.GetLabEvents(user.Id)
					.Select(e => new ProfileCalendarViewModel() { color = e.Color, title = e.Title, start = e.Start })
					.ToList();

			var lectEvents =
				subjectService.GetLecturesEvents(user.Id)
					.Select(e => new ProfileCalendarViewModel() { color = e.Color, title = e.Title, start = e.Start })
					.ToList();

			return Json(new
			{
				Labs = labsEvents,
				Lect = lectEvents
			});
		}
	}
}