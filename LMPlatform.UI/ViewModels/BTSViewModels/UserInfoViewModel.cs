using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Application.Infrastructure.LecturerManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    public class UserInfoViewModel
    {
        [DisplayName("ФИО")]
        public string UserName { get; set; }

        [DisplayName("Роль")]
        public string Role { get; set; }

        [DisplayName("Номер группы")]
        public string GroupNumber { get; set; }

        [DisplayName("Привязанные предметы:")]
        public List<Subject> SubjectList { get; set; }

        [DisplayName("Количество проектов, на которых занят:")]
        public int ProjectQuentity { get; set; }

        public UserInfoViewModel(int id)
        {
            var context = new LmPlatformModelsContext();
            if (context.Students.Find(id) != null)
            {
                //var creator = context.Students.Find(id);
                var creator = new Student();
                foreach (var student in context.Students)
                {
                    if (student.Id == id)
                    {
                        creator = student;
                    }
                }

                UserName = creator.LastName + " " + creator.FirstName + " " + creator.MiddleName;
                GroupNumber = context.Groups.Find(creator.GroupId).Name;
                ProjectQuentity = context.ProjectUsers.Select(e => e.User).Count(e => e.Id == creator.Id);
                Role = "Студент";
            }
            else
            {
                var creator = context.Lecturers.Find(id);
                UserName = creator.LastName + " " + creator.FirstName + " " + creator.MiddleName;
                ProjectQuentity = context.ProjectUsers.Select(e => e.User).Count(e => e.Id == creator.Id);
                Role = "Преподаватель";

                var _context = new SubjectManagementService();
                SubjectList = new List<Subject>();
                foreach (var subject in _context.GetUserSubjects(creator.Id))
                {
                    SubjectList.Add(subject);
                }
            }
        }
    }
}