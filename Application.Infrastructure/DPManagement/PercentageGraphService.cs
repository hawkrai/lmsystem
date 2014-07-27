using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Application.Core;
using Application.Core.Data;
using Application.Core.Extensions;
using Application.Infrastructure.DTO;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Models.DP;

namespace Application.Infrastructure.DPManagement
{
    public class PercentageGraphService : IPercentageGraphService
    {
        private readonly LazyDependency<IDpContext> context = new LazyDependency<IDpContext>();

        private IDpContext Context
        {
            get { return context.Value; }
        }

        public PagedList<PercentageGraphData> GetPercentageGraphs(GetPagedListParams parms)
        {
            return Context.DiplomPercentagesGraphs
                .AsNoTracking()
                .Select(ToPercentageData)
                .ApplyPaging(parms);
        }

        public PercentageGraphData GetPercentageGraph(int id)
        {
            return Context.DiplomPercentagesGraphs
                .AsNoTracking()
                .Include(x => x.DiplomPercentagesGraphToGroups)
                .Select(ToPercentageData)
                .Single(x => x.Id == id);
        }

        public void SavePercentage(int userId, PercentageGraphData percentageData)
        {
            DiplomPercentagesGraph percentage;
            if (percentageData.Id.HasValue)
            {
                percentage = Context.DiplomPercentagesGraphs
                              .Include(x => x.DiplomPercentagesGraphToGroups)
                              .Single(x => x.Id == percentageData.Id);
            }
            else
            {
                percentage = new DiplomPercentagesGraph();
                Context.DiplomPercentagesGraphs.Add(percentage);
            }

            percentage.DiplomPercentagesGraphToGroups = percentage.DiplomPercentagesGraphToGroups ??
                                                        new Collection<DiplomPercentagesGraphToGroup>();
            var currentGroups = percentage.DiplomPercentagesGraphToGroups.ToList();
            var newGroups = percentageData.SelectedGroupsIds.Select(x => new DiplomPercentagesGraphToGroup
            {
                GroupId = x,
                DiplomPercentagesGraphId = percentage.Id
            }).ToList();

            var groupsToAdd = newGroups.Except(currentGroups, grp => grp.GroupId).ToList();
            var groupsToDelete = currentGroups.Except(newGroups, grp => grp.GroupId).ToList();

            groupsToAdd.ForEach(grp => percentage.DiplomPercentagesGraphToGroups.Add(grp));
            groupsToDelete.ForEach(grp => Context.DiplomPercentagesGraphToGroup.Remove(grp));

            percentage.LecturerId = userId;
            percentage.Name = percentageData.Name;
            percentage.Percentage = percentageData.Percentage;
            percentage.Date = percentageData.Date;

            Context.SaveChanges();
        }

        public void DeletePercentage(int userId, int id)
        {
            ValidateLecturerAccess(userId);

            var percentage = Context.DiplomPercentagesGraphs.Single(x => x.Id == id);
            Context.DiplomPercentagesGraphs.Remove(percentage);
            Context.SaveChanges();
        }

        private static readonly Expression<Func<DiplomPercentagesGraph, PercentageGraphData>> ToPercentageData =
            x => new PercentageGraphData
        {
            Id = x.Id,
            Date = x.Date,
            Name = x.Name,
            Percentage = x.Percentage,
            SelectedGroupsIds = x.DiplomPercentagesGraphToGroups.Select(dpg => dpg.GroupId)
        };

        private void ValidateLecturerAccess(int userId)
        {
            if (!IsLecturer(userId))
            {
                throw new ApplicationException("Only lecturers able to remove percentages!");
            }
        }

        private bool IsLecturer(int userId)
        {
            return Context.Users.Include(x => x.Student).Include(x => x.Lecturer).Single(x => x.Id == userId).Lecturer != null;
        }
    }
}