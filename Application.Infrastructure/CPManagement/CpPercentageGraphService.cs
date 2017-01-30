using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Application.Core;
using Application.Core.Data;
using Application.Core.Extensions;
using Application.Infrastructure.CTO;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Models.CP;

namespace Application.Infrastructure.CPManagement
{
    public class CpPercentageGraphService : ICpPercentageGraphService
    {
        private readonly LazyDependency<ICpContext> context = new LazyDependency<ICpContext>();

        private ICpContext Context
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

            var subjectId = 0;
            if (parms.Filters.ContainsKey("subjectId"))
            {
                int.TryParse(parms.Filters["subjectId"], out subjectId);
            }

            var user = Context.Users.Include(x => x.Student.Group).Include(x => x.Lecturer).Single(x => x.Id == userId);

            var isLecturer = user.Lecturer != null;
            var isStudent = user.Student != null;
            var isSecretary = isLecturer && user.Lecturer.IsSecretary;
            
            var secretaryId = isStudent ? user.Student.Group.SecretaryId : userId;
            if (isStudent)
            {
                secretaryId = Context.Users.Where(x => x.Id == userId)
                           .Select(x => x.Student.AssignedCourseProjects.FirstOrDefault().CourseProject.LecturerId)
                           .Single() ?? 0;
            }
            return Context.CoursePercentagesGraphs
                .AsNoTracking()
                .Where(x => x.LecturerId == secretaryId)
                .Where(x => x.SubjectId == subjectId)
                .Select(ToPercentageDataPlain)
                .ApplyPaging(parms);
        }

