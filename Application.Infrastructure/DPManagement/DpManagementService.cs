using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Application.Core;
using Application.Core.Data;
using Application.Core.Extensions;
using Application.Infrastructure.DTO;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Models.DP;

namespace Application.Infrastructure.DPManagement
{
    public class DpManagementService : IDpManagementService
    {
        private readonly LazyDependency<IDpContext> context = new LazyDependency<IDpContext>();

        private IDpContext Context
        {
            get { return context.Value; }
        }

        public PagedList<DiplomProjectData> GetProjects(GetPagedListParams parms)
        {
            var query = Context.DiplomProjects.AsNoTracking()
                .Include(x => x.Lecturer)
                .Include(x => x.AssignedDiplomProjects.Select(asp => asp.Student.Group));

            return (from dp in query
                    let adp = dp.AssignedDiplomProjects.FirstOrDefault()
                    select new DiplomProjectData
                    {
                        Id = dp.DiplomProjectId,
                        Theme = dp.Theme,
                        Lecturer = dp.Lecturer != null ? dp.Lecturer.LastName + " " + dp.Lecturer.FirstName + " " + dp.Lecturer.MiddleName : null,
                        Student = adp != null && adp.Student != null ? adp.Student.LastName + " " + adp.Student.FirstName + " " + adp.Student.MiddleName : null,
                        Group = adp != null && adp.Student != null && adp.Student.Group != null ? adp.Student.Group.Name : null,
                        ApproveDate = adp != null ? adp.ApproveDate : null
                    }).ApplyPaging(parms);
        }

        public DiplomProjectData GetProject(int id)
        {
            var dp = Context.DiplomProjects
                .AsNoTracking()
                .Include(x => x.DiplomProjectGroups)
                .Single(x => x.DiplomProjectId == id);
            return new DiplomProjectData
                {
                    Id = dp.DiplomProjectId,
                    Theme = dp.Theme,
                    SelectedGroupsIds = dp.DiplomProjectGroups.Select(x => x.GroupId)
                };
        }

        public void SaveProject(DiplomProjectData projectData)
        {
            if (!projectData.LecturerId.HasValue)
            {
                throw new ApplicationException("LecturerId cant be empty!");
            }

            DiplomProject project;
            if (projectData.Id.HasValue)
            {
                project = Context.DiplomProjects
                              .Include(x => x.DiplomProjectGroups)
                              .Single(x => x.DiplomProjectId == projectData.Id);
            }
            else
            {
                project = new DiplomProject();
                Context.DiplomProjects.Add(project);
            }

            var currentGroups = project.DiplomProjectGroups.ToList();
            var newGroups = projectData.SelectedGroupsIds.Select(x => new DiplomProjectGroup { GroupId = x, DiplomProjectId = project.DiplomProjectId }).ToList();

            var groupsToAdd = newGroups.Except(currentGroups, grp => grp.GroupId);
            var groupsToDelete = currentGroups.Except(newGroups, grp => grp.GroupId);

            foreach (var projectGroup in groupsToAdd)
            {
                project.DiplomProjectGroups.Add(projectGroup);
            }

            foreach (var projectGroup in groupsToDelete)
            {
                Context.DiplomProjectGroups.Remove(projectGroup);
            }

            project.LecturerId = projectData.LecturerId.Value;
            project.Theme = projectData.Theme;
            Context.SaveChanges();
        }

        public void DeleteProject(int userId, int id)
        {
            ValidateLecturerAccess(userId);

            var project = Context.DiplomProjects.Single(x => x.DiplomProjectId == id);
            Context.DiplomProjects.Remove(project);
            Context.SaveChanges();
        }

        public void AssignProject(int userId, int projectId, int studentId)
        {
            var isLecturer = IsLecturer(userId);
            if (!isLecturer && userId != studentId)
            {
                throw new Exception("A student can only to assign a project to himself!");
            }

            var assignment = Context.AssignedDiplomProjects.FirstOrDefault(x => x.DiplomProjectId == projectId);

            if (assignment != null && assignment.ApproveDate.HasValue)
            {
                throw new ApplicationException("The selected Diplom Project has already been assigned!");
            }

            if (assignment == null)
            {
                assignment = new AssignedDiplomProject
                {
                    DiplomProjectId = projectId
                };
                Context.AssignedDiplomProjects.Add(assignment);
            }

            assignment.StudentId = studentId;
            assignment.ApproveDate = isLecturer ? (DateTime?)DateTime.Now : null;
            Context.SaveChanges();
        }

        public void DeleteAssignment(int userId, int id)
        {
            var project = Context.AssignedDiplomProjects.Single(x => x.DiplomProjectId == id);
            Context.AssignedDiplomProjects.Remove(project);
            Context.SaveChanges();
        }

        public PagedList<StudentData> GetStudentsByDiplomProjectId(GetPagedListParams parms)
        {
            if (!parms.Filters.ContainsKey("diplomProjectId"))
            {
                throw new ApplicationException("diplomPorjectId can't be empty!");
            }

            var diplomProjectId = int.Parse(parms.Filters["diplomProjectId"]);

            return Context.Students
                .Include(x => x.Group.DiplomProjectGroups)
                .Where(x => x.Group.DiplomProjectGroups.Any(dpg => dpg.DiplomProjectId == diplomProjectId))
                .Where(x => !x.AssignedDiplomProjects.Any())
                .OrderBy(x => x.Group.Name).ThenBy(x => x.LastName)
                .Select(s => new StudentData
                {
                    Id = s.Id,
                    Name = s.LastName + " " + s.FirstName + " " + s.MiddleName, //todo
                    Group = s.Group.Name
                }).ApplyPaging(parms);
        }

        private void ValidateLecturerAccess(int userId)
        {
            if (!IsLecturer(userId))
            {
                throw new ApplicationException("Only lecturers able to remove assignments!");
            }
        }

        private bool IsLecturer(int userId)
        {
            return Context.Users.Include(x => x.Student).Include(x => x.Lecturer).Single(x => x.Id == userId).Lecturer != null;
        }
    }
}
