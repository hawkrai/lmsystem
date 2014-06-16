using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using Application.Core.Data;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.LecturerManagement;
using Application.Infrastructure.ProjectManagement;
using Application.Infrastructure.StudentManagement;
using Application.Infrastructure.SubjectManagement;
using Application.Infrastructure.UserManagement;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;
using WebMatrix.WebData;

namespace LMPlatform.UI.ViewModels.BTSViewModels
{
    public class ProjectParticipationViewModel
    {
        private static LmPlatformModelsContext context = new LmPlatformModelsContext();

        [DisplayName("Группа")]
        public string GroupId { get; set; }

        public ProjectParticipationViewModel()
        {   
        }

        public ProjectParticipationViewModel(int groupId)
        {
            var group = new GroupManagementService().GetGroup(groupId);
            GroupId = group.Name;
            StudentGroupUserList = GetStudentGroupUserList(GroupId);
        }

        public IList<SelectListItem> GetGroups()
        {
            var groups = GetAssignedGroups(WebSecurity.CurrentUserId);

            return groups.Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString(CultureInfo.InvariantCulture)
            }).ToList();
        }

        public IList<Group> GetAssignedGroups(int userId)
        {
            var groups = new LmPlatformRepositoriesContainer().ProjectsRepository.GetGroups(userId);

            return groups;
        }

        public List<StudentGroupUser> LecturerList { get; set; }

        public List<StudentGroupUser> GetLecturerList()
        {
            LecturerList = new List<StudentGroupUser>();

            var lecturers = new LecturerManagementService().GetLecturers().ToList();
            var number = 1;

            foreach (var lecturer in lecturers)
            {
                LecturerList.Add(new StudentGroupUser
                {
                    Number = number,
                    Name = lecturer.LastName + " " + lecturer.FirstName + " " + lecturer.MiddleName,
                    ProjectName = GetProjectNameList(lecturer.Id),
                    ProjectRole = GetProjectRoleList(lecturer.Id),
                    ProjectCreatorName = GetProjectCreatorNameList(lecturer.Id),
                    QuentityOfProjects = GetProjectNameList(lecturer.Id).Count()
                });
                number++;
            }

            //LecturerList.Sort();
            return LecturerList;
        }

        public List<StudentGroupUser> StudentGroupUserList { get; set; }

        public List<StudentGroupUser> GetStudentGroupUserList(string groupName)
        {
            StudentGroupUserList = new List<StudentGroupUser>();

            var groupId = new GroupManagementService().GetGroupByName(groupName).Id;

            var students = new StudentManagementService().GetGroupStudents(groupId).ToList();
            var number = 1;

            foreach (var student in students)
            {
                StudentGroupUserList.Add(new StudentGroupUser
                {
                    Number = number,
                    Name = student.LastName + " " + student.FirstName + " " + student.MiddleName,
                    ProjectName = GetProjectNameList(student.Id),
                    ProjectRole = GetProjectRoleList(student.Id),
                    ProjectCreatorName = GetProjectCreatorNameList(student.Id),
                    QuentityOfProjects = GetProjectNameList(student.Id).Count()
                });
                number++;
            }

            return StudentGroupUserList;
        }

        public List<string> GetProjectNameList(int studentId)
        {
            var projectUserList = new ProjectManagementService().GetProjectsOfUser(studentId).ToList();
            var projectNameList = new List<string>();
            foreach (var project in projectUserList)
            {
                projectNameList.Add(project.Project.Title);
            }

            return projectNameList;
        }

        public List<string> GetProjectCreatorNameList(int studentId)
        {
            var context = new ProjectManagementService();
            var projectUserList = context.GetProjectsOfUser(studentId).ToList();
            var projectCreatorNameList = new List<string>();
            foreach (var project in projectUserList)
            {
                var creator =
                    new LmPlatformRepositoriesContainer().UsersRepository.GetBy(
                        new Query<User>(e => e.Id == project.Project.CreatorId));
                projectCreatorNameList.Add(context.GetCreatorName(creator.Id));
            }

            return projectCreatorNameList;
        }

        public List<string> GetProjectRoleList(int studentId)
        {
            var projectUserList = new ProjectManagementService().GetProjectsOfUser(studentId).ToList();
            var projectRoleList = new List<string>();
            foreach (var project in projectUserList)
            {
                projectRoleList.Add(project.ProjectRole.Name);
            }

            return projectRoleList;
        }
    }
}