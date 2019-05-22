using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories
{
	public class SubjectRepository : RepositoryBase<LmPlatformModelsContext, Subject>, ISubjectRepository
	{
		public SubjectRepository(LmPlatformModelsContext dataContext)
			: base(dataContext)
		{
		}

		public List<Subject> GetSubjects(int groupId = 0, int lecturerId = 0)
		{
			using (var context = new LmPlatformModelsContext())
			{
				if (groupId != 0)
				{
					var subjectGroup = context.Set<SubjectGroup>().Include(e => e.Subject.SubjectGroups.Select(x => x.SubjectStudents))
                        .Include(e => e.Subject.Labs)
											.Include(e => e.Subject.SubjectGroups.Select(x => x.Group))
                       .Include(e => e.Subject.SubjectLecturers)
											.Include(e => e.Subject.SubjectGroups.Select(x => x.SubGroups.Select(t => t.ScheduleProtectionLabs)))
											
											.Include(e => e.Subject.SubjectGroups.Select(x => x.SubGroups.Select(v => v.SubjectStudents)))
                        .Include(e => e.Subject.SubjectLecturers.Select(x => x.Lecturer))
						 .Include(e => e.Subject.LecturesScheduleVisitings)
							.Where(e => e.GroupId == groupId).ToList();
					return subjectGroup.Select(e => e.Subject).ToList();
				}

				var subjectLecturer =
					context.Set<SubjectLecturer>()
					.Include(e => e.Subject.SubjectGroups.Select(x => x.SubjectStudents))
                    .Include(e => e.Subject.LecturesScheduleVisitings)
                    .Include(e => e.Subject.Labs)
                        .Include(e=>e.Subject.SubjectGroups.Select(x => x.Group))
                        .Include(e=>e.Subject.SubjectGroups.Select(x => x.Group.Students))
                    .Include(e => e.Subject.SubjectGroups.Select(x => x.SubGroups.Select(t => t.ScheduleProtectionLabs)))
					.Where(e => e.LecturerId == lecturerId).ToList();
				return subjectLecturer.Select(e => e.Subject).ToList();
			}
		}

		public SubjectNews SaveNews(SubjectNews news)
		{
			using (var context = new LmPlatformModelsContext())
			{
				if (news.Id != 0)
				{
					context.Update(news);
				}
				else
				{
					context.Add(news);
				}

				context.SaveChanges();
			}

			return news;
		}

		public bool IsSubjectName(string name, string id)
		public bool IsSubjectName(string name, string id, int userId)
		{
			using (var context = new LmPlatformModelsContext())
			{
				var idN = int.Parse(id);
				if (context.Set<Subject>().Any(e => e.Name == name && !e.IsArchive && e.Id != idN))
				if (context.Set<Subject>().Include(e => e.SubjectLecturers).Any(e => e.Name == name && !e.IsArchive && e.Id != idN && e.SubjectLecturers.Any(x => x.LecturerId == userId)))
				{
					return true;
				}
			}

			return false;
		}

		public bool IsSubjectShortName(string name, string id)
		public bool IsSubjectShortName(string name, string id, int userId)
		{
			using (var context = new LmPlatformModelsContext())
			{
				var idN = int.Parse(id);
				if (context.Set<Subject>().Any(e => e.ShortName == name && !e.IsArchive && e.Id != idN))
				if (context.Set<Subject>().Include(e => e.SubjectLecturers).Any(e => e.ShortName == name && !e.IsArchive && e.Id != idN && e.SubjectLecturers.Any(x => x.LecturerId == userId)))
				{
					return true;
				}
			}

			return false;
		}

		public void DeleteNews(SubjectNews news)
		{
			using (var context = new LmPlatformModelsContext())
			{
				var model = context.Set<SubjectNews>().FirstOrDefault(e => e.Id == news.Id);
				context.Delete(model);

				context.SaveChanges();
			}
		}

		public void DeleteLection(Lectures lectures)
		{
			using (var context = new LmPlatformModelsContext())
			{
				var model = context.Set<Lectures>().FirstOrDefault(e => e.Id == lectures.Id);

				context.Delete(model);

				context.SaveChanges();
			}
		}

		protected override void PerformAdd(Subject model, LmPlatformModelsContext dataContext)
		{
			base.PerformAdd(model, dataContext);

			foreach (var subjectModule in model.SubjectModules)
			{
				subjectModule.SubjectId = model.Id;
				dataContext.Set<SubjectModule>().Add(subjectModule);
			}

			foreach (var subjectGroup in model.SubjectGroups)
			{
				subjectGroup.SubjectId = model.Id;
				dataContext.Set<SubjectGroup>().Add(subjectGroup);
			}

			model.SubjectLecturers.FirstOrDefault().SubjectId = model.Id;
			dataContext.Set<SubjectLecturer>().Add(model.SubjectLecturers.FirstOrDefault());

			dataContext.SaveChanges();
		}

		protected override void PerformUpdate(Subject newValue, LmPlatformModelsContext dataContext)
		{
			var subjectModules = dataContext.Set<SubjectModule>().Where(e => e.SubjectId == newValue.Id).ToList();

			foreach (var subjectModule in subjectModules)
			{
				if (newValue.SubjectModules.All(e => e.ModuleId != subjectModule.ModuleId))
				{
					dataContext.Set<SubjectModule>().Remove(subjectModule);
				}
			}

			if (newValue.SubjectModules != null)
			{
				foreach (var subjectModule in newValue.SubjectModules)
				{
					if (subjectModules.All(e => e.ModuleId != subjectModule.ModuleId))
					{
						dataContext.Set<SubjectModule>().Add(subjectModule);
					}
				}
			}

			var subjectGroups = dataContext.Set<SubjectGroup>().Where(e => e.SubjectId == newValue.Id).ToList();

			foreach (var subjectGroup in subjectGroups)
			{
				if (newValue.SubjectGroups.All(e => e.GroupId != subjectGroup.GroupId))
				{
				}
			}
		    
            if (newValue.SubjectGroups != null)

			if (newValue.SubjectGroups != null)
			{
					if (subjectGroups.All(e => e.GroupId != subjectGroup.GroupId))
					{
						dataContext.Set<SubjectGroup>().Add(subjectGroup);
					}
				}
			}

			dataContext.SaveChanges();

			base.PerformUpdate(newValue, dataContext);
		}

		public void DisableNews(int subjectId, bool disable)
		{
			using (var context = new LmPlatformModelsContext())
			{
				var models = context.Set<SubjectNews>().Where(e => e.SubjectId == subjectId);

				foreach (var subjectNewse in models)
				{
					subjectNewse.Disabled = disable;
				}

				context.Update<SubjectNews, LmPlatformModelsContext>(models);

				context.SaveChanges();
			}
		}
	}
}