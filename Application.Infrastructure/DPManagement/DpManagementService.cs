using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Application.Core;
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

        public List<DiplomProjectData> GetProjects(int page, int count, out int total)
        {
            var query = Context.DiplomProjects.AsNoTracking().Include(x => x.Lecturer).OrderBy(x => x.Lecturer.LastName);
            total = query.Count();
            return query.Skip((page - 1) * count).Take(count).ToList()
                .Select(dp => new DiplomProjectData
                    {
                        Id = dp.DiplomProjectId,
                        Theme = dp.Theme,
                        Lecturer = dp.Lecturer.FullName
                    })
                .ToList();
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

        public void DeleteProject(int id)
        {
            var project = Context.DiplomProjects.Single(x => x.DiplomProjectId == id);
            Context.DiplomProjects.Remove(project);
            Context.SaveChanges();
        }
    }
}
