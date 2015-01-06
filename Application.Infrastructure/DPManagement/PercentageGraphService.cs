using System;
using System.Collections.Generic;
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

        public PagedList<PercentageGraphData> GetPercentageGraphs(int userId, GetPagedListParams parms)
        {
            var groupId = 0;
            if (parms.Filters.ContainsKey("groupId"))
            {
                int.TryParse(parms.Filters["groupId"], out groupId);
            }

            var user = Context.Users.Include(x => x.Student.Group).Include(x => x.Lecturer).Single(x => x.Id == userId);

            var isLecturer = user.Lecturer != null;
            var isStudent = user.Student != null;
            var isSecretary = isLecturer && user.Lecturer.IsSecretary;
            if (isLecturer && !isSecretary)
            {
                return GetPercentageGraphsForLecturer(userId, parms, groupId);
            }

            var secretaryId = isStudent ? user.Student.Group.SecretaryId : userId;
            return Context.DiplomPercentagesGraphs
                .AsNoTracking()
                .Where(x => x.LecturerId == secretaryId)
                .Select(ToPercentageDataPlain)
                .ApplyPaging(parms);
        }

        public PagedList<PercentageGraphData> GetPercentageGraphsForLecturer(int lecturerId, GetPagedListParams parms, int secretaryId)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, lecturerId);

            parms.SortExpression = "Date";
            return GetPercentageGraphDataForLecturerQuery(lecturerId, secretaryId).ApplyPaging(parms);
        }

        public List<PercentageGraphData> GetPercentageGraphsForLecturerAll(int userId, GetPagedListParams parms)
        {
            var secretaryId = 0;
            if (parms.Filters.ContainsKey("secretaryId"))
            {
                int.TryParse(parms.Filters["secretaryId"], out secretaryId);
            }

            var isStudent = AuthorizationHelper.IsStudent(Context, userId);
            var isLecturer = AuthorizationHelper.IsLecturer(Context, userId);
            var isLecturerSecretary = isLecturer && Context.Lecturers.Single(x => x.Id == userId).IsSecretary;
            secretaryId = isLecturerSecretary ? userId : secretaryId;

            if (isStudent)
            {
                secretaryId = Context.Users.Where(x => x.Id == userId).Select(x => x.Student.Group.SecretaryId).Single() ?? 0;
            }

            return GetPercentageGraphDataForLecturerQuery(isLecturerSecretary || isStudent ? 0 : userId, secretaryId)
                .Where(x => x.Date >= _currentAcademicYearStartDate && x.Date < _currentAcademicYearEndDate)
                .OrderBy(x => x.Date)
                .ToList();
        }

        public List<DiplomProjectConsultationDateData> GetConsultationDatesForUser(int userId)
        {
            if (AuthorizationHelper.IsStudent(Context, userId))
            {
                var student = Context.Students
                    .Include(x => x.AssignedDiplomProjects.Select(adp => adp.DiplomProject))
                    .Single(x => x.User.Id == userId);
                if (student.AssignedDiplomProjects.Count == 0)
                {
                    return new List<DiplomProjectConsultationDateData>();
                }

                userId = student.AssignedDiplomProjects.First().DiplomProject.LecturerId ?? 0;
            }

            return Context.DiplomProjectConsultationDates
                .Where(x => x.Day >= _currentAcademicYearStartDate && x.Day < _currentAcademicYearEndDate)
                .Where(x => x.LecturerId == userId)
                .OrderBy(x => x.Day)
                .Select(x => new DiplomProjectConsultationDateData
                {
                    Day = x.Day,
                    LecturerId = x.LecturerId,
                    Id = x.Id
                })
                .ToList();
        }

        /// <summary>
        /// Lecturer.DiplomProjects=>Groups.Secretary.PercentageGraphs
        /// </summary>
        /// <param name="lecturerId"></param>
        /// <param name="secretaryId"></param>
        /// <returns></returns>
        private IQueryable<PercentageGraphData> GetPercentageGraphDataForLecturerQuery(int lecturerId, int secretaryId)
        {
            return Context.Lecturers.Where(x => lecturerId == 0 || x.Id == lecturerId)
                .SelectMany(x => x.DiplomProjects
                    .SelectMany(dp => dp.DiplomProjectGroups.Where(dpg => secretaryId == 0 || dpg.Group.SecretaryId == secretaryId)
                        .SelectMany(dpg => dpg.Group.Secretary.DiplomPercentagesGraphs)))
                .Distinct().Select(ToPercentageDataPlain);
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
            AuthorizationHelper.ValidateLecturerAccess(Context, userId);

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

            //            percentage.DiplomPercentagesGraphToGroups = percentage.DiplomPercentagesGraphToGroups ??
            //                                                        new Collection<DiplomPercentagesGraphToGroup>();

            //            var currentGroups = percentage.DiplomPercentagesGraphToGroups.ToList();
            //            var newGroups = percentageData.SelectedGroupsIds.Select(x => new DiplomPercentagesGraphToGroup
            //            {
            //                GroupId = x,
            //                DiplomPercentagesGraphId = percentage.Id
            //            }).ToList();
            //
            //            var groupsToAdd = newGroups.Except(currentGroups, grp => grp.GroupId).ToList();
            //            var groupsToDelete = currentGroups.Except(newGroups, grp => grp.GroupId).ToList();
            //
            //            groupsToAdd.ForEach(grp => percentage.DiplomPercentagesGraphToGroups.Add(grp));
            //            groupsToDelete.ForEach(grp => Context.DiplomPercentagesGraphToGroup.Remove(grp));
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