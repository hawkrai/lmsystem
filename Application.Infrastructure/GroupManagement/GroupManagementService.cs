using System.Collections.Generic;
using System.Linq;
using Application.Core.Data;
using Application.SearchEngine.SearchMethods;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;
using Application.Core;
using LMPlatform.Data.Infrastructure;

namespace Application.Infrastructure.GroupManagement
{
    public class GroupManagementService : IGroupManagementService
    {
        public Group GetGroup(int groupId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.GroupsRepository.GetBy(new Query<Group>(e => e.Id == groupId).Include(e => e.Students.Select(x => x.LecturesVisitMarks)));
            }
        }

        public List<Group> GetGroups(IQuery<Group> query = null)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.GroupsRepository.GetAll(query).ToList();
            }
        }

        public IPageableList<Group> GetGroupsPageable(string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null)
        {
            var query = new PageableQuery<Group>(pageInfo);

            if (!string.IsNullOrEmpty(searchString))
            {
                query.AddFilterClause(
                    e => e.Name.ToLower().StartsWith(searchString) || e.Name.ToLower().Contains(searchString));
            }

            query.OrderBy(sortCriterias).Include(g => g.Students);
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var groups = repositoriesContainer.GroupsRepository.GetPageableBy(query);
                return groups;
            }
        }

        public Group AddGroup(Group group)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.GroupsRepository.Save(group);
                repositoriesContainer.ApplyChanges();
            }
            new GroupSearchMethod().AddToIndex(group);
            return group;
        }

        public Group UpdateGroup(Group group)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.GroupsRepository.Save(group);
                repositoriesContainer.ApplyChanges();
            }
            new GroupSearchMethod().UpdateIndex(group);
            return group;
        }

        public void DeleteGroup(int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var group = repositoriesContainer.GroupsRepository.GetBy(new Query<Group>().AddFilterClause(g => g.Id == id));
                repositoriesContainer.GroupsRepository.Delete(group);
                repositoriesContainer.ApplyChanges();
            }
            new GroupSearchMethod().DeleteIndex(id);
        }

		public List<string> GetLabsScheduleVisitings(int subjectId, int groupId, int subGorupId)
	    {
			var data = new List<string>();

			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var subject = repositoriesContainer.SubjectRepository.GetBy(new PageableQuery<Subject>(e => e.Id == subjectId)
					.Include(x => x.SubjectGroups.Select(t => t.SubGroups.Select(f => f.ScheduleProtectionLabs))));

				foreach (var scheduleProtectionLabs in subject.SubjectGroups.FirstOrDefault(e => e.GroupId == groupId).SubGroups.FirstOrDefault(e => e.Id == subGorupId).ScheduleProtectionLabs)
				{
					data.Add(scheduleProtectionLabs.Date.ToString("dd/MM/yyyy"));	
				}
			}

			return data;
	    }

	    public List<List<string>> GetLabsScheduleMarks(int subjectId, int groupId, int subGorupId)
	    {
			var data = new List<List<string>>();
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var group = repositoriesContainer.GroupsRepository.GetBy(new Query<Group>(e => e.Id == groupId).Include(e => e.SubjectGroups.Select(x => x.SubGroups.Select(u => u.SubjectStudents.Select(r => r.Student.ScheduleProtectionLabMarks.Select(n => n.ScheduleProtectionLab))))));

				foreach (var student in group.SubjectGroups.FirstOrDefault(e => e.SubjectId == subjectId).SubGroups.FirstOrDefault(e => e.Id == subGorupId).SubjectStudents.OrderBy(e => e.Student.FullName))
				{
					var rows = new List<string>();
					if (student.SubGroupId == subGorupId)
					{
						rows.Add(student.Student.FullName);

						rows.AddRange(student.Student.ScheduleProtectionLabMarks.OrderBy(e => e.ScheduleProtectionLab.Date).Select(scheduleProtectionLabMark => scheduleProtectionLabMark.Mark));
					}

					data.Add(rows);
				}
			}
			return data;
	    }

        public List<string> GetCpScheduleVisitings(int subjectId, int groupId)
        {
            var data = new List<string>();

            var subject = Context.CourseProjectConsultationDates.Where(x => x.SubjectId == subjectId);

                foreach (var cp in subject)
                {
                    data.Add(cp.Day.ToString("dd/MM/yyyy"));
                }

            return data;
        }

        public List<List<string>> GetCpScheduleMarks(int subjectId, int groupId)
        {
            var data = new List<List<string>>();
            var groups = Context.Groups.Include("Students").Single(x=>x.Id == groupId);

            foreach (var student in groups.Students.OrderBy(e => e.FullName))
            {
                if (student.AssignedCourseProjects.Any(x=>x.CourseProject.SubjectId == subjectId))
                {
                    var rows = new List<string>();
                    rows.Add(student.FullName);
                    foreach (var cpd in Context.CourseProjectConsultationDates.Include("CourseProjectConsultationMarks").Where(x => x.SubjectId == subjectId))
                    {
                        var cpM = cpd.CourseProjectConsultationMarks.Where(x => x.StudentId == student.Id);
                        if (cpM.Count() > 0)
                        {
                            rows.Add("+");
                        }
                        else
                        {
                            rows.Add("-");
                        }
                    }
                    data.Add(rows);
                }
            }
            return data;
        }

        public List<string> GetCpPercentage(int subjectId, int groupId)
        {
            var data = new List<string>();

            var subject = Context.CoursePercentagesGraphs.Where(x => x.SubjectId == subjectId);

            foreach (var cp in subject)
            {
                data.Add(cp.Date.ToString("dd/MM/yyyy"));
            }

            return data;
        }

        public List<List<string>> GetCpMarks(int subjectId, int groupId)
        {
            var data = new List<List<string>>();
            var groups = Context.Groups.Include("Students").Single(x => x.Id == groupId);

            foreach (var student in groups.Students.OrderBy(e => e.FullName))
            {
                if (student.AssignedCourseProjects.Any(x => x.CourseProject.SubjectId == subjectId))
                {
                    var rows = new List<string>();
                    rows.Add(student.FullName);
                    foreach (var cpd in Context.CoursePercentagesGraphs.Include("CoursePercentagesResults").Where(x => x.SubjectId == subjectId))
                    {
                        var cpM = cpd.CoursePercentagesResults.FirstOrDefault(x => x.StudentId == student.Id);
                        if (cpM != null)
                        {
                            rows.Add(cpM.Mark.ToString());
                        }
                        else
                        {
                            rows.Add("-");
                        }
                    }
                    var st = student.AssignedCourseProjects.First();
                    rows.Add(st.Mark.ToString());
                    data.Add(rows);
                }
            }
            return data;
        }

        public Group GetGroupByName(string groupName)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var group = repositoriesContainer.GroupsRepository.GetBy(new Query<Group>().AddFilterClause(g => g.Name == groupName));

                return group;
            }
        }


        private readonly LazyDependency<ICpContext> context = new LazyDependency<ICpContext>();

        private ICpContext Context
        {
            get { return context.Value; }
        }
    }
}