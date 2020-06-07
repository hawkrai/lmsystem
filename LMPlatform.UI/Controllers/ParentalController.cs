﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Core.Data;
using Application.Core.UI.HtmlHelpers;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;
using LMPlatform.UI.ViewModels.ParentalViewModels;
using Mvc.JQuery.Datatables;

namespace LMPlatform.UI.Controllers
{
    using Application.Core.UI.Controllers;
    using Application.Infrastructure.GroupManagement;
    using Application.Infrastructure.StudentManagement;
    using Application.Infrastructure.UserManagement;

    [AllowAnonymous]
    public class ParentalController : BasicController
    {
        [AllowAnonymous]
        public ActionResult Index(string id, string FIO)
        {
            if (id == null || id == "")
            {
                if (FIO == null || FIO == "")
                {
                    return RedirectToAction("GroupNotFound");
                }
                else
                {
                    Student student = StudentManagementService.GetStudents().FirstOrDefault(s => s.FullName.Equals(FIO));
                    if (student == null)
                    {
                        return RedirectToAction("GroupNotFound");
                    }
                    else
                    {
                        var model = new ParentalViewModel()
                        {
                            Group = GroupManagementService.GetGroup(student.GroupId)
                        };
                        return View(model);
                    }
                }
            }
            else
            {
                var group = GroupManagementService.GetGroupByName(id);

                if (group != null)
                {
                    var model = new ParentalViewModel()
                    {
                        Group = group
                    };
                    return View(model);
                }
                else
                {
                    if (FIO == null || FIO == "")
                    {
                        Student student = StudentManagementService.GetStudents().FirstOrDefault(s => s.FullName.Equals(FIO));
                        if (student == null)
                        {
                            return RedirectToAction("GroupNotFound");
                        }
                        else
                        {
                            var model = new ParentalViewModel()
                            {
                                Group = GroupManagementService.GetGroup(student.GroupId)
                            };
                            return View(model);
                        }
                    }
                    else
                        return RedirectToAction("GroupNotFound");
                }
            }
        }

        public ActionResult Front()
        {
            return this.PartialView("Parental/_Front");
        }

        public ActionResult Statistics()
        {
            return this.PartialView("Parental/_Statistics");
        }

        public ActionResult Plan()
        {
            return this.PartialView("Parental/_Plan");
        }

        public ActionResult GetSideNav(int groupId)
        {
            var group = GroupManagementService.GetGroup(groupId);
            var subjects = SubjectManagementService.GetGroupSubjects(groupId);

            var model = new ParentalViewModel(group)
            {
                Subjects = subjects
            };
            return PartialView("_ParentalSideNavPartial", model);
        }

        public List<Subject> GetSubjects(int groupId)
        {
            return SubjectManagementService.GetGroupSubjects(groupId);
        }

        public ActionResult GroupNotFound()
        {
            return View();
        }

        public bool IsValidGroup(string groupName)
        {
            return GroupManagementService.GetGroupByName(groupName) != null;
        }

        public IGroupManagementService GroupManagementService
        {
            get
            {
                return ApplicationService<IGroupManagementService>();
            }
        }

        public IStudentManagementService StudentManagementService
        {
            get
            {
                return ApplicationService<IStudentManagementService>();
            }
        }


        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return ApplicationService<ISubjectManagementService>();
            }
        }
    }
}