        public PagedList<PercentageGraphData> GetPercentageGraphsForLecturer(int lecturerId, GetPagedListParams parms, int secretaryId)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, lecturerId);

            parms.SortExpression = "Date";
            return GetPercentageGraphDataForLecturerQuery(lecturerId, secretaryId, 2).ApplyPaging(parms);
        }

        public List<PercentageGraphData> GetPercentageGraphsForLecturerAll(int userId, GetPagedListParams parms)
        {
            var secretaryId = 0;
            if (parms.Filters.ContainsKey("secretaryId"))
            {
                int.TryParse(parms.Filters["secretaryId"], out secretaryId);
            }
            var subjectId = 0;
            if (parms.Filters.ContainsKey("subjectId"))
            {
                int.TryParse(parms.Filters["subjectId"], out subjectId);
            }
            var isStudent = AuthorizationHelper.IsStudent(Context, userId);
            var isLecturer = AuthorizationHelper.IsLecturer(Context, userId);
            var isLecturerSecretary = isLecturer && Context.Lecturers.Single(x => x.Id == userId).IsSecretary;
            secretaryId = userId;

            if (isStudent)
            {

                secretaryId = Context.Users.Where(x => x.Id == userId)
                            .Select(x => x.Student.AssignedCourseProjects.FirstOrDefault().CourseProject.LecturerId)
                            .Single() ?? 0;
            }

            return GetPercentageGraphDataForLecturerQuery(isLecturer || isStudent ? 0 : userId, secretaryId, subjectId)
                .Where(x => x.Date >= _currentAcademicYearStartDate && x.Date < _currentAcademicYearEndDate)
                .OrderBy(x => x.Date)
                .ToList();
        }

         public List<CourseProjectConsultationDateData> GetConsultationDatesForUser(int userId, int subjectId)
        {
            if (AuthorizationHelper.IsStudent(Context, userId))
            {
                var student = Context.Students
                    .Include(x => x.AssignedCourseProjects.Select(adp => adp.CourseProject))
                    .Single(x => x.User.Id == userId);
                if (student.AssignedCourseProjects.Count == 0)
                {
                    return new List<CourseProjectConsultationDateData>();
                }

                userId = student.AssignedCourseProjects.First().CourseProject.LecturerId ?? 0;
            }

            return Context.CourseProjectConsultationDates
                .Where(x => x.Day >= _currentAcademicYearStartDate && x.Day < _currentAcademicYearEndDate)
                .Where(x => x.LecturerId == userId)
                .Where(x => x.SubjectId == subjectId)
                .OrderBy(x => x.Day)
                .Select(x => new CourseProjectConsultationDateData
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
        private IQueryable<PercentageGraphData> GetPercentageGraphDataForLecturerQuery(int lecturer, int secretaryId, int subjectId)
        {
            IQueryable<PercentageGraphData> p = Context.CoursePercentagesGraphs.Where(x => x.LecturerId == secretaryId).Where(x=>x.SubjectId == subjectId).
                Select(ToPercentageDataPlain);

            return p;
        }

        public PercentageGraphData GetPercentageGraph(int id)
        {
            return Context.CoursePercentagesGraphs
                .AsNoTracking()
                .Include(x => x.CoursePercentagesGraphToGroups)
                .Select(ToPercentageData)
                .Single(x => x.Id == id);
        }

        public void SavePercentage(int userId, PercentageGraphData percentageData)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, userId);
            CoursePercentagesGraph percentage;
            if (percentageData.Id.HasValue)
            {
                percentage = Context.CoursePercentagesGraphs
                              .Include(x => x.CoursePercentagesGraphToGroups)
                              .Single(x => x.Id == percentageData.Id);
                if (Context.CoursePercentagesGraphs.Any(x => x.SubjectId == percentageData.SubjectId && x.Name == percentageData.Name && x.Id != percentageData.Id))
                {
                    throw new ApplicationException("Этап с таким названием уже есть!");
                }
            }
            else
            {
				if (Context.CoursePercentagesGraphs.Any(x => x.SubjectId == percentageData.SubjectId && x.Name == percentageData.Name))
                {
                    throw new ApplicationException("Этап с таким названием уже есть!");
                }
                percentage = new CoursePercentagesGraph();
                Context.CoursePercentagesGraphs.Add(percentage);
                percentage.SubjectId = percentageData.SubjectId;
            }

            percentage.LecturerId = userId;
            percentage.Name = percentageData.Name;
            percentage.Percentage = percentageData.Percentage;
            percentage.Date = percentageData.Date;
            Context.SaveChanges();
        }

        public void DeletePercentage(int userId, int id)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, userId);

            var percentage = Context.CoursePercentagesGraphs.Single(x => x.Id == id);
            Context.CoursePercentagesGraphs.Remove(percentage);
            Context.SaveChanges();
        }

        public void SavePercentageResult(int userId, PercentageResultData percentageResultData)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, userId);

            CoursePercentagesResult coursePercentagesResult;
            if (percentageResultData.Id.HasValue)
            {
                coursePercentagesResult = Context.CoursePercentagesResults
                    .Single(x => x.Id == percentageResultData.Id);
            }
            else
            {
                coursePercentagesResult = new CoursePercentagesResult
                {
                    StudentId = percentageResultData.StudentId,
                    CoursePercentagesGraphId = percentageResultData.PercentageGraphId
                };
                Context.CoursePercentagesResults.Add(coursePercentagesResult);
            }

            coursePercentagesResult.Mark = percentageResultData.Mark;
            coursePercentagesResult.Comments = percentageResultData.Comment;

            Context.SaveChanges();
        }

        public void SaveConsultationMark(int userId, CourseProjectConsultationMarkData consultationMarkData)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, userId);

            CourseProjectConsultationMark consultationMark;
            if (consultationMarkData.Id.HasValue)
            {
                consultationMark = Context.CourseProjectConsultationMarks
                    .Single(x => x.Id == consultationMarkData.Id);
            }
            else
            {
                consultationMark = new CourseProjectConsultationMark
                {
                    StudentId = consultationMarkData.StudentId,
                    ConsultationDateId = consultationMarkData.ConsultationDateId
                };
                Context.CourseProjectConsultationMarks.Add(consultationMark);
            }

            consultationMark.Mark = string.IsNullOrWhiteSpace(consultationMarkData.Mark) ? null : consultationMarkData.Mark;

            Context.SaveChanges();
        }

        public void SaveConsultationDate(int userId, DateTime date, int subjectId)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, userId);

            Context.CourseProjectConsultationDates.Add(new CourseProjectConsultationDate
            {
                Day = date,
                LecturerId = userId,
                SubjectId = subjectId
             
            });

            Context.SaveChanges();
        }

        public void DeleteConsultationDate(int userId, int id)
        {
            AuthorizationHelper.ValidateLecturerAccess(Context, userId);

            var consultation = Context.CourseProjectConsultationDates.Single(x => x.Id == id);
            Context.CourseProjectConsultationDates.Remove(consultation);
            Context.SaveChanges();
        }
        
        private static readonly Expression<Func<CoursePercentagesGraph, PercentageGraphData>> ToPercentageData =
            x => new PercentageGraphData
        {
            Id = x.Id,
            Date = x.Date,
            Name = x.Name,
            Percentage = x.Percentage,
            SelectedGroupsIds = x.CoursePercentagesGraphToGroups.Select(dpg => dpg.GroupId)
        };

        private static readonly Expression<Func<CoursePercentagesGraph, PercentageGraphData>> ToPercentageDataPlain =
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