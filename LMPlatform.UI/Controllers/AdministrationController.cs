using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using Application.Core.UI.Controllers;
using Application.Core.UI.HtmlHelpers;
using Application.Infrastructure.DPManagement;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.LecturerManagement;
using Application.Infrastructure.StudentManagement;
using Application.Infrastructure.SubjectManagement;
using Application.Infrastructure.UserManagement;
using LMPlatform.UI.ViewModels.AccountViewModels;
using LMPlatform.UI.ViewModels.AdministrationViewModels;
using Mvc.JQuery.Datatables;

namespace LMPlatform.UI.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdministrationController : BasicController
    {
        public ActionResult Index()
        {
            var activityModel = new UserActivityViewModel();
            return View(activityModel);
        }

        public ActionResult Students()
        {
            return View();
        }

        public ActionResult EditStudent(int id)
        {
            var student = StudentManagementService.GetStudent(id);

            if (student != null)
            {
                var model = new ModifyStudentViewModel(student);
                return PartialView("_EditStudentView", model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditStudent(ModifyStudentViewModel model, int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.ModifyStudent();
                    ViewBag.ResultSuccess = true;
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, string.Empty);
                }
            }

            return null;
        }

        [HttpPost]
        public ActionResult EditStudentAjax(ModifyStudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = UsersManagementService.GetUserByName(model.Name, model.Surname, model.Patronymic);
                    if (user == null || user.Id == model.Id)
                    {
                        model.ModifyStudent();
                        return Json(new { resultMessage = "Студент сохранен" });
                    }

                    ModelState.AddModelError(string.Empty, "Пользователь с таким именем уже существует");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, string.Empty);
                }
            }

            return PartialView("_EditStudentView", model);
        }

        public ActionResult AddProfessor()
        {
            var model = new RegisterViewModel();
            return PartialView("_AddProfessorView", model);
        }

        [HttpPost]
        public ActionResult AddProfessorAjax(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = UsersManagementService.GetUserByName(model.Name, model.Surname, model.Patronymic);
                    if (user == null)
                    {
                        model.RegistrationUser(new[] { "lector" });
                        return Json(new { resultMessage = "Преподаватель сохранен" });
                    }

                    ModelState.AddModelError(string.Empty, "Пользователь с таким именем уже существует");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError(string.Empty, e.StatusCode.ToString());
                }
            }

            return PartialView("_AddProfessorView", model);
        }

        [HttpPost]
        public ActionResult AddProfessor(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.RegistrationUser(new[] { "lector" });
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError(string.Empty, e.StatusCode.ToString());
                    return View(model);
                }
            }

            return null;
        }

        public ActionResult EditProfessor(int id)
        {
            var lecturer = LecturerManagementService.GetLecturer(id);

            if (lecturer != null)
            {
                var model = new ModifyLecturerViewModel(lecturer);
                return PartialView("_EditProfessorView", model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditProfessor(ModifyLecturerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.ModifyLecturer();
                    ViewBag.ResultSuccess = true;
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, string.Empty);
                }
            }

            return null;
        }

        [HttpPost]
        public ActionResult EditProfessorAjax(ModifyLecturerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = UsersManagementService.GetUserByName(model.Name, model.Surname, model.Patronymic);
                    if (user == null || user.Id == model.LecturerId)
                    {
                        model.ModifyLecturer();
                        return this.Json(new { resultMessage = "Преподаватель сохранен" });
                    }

                    ModelState.AddModelError(string.Empty, "Пользователь с таким именем уже существует");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, string.Empty);
                }
            }

            return PartialView("_EditProfessorView", model);
        }

        public ActionResult AddGroup()
        {
            var model = new GroupViewModel();
            return PartialView("_AddGroupView", model);
        }

        [HttpPost]
        public ActionResult AddGroup(GroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!model.CheckGroupName())
                    {
                        ModelState.AddModelError(string.Empty, "Группа с таким номером уже существует");
                    }
                    else
                    {
                        model.AddGroup();
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError(string.Empty, e.StatusCode.ToString());
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddGroupAjax(GroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!model.CheckGroupName())
                    {
                        ModelState.AddModelError(string.Empty, "Группа с таким номером уже существует");
                    }
                    else
                    {
                        model.AddGroup();
                        return Json(new { resultMessage = "Группа сохранена" });
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError(string.Empty, e.StatusCode.ToString());
                }
            }

            return PartialView("_AddGroupView", model);
        }

        public ActionResult EditGroup(int id)
        {
            var group = GroupManagementService.GetGroup(id);

            if (group != null)
            {
                var model = new GroupViewModel(group);
                return PartialView("_EditGroupView", model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EditGroup(GroupViewModel model, int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.ModifyGroup();
                    ViewBag.ResultSuccess = true;
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, string.Empty);
                }
            }

            return null;
        }

        [HttpPost]
        public ActionResult EditGroupAjax(GroupViewModel model)
        {
            if (!model.CheckGroupName())
            {
                this.ModelState.AddModelError(string.Empty, "Группа с таким именем уже существует");
            }

            if (ModelState.IsValid && model.CheckGroupName())
            {
                try
                {
                    if (!model.CheckGroupName())
                    {
                        this.ModelState.AddModelError(string.Empty, "Группа с таким именем уже существует");
                    }
                    else
                    {
                        model.ModifyGroup();
                        return Json(new { resultMessage = "Группа сохранена" });
                    }
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Произошла ошибка");
                }
            }

            return PartialView("_EditGroupView", model);
        }

        public ActionResult Professors()
        {
            return View();
        }

        public ActionResult Groups()
        {
            return View();
        }

     public ActionResult ListOfStudents(int id)
        {
            var students = StudentManagementService.GetGroupStudents(id).OrderBy(student => student.FullName).ToList();

            if (students != null)
            {
                //var model =  StudentViewModel.FromStudent();
                return PartialView("_ListOfStudents", students);
            }

            return RedirectToAction("Index");
        }

     public ActionResult ListOfGroups(int id)
     {
         var sub = SubjectManagementService.GetSubjectsByLector(id).OrderBy(subject => subject.Name).ToList();
         var lec = LecturerManagementService.GetLecturer(id);
         if (sub != null)
         {

             var model = ListSubjectViewModel.FormSubjects(sub, lec.FullName);
             return PartialView("_ListOfGroups", model);
         }

         return RedirectToAction("Index");
     }

        public ActionResult ListOfSubject(int id)
        {
            var groups = SubjectManagementService.GetSubjectsByStudent(id).OrderBy(subject => subject.Name).ToList();
            var stud = StudentManagementService.GetStudent(id);

            if (groups != null)
            {
                var model = ListSubjectViewModel.FormSubjects(groups, stud.FullName);
                return PartialView("ListOfSubject", model);
            }

            return RedirectToAction("Index");
        }


        public ActionResult Profile(int id)
        {
            var login = UsersManagementService.GetUser(id);

            if (login != null)
            {

                //var model =  StudentViewModel.FromStudent();
                return Redirect(string.Format("/Profile/Page/{0}", login.UserName));
            }

            return RedirectToAction("Index");
        }

        public ActionResult Files()
        {
            return View();
        }

        public ActionResult ResetPassword(int id)
        {
            try
            {
                var user = UsersManagementService.GetUser(id);


                var resetPassModel = new ResetPasswordViewModel(user);
                if (Request != null && Request.UrlReferrer != null)
                {
                    ViewBag.ReturnUrl = Request.UrlReferrer;
                }
                else
                {
                    ViewBag.ReturnUrl = "/Administration/Index";
                }

                return View(resetPassModel);
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resetResult = model.ResetPassword();
                ViewBag.ResultSuccess = resetResult;

                if (!resetResult)
                {
                    ModelState.AddModelError(string.Empty, "Пароль не был сброшен");
                }
            }

            return View(model);
        }

        [HttpPost]
        public bool DeleteUser(int id)
        {
            try
            {
                UsersManagementService.DeleteUser(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public JsonResult DeleteStudent(int id)
        {
            try
            {
                var student = StudentManagementService.GetStudent(id);
                if (student != null)
                {
                    var result = StudentManagementService.DeleteStudent(id);
                    if (result)
                    {
                        return Json(new { resultMessage = string.Format("Студент {0} удален", student.FullName) });
                    }

                    return Json(new { resultMessage = string.Format("Не удалось удалить студента {0}", student.FullName) });
                }

				return Json(new { resultMessage = "Удаление невозможно. Студента не существует", status = "500" });
            }
            catch(Exception ex)
            {
				return Json(new { resultMessage = ex.Message, status = "500" });
            }
        }

        [HttpPost]
        public JsonResult DeleteLecturer(int id)
        {
            try
            {
                var lecturer = LecturerManagementService.GetLecturer(id);

                if (lecturer != null)
                {
					if (lecturer.SubjectLecturers != null && lecturer.SubjectLecturers.Any() && lecturer.SubjectLecturers.All(e => e.Subject.IsArchive))
					{
						foreach (var lecturerSubjectLecturer in lecturer.SubjectLecturers)
						{
							LecturerManagementService.DisjoinOwnerLector(lecturerSubjectLecturer.SubjectId, id);	
						}
					}
					//else if (lecturer.SubjectLecturers != null && lecturer.SubjectLecturers.Any() && lecturer.SubjectLecturers.Any(e => !e.Subject.IsArchive))
					//{
					//	return Json(new { resultMessage = "Удаление невозможно. Преподаватель связан с предметами", status = "500" });
					//}

                    var result = LecturerManagementService.DeleteLecturer(id);

                    if (result)
                    {
                        return Json(new { resultMessage = string.Format("Преподаватель {0} удален", lecturer.FullName) });
                    }

					return Json(new { resultMessage = string.Format("Не удалось удалить преподавателя {0}", lecturer.FullName), status = "500" });
                }

				return Json(new { resultMessage = "Удаление невозможно. Преподавателя не существует", status = "500" });
            }
            catch
            {
				return Json(new { resultMessage = "Произошла ошибка при удалении", status = "500" });
            }
        }

        [HttpPost]
        public JsonResult DeleteGroup(int id)
        {
            try
            {
                var group = GroupManagementService.GetGroup(id);
                if (group != null)
                {
                    if (group.Students != null && group.Students.Count > 0)
                    {
                        return Json(new { resultMessage = "Группа содержит студентов и не может быть удалена" });
                    }

                    GroupManagementService.DeleteGroup(id);
                    return Json(new { resultMessage = string.Format("Группа {0} удалена", group.Name) });
                }

				return Json(new { resultMessage = "Группы не существует", status = "500" });
            }
			catch (Exception ex)
            {
				return Json(new { resultMessage = ex.Message, status = "500" });
            }
        }

        public JsonResult Attendance(int id)
        {
            var user = UsersManagementService.GetUser(id);

            if (user != null && user.Attendance != null)
            {
                var data = user.AttendanceList.GroupBy(e => e.Date).Select(d => new { day = d.Key.ToString("d"), count = d.Count() });
                return Json(new { resultMessage = user.FullName, attendance = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { resultMessage = "Нет данных", data = "[]" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public DataTablesResult<StudentViewModel> GetCollectionStudents(DataTablesParam dataTableParam)
        {
            var searchString = dataTableParam.GetSearchString();
	        ViewBag.Profile = "/Administration/Profile";
            ViewBag.ListOfSubject = "/Administration/ListOfSubject";
            ViewBag.EditActionLink = "/Administration/EditStudent";
            ViewBag.DeleteActionLink = "/Administration/DeleteStudent";
            ViewBag.StatActionLink = "/Administration/Attendance";
            var students = StudentManagementService.GetStudentsPageable(pageInfo: dataTableParam.ToPageInfo(), searchString: searchString);
            this.SetupSettings(dataTableParam);
            return DataTableExtensions.GetResults(students.Items.Select(s => StudentViewModel.FromStudent(s, PartialViewToString("_EditGlyphLinks", s.Id))), dataTableParam, students.TotalCount);
        }

        [HttpPost]
        public DataTablesResult<LecturerViewModel> GetCollectionLecturers(DataTablesParam dataTableParam)
        {
            var searchString = dataTableParam.GetSearchString();
            ViewBag.Profile = "/Administration/Profile";
            ViewBag.ListOfSubject = "/Administration/ListOfGroups";
            ViewBag.EditActionLink = "/Administration/EditProfessor";
            ViewBag.DeleteActionLink = "/Administration/DeleteLecturer";
            ViewBag.StatActionLink = "/Administration/Attendance";
            var lecturers = LecturerManagementService.GetLecturersPageable(pageInfo: dataTableParam.ToPageInfo(), searchString: searchString);
			this.SetupSettings(dataTableParam);
            return DataTableExtensions.GetResults(lecturers.Items.Select(l => LecturerViewModel.FormLecturers(l, PartialViewToString("_EditGlyphLinks", l.Id, l.IsActive))), dataTableParam, lecturers.TotalCount);
        }

        [HttpPost]
        public DataTablesResult<GroupViewModel> GetCollectionGroups(DataTablesParam dataTableParam)
        {
            var searchString = dataTableParam.GetSearchString();
            ViewBag.ListOfStudents = "/Administration/ListOfStudents";
            ViewBag.EditActionLink = "/Administration/EditGroup";
            ViewBag.DeleteActionLink = "/Administration/DeleteGroup";
            var groups = GroupManagementService.GetGroupsPageable(pageInfo: dataTableParam.ToPageInfo(), searchString: searchString);
            this.SetupSettings(dataTableParam);
            return DataTableExtensions.GetResults(groups.Items.Select(g => GroupViewModel.FormGroup(g, PartialViewToString("_EditGlyphLinks", g.Id))), dataTableParam, groups.TotalCount);
        }

	    private void SetupSettings(DataTablesParam dataTableParam)
	    {
			var n = 20;

		    for (var i = 0; i < n; i++)
		    {
			    if (string.IsNullOrEmpty(this.HttpContext.Request.Form["iSortCol_" + i]))
			    {
				    return;
			    }

				dataTableParam.iSortCol.Add(int.Parse(this.HttpContext.Request.Form["iSortCol_" + i]));
				dataTableParam.sSortDir.Add(this.HttpContext.Request.Form["sSortDir_" + i]);
		    }
		    
	    }

		#region Json actions

		[HttpGet]
		public ActionResult UserActivityJson()
		{
			var activityModel = new UserActivityViewModel();

			var responseObj = new
			{
                activityModel.ServiceAccountsCount,
                activityModel.TotalLecturersCount,
                activityModel.TotalStudentsCount,
                activityModel.TotalUsersCount,
                activityModel.UserActivityJson,
			};
			
			return JsonResponse(responseObj);
		}

		[HttpGet]
		public ActionResult StudentsJson()
		{
			var students = StudentManagementService.GetStudents();
			
			var result = students.Select(s => new ModifyStudentViewModel(s){Avatar = null});

			return JsonResponse(result);
		}

		[HttpGet]
		public ActionResult GetStudentJson(int id)
		{
			var student = StudentManagementService.GetStudent(id);

			if (student != null)
			{
				var model = new ModifyStudentViewModel(student);

				return JsonResponse(model);
			}

			return StatusCode(HttpStatusCode.BadRequest);
		}

		[HttpPost]
		public ActionResult SaveStudentJson(ModifyStudentViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var user = UsersManagementService.GetUser(model.Id);

					if (user != null)
					{
						model.ModifyStudent();
						return StatusCode(HttpStatusCode.OK);
					}
				}
				catch
				{
					return StatusCode(HttpStatusCode.InternalServerError);
				}
			}

			return StatusCode(HttpStatusCode.BadRequest);
		}

		[HttpPost]
		public ActionResult AddProfessorJson(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var user = UsersManagementService.GetUserByName(model.Name, model.Surname, model.Patronymic);

					if (user == null)
					{
						model.RegistrationUser(new[] { "lector" });
						return StatusCode(HttpStatusCode.OK);
					}
				}
				catch
				{
					return StatusCode(HttpStatusCode.InternalServerError);
				}
			}

			return StatusCode(HttpStatusCode.BadRequest);
		}

		[HttpGet]
		public ActionResult GetProfessorJson(int id)
		{
			var lecturer = LecturerManagementService.GetLecturer(id);

			if (lecturer != null)
			{
				var model = LecturerViewModel.FormLecturers(lecturer, null);
				return JsonResponse(model);
			}

			return StatusCode(HttpStatusCode.BadRequest);
		}

		[HttpPost]
		public ActionResult SaveProfessorJson(ModifyLecturerViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					model.ModifyLecturer();

					return StatusCode(HttpStatusCode.OK);
				}
				catch
				{
					return StatusCode(HttpStatusCode.InternalServerError);
				}
			}

			return StatusCode(HttpStatusCode.BadRequest);
		}

		[HttpPost]
		public ActionResult AddGroupJson(GroupViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (!model.CheckGroupName())
					{
						return StatusCode(HttpStatusCode.BadRequest, "Группа с таким именем уже существует.");
					}
					else
					{
						model.AddGroup();
						return StatusCode(HttpStatusCode.OK);
					}
				}
				catch (MembershipCreateUserException e)
				{
					return StatusCode(HttpStatusCode.InternalServerError, e.Message);
				}
			}

			return StatusCode(HttpStatusCode.BadRequest, "Невалидная модель.");
		}

		[HttpGet]
		public ActionResult GetGroupJson(int id)
		{
			var group = GroupManagementService.GetGroup(id);

			if (group != null)
			{
				var model = new GroupViewModel(group);
				return JsonResponse(model);
			}

			return StatusCode(HttpStatusCode.BadRequest);
		}

		[HttpPost]
		public ActionResult SaveGroupJson(GroupViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					model.ModifyGroup();
					return StatusCode(HttpStatusCode.OK);
				}
				catch (Exception ex)
				{
					return StatusCode(HttpStatusCode.InternalServerError, ex.Message);
				}
			}

			return StatusCode(HttpStatusCode.BadRequest, "Невалидная модель.");
		}

		[HttpGet]
		public ActionResult GetProfessorsJson()
		{
			var lecturers = LecturerManagementService.GetLecturers();

			var responseModel = lecturers.Select(l => LecturerViewModel.FormLecturers(l, null));

			return JsonResponse(responseModel);
		}

		[HttpGet]
		public ActionResult GetGroupsJson()
		{
			var groups = GroupManagementService.GetGroups();

			var responseModel = groups.Select(l => GroupViewModel.FormGroup(l, null));

			return JsonResponse(responseModel);
		}

		[HttpGet]
		public ActionResult GetSubjectsJson(int id)
		{
			var subjects = SubjectManagementService.GetSubjectsByStudent(id).OrderBy(subject => subject.Name).ToList();
			var student = StudentManagementService.GetStudent(id);

			if (subjects.Count > 0)
			{
				var model = ListSubjectViewModel.FormSubjects(subjects, student.FullName);
				return JsonResponse(model);
			}

			return StatusCode(HttpStatusCode.BadRequest);
		}

		[HttpGet]
		public ActionResult GetResetPasswordModelJson(int id)
		{
			try
			{
				var user = UsersManagementService.GetUser(id);

				var resetPassModel = new ResetPasswordViewModel(user);

				return JsonResponse(resetPassModel);
			}
			catch (Exception ex)
			{
				return StatusCode(HttpStatusCode.InternalServerError, ex.Message);
			}
		}

		[HttpPost]
		public ActionResult ResetPasswordJson(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var resetResult = model.ResetPassword();

				if (!resetResult)
				{
					return StatusCode(HttpStatusCode.Conflict, "Password hasn't been reseted.");
				}

				return StatusCode(HttpStatusCode.OK);
			}

			return StatusCode(HttpStatusCode.BadRequest, "Invalid model.");
		}

		[HttpDelete]
		public ActionResult DeleteUserJson(int id)
		{
			try
			{
				var deleted = UsersManagementService.DeleteUser(id);

				if (deleted)
				{
					return StatusCode(HttpStatusCode.OK);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(HttpStatusCode.InternalServerError, ex.Message);
			}

			return StatusCode(HttpStatusCode.BadRequest);
		}

		[HttpGet]
		public ActionResult AttendanceJson(int id)
		{
			var user = UsersManagementService.GetUser(id);

			if (user?.Attendance != null)
			{
				var data = user.AttendanceList.GroupBy(e => e.Date).Select(d => new { day = d.Key.ToString("d"), count = d.Count() });

				return JsonResponse(new { resultMessage = user.FullName, attendance = data });
			}

			return StatusCode(HttpStatusCode.BadRequest);
		}

		[HttpDelete]
		public ActionResult DeleteStudentJson(int id)
		{
			try
			{
				var student = StudentManagementService.GetStudent(id);

				if (student != null)
				{
					var result = StudentManagementService.DeleteStudent(id);

					if (result)
					{
						return StatusCode(HttpStatusCode.OK);
					}

					return StatusCode(HttpStatusCode.Conflict);
				}

				return StatusCode(HttpStatusCode.BadRequest);
			}
			catch (Exception ex)
			{
				return StatusCode(HttpStatusCode.InternalServerError, ex.Message);
			}
		}

		[HttpDelete]
		public ActionResult DeleteLecturerJson(int id)
		{
			try
			{
				var lecturer = LecturerManagementService.GetLecturer(id);

				if (lecturer != null)
				{
					if (lecturer.SubjectLecturers != null && lecturer.SubjectLecturers.Any() && lecturer.SubjectLecturers.All(e => e.Subject.IsArchive))
					{
						foreach (var lecturerSubjectLecturer in lecturer.SubjectLecturers)
						{
							LecturerManagementService.DisjoinOwnerLector(lecturerSubjectLecturer.SubjectId, id);
						}
					}

					var result = LecturerManagementService.DeleteLecturer(id);

					if (result)
					{
						return StatusCode(HttpStatusCode.OK);
					}

					return StatusCode(HttpStatusCode.Conflict);
				}

				return StatusCode(HttpStatusCode.BadRequest);
			}
			catch (Exception ex)
			{
				return StatusCode(HttpStatusCode.InternalServerError, ex.Message);
			}
		}

		[HttpDelete]
		public ActionResult DeleteGroupJson(int id)
		{
			try
			{
				var group = GroupManagementService.GetGroup(id);
				if (group != null)
				{
					if (group.Students != null && group.Students.Count > 0)
					{
						return StatusCode(HttpStatusCode.Conflict);
					}

					GroupManagementService.DeleteGroup(id);
					return StatusCode(HttpStatusCode.OK);
				}

				return StatusCode(HttpStatusCode.BadRequest);
			}
			catch (Exception ex)
			{
				return StatusCode(HttpStatusCode.InternalServerError, ex.Message);
			}
		}

		[HttpGet]
		public ActionResult ListOfGroupsByLecturerJson(int id)
		{
			var sub = SubjectManagementService.GetSubjectsByLector(id).OrderBy(subject => subject.Name).ToList();
			var lec = LecturerManagementService.GetLecturer(id);
			if (sub != null && lec != null)
			{
				var model = ListSubjectViewModel.FormSubjects(sub, lec.FullName);
				var response = new
				{
					Lecturer = model.Name,
					Subjects = model.Subjects.Distinct().Select(s => new
					{
						SubjectName = s.Name,
						Groups = s.SubjectGroups.Where(sg => sg.Group != null).Select(sg => new
						{
							GroupName = sg.Group.Name,
							StudentsCount = sg.Group.Students.Count
						})
					})
				};
				return JsonResponse(response);
			}

			return StatusCode(HttpStatusCode.BadRequest);
		}

		[HttpGet]
		public ActionResult ListOfSubjectsByStudentJson(int id)
		{
			var groups = SubjectManagementService.GetSubjectsByStudent(id).OrderBy(subject => subject.Name).ToList();
			var stud = StudentManagementService.GetStudent(id);

			if (groups != null && stud != null)
			{
				var model = ListSubjectViewModel.FormSubjects(groups, stud.FullName);
				var response = new
				{
					Student = model.Name,
					Subjects = model.Subjects.Select(s => new
					{
						SubjectName = s.Name,
						Lecturers = s.SubjectLecturers?.Select(prof => prof.Lecturer.FullName)
					})
				};
				return JsonResponse(response);
			}

			return StatusCode(HttpStatusCode.BadRequest);
		}

		[HttpGet]
		public ActionResult ListOfStudentsByGroupJson(int id)
		{
			try
			{
				var students = StudentManagementService.GetGroupStudents(id)
					.OrderBy(student => student.FullName)
					.ToList();

				var response = new
				{
					Group = students.Count > 0 ? students[0].Group.Name : null,
					Students = students.Select(s => new
					{
						Name = s.FullName,
						IsConfirmed = s.Confirmed ?? false
					})
				};
				return JsonResponse(response);
			}
			catch(Exception ex)
			{
				return StatusCode(HttpStatusCode.InternalServerError, ex.Message);
			}
		}

		[HttpPost]
		public ActionResult EditStudentJson(ModifyStudentViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					model.ModifyStudent();
					return StatusCode(HttpStatusCode.Created, "Студент сохранен");
				}
				catch (Exception ex)
				{
					return StatusCode(HttpStatusCode.InternalServerError, ex.Message);
				}
			}
			return StatusCode(HttpStatusCode.BadRequest);
		}

		#endregion

		#region Dependencies

		public IStudentManagementService StudentManagementService => ApplicationService<IStudentManagementService>();

		public ISubjectManagementService SubjectManagementService => ApplicationService<ISubjectManagementService>();

		public IGroupManagementService GroupManagementService => ApplicationService<IGroupManagementService>();

		public ILecturerManagementService LecturerManagementService => ApplicationService<ILecturerManagementService>();

		public IUsersManagementService UsersManagementService => ApplicationService<IUsersManagementService>();

		public IDpManagementService DpManagementService => ApplicationService<IDpManagementService>();

		#endregion
    }
}
