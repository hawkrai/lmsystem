using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Application.Core;
using Application.Infrastructure.CTO;
using LMPlatform.Data.Infrastructure;

namespace Application.Infrastructure.CPManagement
{
    public class CpCorrelationService : ICpCorrelationService
    {
        public CpCorrelationService()
        {
            _correlationMethodsMapping = new Dictionary<string, Func<int , int?, List<Correlation>>>
            {
                { "Group", GetGroupsCorrelation },
                { "CourseProject", GetCourseProjectCorrelation },
                { "CourseLecturer", GetCourseLecturerCorrelation },
                { "LecturerCourseGroup", GetLecturerCourseGroupsCorrelation },
                { "CourseProjectTaskSheetTemplate", GetCourseProjectTaskSheetTemplateCorrelation },
            };
        }

        private readonly LazyDependency<ICpContext> context = new LazyDependency<ICpContext>();

        private ICpContext Context
        {
            get { return context.Value; }
        }

        private readonly Dictionary<string, Func<int, int?, List<Correlation>>> _correlationMethodsMapping;

        public List<Correlation> GetCorrelation(string entity, int subjectId, int? id)
        {
            if (!_correlationMethodsMapping.ContainsKey(entity))
            {
                throw new Exception("CorrelationService doesn't serve this entity type!");
            }

            return _correlationMethodsMapping[entity](subjectId, id);
        }

        private List<Correlation> GetGroupsCorrelation(int subjectId, int? userId)
        {
            var groups = Context.GetGraduateGroups();

            if (userId.HasValue)
            {
                var user = Context.Users.Include(x => x.Lecturer).Single(x => x.Id == userId);
                var isSecretary = user.Lecturer != null && user.Lecturer.IsSecretary;
               /* if (isSecretary)
                {
                    groups = groups.Where(x => !x.SecretaryId.HasValue || x.SecretaryId == userId);
                }*/
            }

            return groups.OrderBy(x => x.Name).Select(x => new Correlation
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
        }

        /// <summary>
        /// Lecturer.AssignedProjects.Students.Groups.Secretary
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<Correlation> GetLecturerCourseGroupsCorrelation(int subjectId, int? id)
        {
            if (!id.HasValue)
            {
                throw new ApplicationException("userId cant be null!");
            }

            return AuthorizationHelper.IsLecturer(Context, id.Value)
                ? Context.Lecturers
                    .Include(x => x.CourseProjects.Select(
                                dp => dp.AssignedCourseProjects.Select(adp => adp.Student.Group.Secretary)))
                    .Single(x => x.Id == id)
                    .CourseProjects.Where(dp => dp.AssignedCourseProjects.Any())
                    .SelectMany(dp => dp.AssignedCourseProjects.Select(ap => ap.Student.Group))
                    .Where(x => x.Secretary != null)
                    .GroupBy(x => x.Secretary, (secretary, groups) => new { secretary, groups })
                    .ToList().Select(x => new Correlation
                    {
                        Id = x.secretary.Id,
                        Name = string.Format("{0} ({1})", x.secretary.FullName, string.Join(", ", x.groups.Select(g => g.Name).Distinct()))
                    }).ToList()
                : AuthorizationHelper.IsStudent(Context, id.Value)
                    ? Context.Students.Where(x => x.Id == id).Select(x => x.Group.Secretary).Where(x => x != null).ToList()
                    .Select(x => new Correlation
                    {
                        Id = x.Id,
                        Name = x.FullName
                    }).ToList() : new List<Correlation>();
        }

       private List<Correlation> GetCourseProjectTaskSheetTemplateCorrelation(int subjectId, int? id)
        {
            return Context.CourseProjectTaskSheetTemplates
                    .Select(x => new Correlation
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .OrderBy(x => x.Name)
                    .ToList();
        }
        
        private List<Correlation> GetCourseProjectCorrelation(int subjectId, int? lecturerId)
        {
            var projects = Context.CourseProjects.Where(x=>x.SubjectId == subjectId);
            var user = Context.Students.Any(x => x.Id == lecturerId);
            if (user)
            {
                projects = Context.AssignedCourseProjects.Where(x=>x.StudentId == lecturerId).Select(x=>x.CourseProject);
            }
            return projects.Select(x => new Correlation
                {
                    Id = x.CourseProjectId,
                    Name = x.Theme
                }).OrderBy(x => x.Name).ToList();
        }

        private List<Correlation> GetCourseLecturerCorrelation(int subjectId, int? id)
        {
            return Context.Lecturers
                .Where(x => x.IsLecturerHasGraduateStudents && x.CourseProjects.Any(dp => dp.AssignedCourseProjects.Any()))
                .ToList()
                .Select(x => new Correlation
                {
                    Id = x.Id,
                    Name = x.FullName
                }).OrderBy(x => x.Name).ToList();
        }
    }
}