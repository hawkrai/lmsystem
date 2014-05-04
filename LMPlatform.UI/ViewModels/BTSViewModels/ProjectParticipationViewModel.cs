using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Application.Core.Data;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.ProjectManagement;
using Application.Infrastructure.StudentManagement;
using Application.Infrastructure.UserManagement;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;

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
            var groups = new GroupManagementService().GetGroups();

            return groups.Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString(CultureInfo.InvariantCulture)
            }).ToList();
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
                    Name = student.FirstName + " " + student.MiddleName + " " + student.LastName,
                    ProjectName = GetProjectNameList(student.Id),
                    ProjectCreatorName = GetProjectCreatorNameList(student.Id),
                    ProjectRole = GetProjectRoleList(student.Id)
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
            var projectUserList = new ProjectManagementService().GetProjectsOfUser(studentId).ToList();
            var projectCreatorNameList = new List<string>();
            foreach (var project in projectUserList)
            {
                var creator =
                    new LmPlatformRepositoriesContainer().UsersRepository.GetBy(
                        new Query<User>(e => e.Id == project.Project.CreatorId));
                projectCreatorNameList.Add(creator.FullName);
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