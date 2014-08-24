using System;
using System.Collections.Generic;
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

        public PagedList<PercentageGraphData> GetPercentageGraphsForSecretary(int lecturerId, GetPagedListParams parms)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, lecturerId);

            var isSecretary = Context.Lecturers.Single(x => x.Id == lecturerId).IsSecretary;
            if (!isSecretary)
            {
                return GetPercentageGraphsForLecturer(lecturerId, parms);
            }

            return Context.DiplomPercentagesGraphs
                .AsNoTracking()
                .Select(ToPercentageDataPlain)
                .ApplyPaging(parms);
        }

        public PagedList<PercentageGraphData> GetPercentageGraphsForLecturer(int lecturerId, GetPagedListParams parms)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, lecturerId);

            parms.SortExpression = "Date";
            return GetPercentageGraphDataForLecturerQuery(lecturerId).ApplyPaging(parms);
        }

        public List<PercentageGraphData> GetPercentageGraphsForLecturerAll(int lecturerId)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, lecturerId);

            return GetPercentageGraphDataForLecturerQuery(lecturerId)
                .Where(x => x.Date >= _currentAcademicYearStartDate && x.Date < _currentAcademicYearEndDate)
                .OrderBy(x => x.Date)
                .ToList();
        }

        public List<DiplomProjectConsultationDateData> GetConsultationDatesForLecturer(int lecturerId)
        {
            return Context.DiplomProjectConsultationDates
                .Where(x => x.Day >= _currentAcademicYearStartDate && x.Day < _currentAcademicYearEndDate)
                .OrderBy(x => x.Day)
                .Select(x => new DiplomProjectConsultationDateData
                {
                    Day = x.Day,
                    LecturerId = x.LecturerId,
                    Id = x.Id
                })
                .ToList();
        }

        private IQueryable<PercentageGraphData> GetPercentageGraphDataForLecturerQuery(int lecturerId)
        {
            var query =
                (from dpg in Context.DiplomPercentagesGraphs.AsNoTracking()
                 join dpgg in Context.DiplomPercentagesGraphToGroup.AsNoTracking()
                     on dpg.Id equals dpgg.DiplomPercentagesGraphId
                 join grp in Context.Groups.AsNoTracking()
                     on dpgg.GroupId equals grp.Id
                 join dptg in Context.DiplomProjectGroups.AsNoTracking()
                     on grp.Id equals dptg.GroupId
                 join dp in Context.DiplomProjects.AsNoTracking()
                     on dptg.DiplomProjectId equals dp.DiplomProjectId
                 where dp.LecturerId == lecturerId
                 group dpg by dpg
                     into groupedDpg
                     select groupedDpg.FirstOrDefault())
                    .Select(ToPercentageDataPlain);
            return query;
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
            AuthorizationHelper.ValidateLecturerAccess(Context, userId);

            var percentage = Context.DiplomPercentagesGraphs.Single(x => x.Id == id);
            Context.DiplomPercentagesGraphs.Remove(percentage);
            Context.SaveChanges();
        }

        public void SavePercentageResult(int userId, PercentageResultData percentageResultData)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, userId);

            DiplomPercentagesResult diplomPercentagesResult;
            if (percentageResultData.Id.HasValue)
            {
                diplomPercentagesResult = Context.DiplomPercentagesResults
                    .Single(x => x.Id == percentageResultData.Id);
            }
            else
            {
                diplomPercentagesResult = new DiplomPercentagesResult
                {
                    StudentId = percentageResultData.StudentId,
                    DiplomPercentagesGraphId = percentageResultData.PercentageGraphId
                };
                Context.DiplomPercentagesResults.Add(diplomPercentagesResult);
            }

            diplomPercentagesResult.Mark = percentageResultData.Mark;
            diplomPercentagesResult.Comments = percentageResultData.Comment;

            Context.SaveChanges();
        }

        public void SaveConsultationMark(int userId, DipomProjectConsultationMarkData consultationMarkData)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, userId);

            DiplomProjectConsultationMark consultationMark;
            if (consultationMarkData.Id.HasValue)
            {
                consultationMark = Context.DiplomProjectConsultationMarks
                    .Single(x => x.Id == consultationMarkData.Id);
            }
            else
            {
                consultationMark = new DiplomProjectConsultationMark
                {
                    StudentId = consultationMarkData.StudentId,
                    ConsultationDateId = consultationMarkData.ConsultationDateId
                };
                Context.DiplomProjectConsultationMarks.Add(consultationMark);
            }

            consultationMark.Mark = string.IsNullOrWhiteSpace(consultationMarkData.Mark) ? null : consultationMarkData.Mark;

            //            consultationMark.Comments = consultationMarkData.Comment;
            Context.SaveChanges();
        }

        public void SaveConsultationDate(int userId, DateTime date)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, userId);

            Context.DiplomProjectConsultationDates.Add(new DiplomProjectConsultationDate
            {
                Day = date,
                LecturerId = userId
            });
            
            Context.SaveChanges();
        }

        public void DeleteConsultationDate(int userId, int id)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, userId);

            var consultation = Context.DiplomProjectConsultationDates.Single(x => x.Id == id);
            Context.DiplomProjectConsultationDates.Remove(consultation);
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

        private static readonly Expression<Func<DiplomPercentagesGraph, PercentageGraphData>> ToPercentageDataPlain =
            x => new PercentageGraphData
        {
            Id = x.Id,
            Date = x.Date,
            Name = x.Name,
            Percentage = x.Percentage,
        };

        private readonly DateTime _currentAcademicYearStartDate = DateTime.Now.Month < 9
            ? new DateTime(DateTime.Now.Year - 1, 9, 1)
            : new DateTime(DateTime.Now.Year, 9, 1);

        private readonly DateTime _currentAcademicYearEndDate = DateTime.Now.Month < 9
            ? new DateTime(DateTime.Now.Year, 9, 1)
            : new DateTime(DateTime.Now.Year + 1, 9, 1);
    }
}