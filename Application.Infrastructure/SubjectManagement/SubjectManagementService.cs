using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.FilesManagement;
using Application.Infrastructure.StudentManagement;
using LMPlatform.Data.Repositories;
using LMPlatform.Models;
using Application.Infrastructure.ConceptManagement;

namespace Application.Infrastructure.SubjectManagement
{
	using Models;

	public class SubjectManagementService : ISubjectManagementService
	{
		private readonly LazyDependency<IStudentManagementService> _studentManagementService = new LazyDependency<IStudentManagementService>();

		private readonly LazyDependency<IFilesManagementService> _filesManagementService =
			new LazyDependency<IFilesManagementService>();

		public IFilesManagementService FilesManagementService => _filesManagementService.Value;

		public IStudentManagementService StudentManagementService => _studentManagementService.Value;

		private readonly LazyDependency<IConceptManagementService> _conceptManagementService = new LazyDependency<IConceptManagementService>();

		public IConceptManagementService ConceptManagementService => _conceptManagementService.Value;

		public List<Subject> GetUserSubjects(int userId)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var user = repositoriesContainer.UsersRepository.GetBy(new Query<User>(e => e.Id == userId)
					.Include(e => e.Lecturer)
					.Include(e => e.Student));
				if (user.Student != null)
				{
					return repositoriesContainer.SubjectRepository.GetSubjects(groupId: user.Student.GroupId);
				}
				else
				{
					return repositoriesContainer.SubjectRepository.GetSubjects(lecturerId: user.Lecturer.Id);
				}
			}
		}

		public List<Subject> GetGroupSubjects(int groupId)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			return repositoriesContainer.SubjectRepository.GetSubjects(groupId: groupId).Where(e => !e.IsArchive).ToList();
		}

		public List<Subject> GetGroupSubjectsLite(int groupId)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			return repositoriesContainer.SubjectRepository.GetSubjectsLite(groupId).Where(e => !e.IsArchive).ToList();
		}

		public Subject GetSubject(int id)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			return repositoriesContainer.SubjectRepository
				.GetBy(new Query<Subject>(e => e.Id == id)
				.Include(e => e.SubjectModules.Select(x => x.Module))
				.Include(e => e.SubjectNewses)
				.Include(e => e.Lectures)
				.Include(e => e.Labs)
				.Include(e => e.SubjectLecturers.Select(x => x.Lecturer.User))
				.Include(e => e.Practicals)
				.Include(e => e.LecturesScheduleVisitings)
				.Include(e => e.SubjectGroups.Select(x => x.SubGroups.Select(v => v.ScheduleProtectionLabs))));
		}

		public Subject GetSubject(IQuery<Subject> query)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			return repositoriesContainer.SubjectRepository.GetBy(query);
		}

		public List<Labs> GetLabsV2(int subjectId)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			return repositoriesContainer.LabsRepository.GetAll(new Query<Labs>(e => e.SubjectId == subjectId)).ToList();
		}

		public IPageableList<Subject> GetSubjectsLecturer(int lecturerId, string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null)
		{
			var query = new PageableQuery<Subject>(pageInfo, e => e.SubjectLecturers.Any(x => x.LecturerId == lecturerId && x.Owner == null && !e.IsArchive));

			if (!string.IsNullOrEmpty(searchString))
			{
				query.AddFilterClause(
					e => e.Name.ToLower().StartsWith(searchString) || e.Name.ToLower().Contains(searchString));
			}

			query.OrderBy(sortCriterias);
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				return repositoriesContainer.SubjectRepository.GetPageableBy(query);
			}
		}

		public Subject SaveSubject(Subject subject)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			repositoriesContainer.SubjectRepository.Save(subject);
			repositoriesContainer.ApplyChanges();
			return subject;
		}

		public SubjectNews SaveNews(SubjectNews news)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			repositoriesContainer.SubjectRepository.SaveNews(news);
			repositoriesContainer.ApplyChanges();
			return news;
		}

		public void DeleteNews(SubjectNews news)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			repositoriesContainer.SubjectRepository.DeleteNews(news);
		}

		public void DeleteLection(Lectures lectures)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var lectModel =
					repositoriesContainer.LecturesRepository.GetBy(new Query<Lectures>(e => e.Id == lectures.Id));
				var deleteFiles =
						repositoriesContainer.AttachmentRepository.GetAll(
							new Query<Attachment>(e => e.PathName == lectModel.Attachments)).ToList();

				foreach (var attachment in deleteFiles)
				{
					FilesManagementService.DeleteFileAttachment(attachment);
				}

				repositoriesContainer.SubjectRepository.DeleteLection(lectures);
				repositoriesContainer.ApplyChanges();
			}
		}

		public void DeleteLectionVisitingDate(int id)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			var dateModelmarks =
				repositoriesContainer.RepositoryFor<LecturesVisitMark>()
					.GetAll(new Query<LecturesVisitMark>(e => e.LecturesScheduleVisitingId == id))
					.ToList();

			foreach (var lecturesVisitMark in dateModelmarks)
			{
				repositoriesContainer.RepositoryFor<LecturesVisitMark>().Delete(lecturesVisitMark);
			}

			repositoriesContainer.ApplyChanges();

			var dateModel =
				repositoriesContainer.RepositoryFor<LecturesScheduleVisiting>()
					.GetBy(new Query<LecturesScheduleVisiting>(e => e.Id == id));

			repositoriesContainer.RepositoryFor<LecturesScheduleVisiting>().Delete(dateModel);

			repositoriesContainer.ApplyChanges();
		}

		public void DeletePracticalsVisitingDate(int id)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			var dateModelmarks =
				repositoriesContainer.RepositoryFor<ScheduleProtectionPracticalMark>()
					.GetAll(new Query<ScheduleProtectionPracticalMark>(e => e.ScheduleProtectionPracticalId == id))
					.ToList();

			foreach (var practicalVisitMark in dateModelmarks)
			{
				repositoriesContainer.RepositoryFor<ScheduleProtectionPracticalMark>().Delete(practicalVisitMark);
			}

			repositoriesContainer.ApplyChanges();

			var dateModel =
				repositoriesContainer.RepositoryFor<ScheduleProtectionPractical>()
					.GetBy(new Query<ScheduleProtectionPractical>(e => e.Id == id));

			repositoriesContainer.RepositoryFor<ScheduleProtectionPractical>().Delete(dateModel);

			repositoriesContainer.ApplyChanges();
		}

		public void DeleteLabsVisitingDate(int id)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			var dateModelmarks =
				repositoriesContainer.RepositoryFor<ScheduleProtectionLabMark>()
					.GetAll(new Query<ScheduleProtectionLabMark>(e => e.ScheduleProtectionLabId == id))
					.ToList();

			foreach (var labsVisitMark in dateModelmarks)
			{
				repositoriesContainer.RepositoryFor<ScheduleProtectionLabMark>().Delete(labsVisitMark);
			}

			repositoriesContainer.ApplyChanges();

			var dateModel =
				repositoriesContainer.RepositoryFor<ScheduleProtectionLabs>()
					.GetBy(new Query<ScheduleProtectionLabs>(e => e.Id == id));

			repositoriesContainer.RepositoryFor<ScheduleProtectionLabs>().Delete(dateModel);

			repositoriesContainer.ApplyChanges();
		}

		public void DeleteLabs(int id)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			var labs =
				repositoriesContainer.LabsRepository.GetBy(
					new Query<Labs>(e => e.Id == id).Include(e => e.StudentLabMarks));

			var deleteFiles =
				repositoriesContainer.AttachmentRepository.GetAll(
					new Query<Attachment>(e => e.PathName == labs.Attachments)).ToList();

			var studentLabMarks =
				repositoriesContainer.RepositoryFor<StudentLabMark>()
					.GetAll(new Query<StudentLabMark>(e => e.LabId == id))
					.ToList();

			foreach (var attachment in deleteFiles)
			{
				FilesManagementService.DeleteFileAttachment(attachment);
			}

			foreach (var mark in studentLabMarks)
			{
				repositoriesContainer.RepositoryFor<StudentLabMark>().Delete(mark);
			}

			repositoriesContainer.ApplyChanges();

			repositoriesContainer.LabsRepository.Delete(labs);

			repositoriesContainer.ApplyChanges();
		}

		public void DeletePracticals(int id)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			var practicals =
				repositoriesContainer.PracticalRepository.GetBy(
					new Query<Practical>(e => e.Id == id).Include(e => e.StudentPracticalMarks));

			var deleteFiles =
				repositoriesContainer.AttachmentRepository.GetAll(
					new Query<Attachment>(e => e.PathName == practicals.Attachments)).ToList();

			var studentPracticalsMarks =
				repositoriesContainer.RepositoryFor<StudentPracticalMark>()
					.GetAll(new Query<StudentPracticalMark>(e => e.PracticalId == id))
					.ToList();

			foreach (var attachment in deleteFiles)
			{
				FilesManagementService.DeleteFileAttachment(attachment);
			}

			foreach (var mark in studentPracticalsMarks)
			{
				repositoriesContainer.RepositoryFor<StudentPracticalMark>().Delete(mark);
			}

			repositoriesContainer.ApplyChanges();

			repositoriesContainer.PracticalRepository.Delete(practicals);

			repositoriesContainer.ApplyChanges();
		}

		public bool IsWorkingSubject(int userId, int subjectId)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var subjectlect =
					repositoriesContainer.RepositoryFor<SubjectLecturer>()
						.GetAll(new Query<SubjectLecturer>(e => e.LecturerId == userId && e.SubjectId == subjectId))
						.ToList();

				return subjectlect.Any();
			}
		}
		
		public SubjectNews GetNews(int id, int subjectId)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			return repositoriesContainer.SubjectRepository
				.GetBy(new Query<Subject>(e => e.Id == subjectId).Include(e => e.SubjectNewses))
				.SubjectNewses
				.FirstOrDefault(e => e.Id == id);
		}

		public List<SubjectNews> GetNewsByGroup(int id)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var subjects =
					repositoriesContainer.RepositoryFor<SubjectGroup>().GetAll(new Query<SubjectGroup>(e => e.GroupId == id && e.IsActiveOnCurrentGroup)).Select(
						e => e.SubjectId).ToList();

				var subjectsData =
					repositoriesContainer.RepositoryFor<SubjectGroup>().GetAll(new Query<SubjectGroup>(e => e.GroupId == id && e.IsActiveOnCurrentGroup).Include(e => e.Subject)).ToList();

				var news =
					repositoriesContainer.RepositoryFor<SubjectNews>().GetAll(
						new Query<SubjectNews>(e => subjects.Contains(e.SubjectId) && !e.Disabled)).ToList();

				foreach (var subjectNewse in news)
				{
					subjectNewse.Subject = new Subject
					{
						Name = subjectsData.FirstOrDefault(e => e.SubjectId == subjectNewse.SubjectId).Subject.Name
					};
				}

				return news;
			}
		}

		public List<SubjectNews> GetNewsByLector(int id)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var subjects =
					repositoriesContainer.RepositoryFor<SubjectLecturer>().GetAll(new Query<SubjectLecturer>(e => e.LecturerId == id)).Select(
						e => e.SubjectId).ToList();

				var subjectsData =
					repositoriesContainer.RepositoryFor<SubjectLecturer>().GetAll(new Query<SubjectLecturer>(e => e.LecturerId == id).Include(e => e.Subject)).ToList();

				var news =
					repositoriesContainer.RepositoryFor<SubjectNews>().GetAll(
						new Query<SubjectNews>(e => subjects.Contains(e.SubjectId) && !e.Disabled)).ToList();

				foreach (var subjectNewse in news)
				{
					subjectNewse.Subject = new Subject
												{
													Name = subjectsData.FirstOrDefault(e => e.SubjectId == subjectNewse.SubjectId).Subject.Name
												};
				}

				return news;
			}
		}

		public IList<SubGroup> GetSubGroups(int subjectId, int groupId)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var subjectGroup =
					repositoriesContainer.SubjectRepository.GetBy(
						new Query<Subject>(e => e.Id == subjectId && e.SubjectGroups.Any(x => x.GroupId == groupId))
						.Include(e => e.SubjectGroups.Select(x => x.SubGroups.Select(c => c.SubjectStudents.Select(t => t.Student.ScheduleProtectionLabMarks))))
						.Include(e => e.SubjectGroups.Select(x => x.SubGroups.Select(c => c.SubjectStudents.Select(t => t.Student.ScheduleProtectionPracticalMarks))))
						.Include(e => e.SubjectGroups.Select(x => x.SubGroups.Select(c => c.SubjectStudents.Select(t => t.Student.StudentLabMarks))))
						.Include(e => e.SubjectGroups.Select(x => x.SubGroups.Select(c => c.SubjectStudents.Select(t => t.Student.User))))
						.Include(e => e.SubjectGroups.Select(x => x.SubGroups.Select(c => c.SubjectStudents.Select(t => t.Student.StudentPracticalMarks))))
						.Include(e => e.SubjectGroups.Select(x => x.SubGroups.Select(c => c.ScheduleProtectionLabs))));
				return subjectGroup.SubjectGroups.First(e => e.GroupId == groupId).SubGroups.ToList();
			}
		}

		public IList<SubGroup> GetSubGroupsV2(int subjectId, int groupId)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			var subjectGroup =
				repositoriesContainer.SubjectRepository.GetBy(
					new Query<Subject>(e => e.Id == subjectId && e.SubjectGroups.Any(x => x.GroupId == groupId))
						.Include(e => e.SubjectGroups.Select(x => x.SubGroups.Select(c => c.SubjectStudents))));
						
			return subjectGroup.SubjectGroups.First(e => e.GroupId == groupId).SubGroups.ToList();
		}

		public IList<SubGroup> GetSubGroupsV3(int subjectId, int groupId)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			var subjectGroup =
				repositoriesContainer.SubjectRepository
					.GetBy(new Query<Subject>(e => e.Id == subjectId && e.SubjectGroups.Any(x => x.GroupId == groupId))
						.Include(e => e.SubjectGroups.Select(x => x.SubGroups.Select(c => c.SubjectStudents.Select(t => t.Student.User)))));

			return subjectGroup.SubjectGroups.First(e => e.GroupId == groupId).SubGroups.ToList();
		}

		public IList<SubGroup> GetSubGroupsV2WithScheduleProtectionLabs(int subjectId, int groupId)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			var subjectGroup =
				repositoriesContainer.SubjectRepository.GetBy(
					new Query<Subject>(e => e.Id == subjectId && e.SubjectGroups.Any(x => x.GroupId == groupId))
						.Include(e => e.SubjectGroups.Select(x => x.SubGroups.Select(c => c.ScheduleProtectionLabs))));
						
			return subjectGroup.SubjectGroups.First(e => e.GroupId == groupId).SubGroups.ToList();
		}

		public void SaveSubGroup(int subjectId, int groupId, IList<int> firstInts, IList<int> secoInts, IList<int> thirdInts)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var subject = repositoriesContainer.SubjectRepository.GetBy(new Query<Subject>(e => e.Id == subjectId && e.SubjectGroups.Any(x => x.GroupId == groupId)).Include(e => e.SubjectGroups.Select(x => x.SubGroups)));
				var firstOrDefault = subject.SubjectGroups.FirstOrDefault(e => e.GroupId == groupId);
				if (firstOrDefault.SubGroups.Any())
				{
					repositoriesContainer.SubGroupRepository.SaveStudents(subjectId, firstOrDefault.Id, firstInts, secoInts, thirdInts);
				}
				else
				{
					repositoriesContainer.SubGroupRepository.CreateSubGroup(subjectId, firstOrDefault.Id, firstInts, secoInts, thirdInts);
				}
			}
		}

		public Lectures GetLectures(int id)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				return repositoriesContainer.LecturesRepository.GetBy(new Query<Lectures>(e => e.Id == id).Include(e => e.Subject));
			}
		}

		public Labs SaveLabs(Labs labs, IList<Attachment> attachments, int userId)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			if (!string.IsNullOrEmpty(labs.Attachments))
			{
				var deleteFiles =
					repositoriesContainer.AttachmentRepository.GetAll(
						new Query<Attachment>(e => e.PathName == labs.Attachments)).ToList().Where(e => attachments.All(x => x.Id != e.Id)).ToList();

				foreach (var attachment in deleteFiles)
				{
					FilesManagementService.DeleteFileAttachment(attachment);
				}
			}
			else
			{
				labs.Attachments = GetGuidFileName();
			}

			FilesManagementService.SaveFiles(attachments.Where(e => e.Id == 0), labs.Attachments);

			foreach (var attachment in attachments)
			{
				if (attachment.Id == 0)
				{
					attachment.PathName = labs.Attachments;
					repositoriesContainer.AttachmentRepository.Save(attachment);
				}
			}

			repositoriesContainer.LabsRepository.Save(labs);
			repositoriesContainer.ApplyChanges();

			if (labs.IsNew && labs.Subject.SubjectModules.All(m => m.Module.ModuleType != ModuleType.Practical) &&
			    labs.Subject.SubjectModules.Any(m => m.Module.ModuleType == ModuleType.Labs))
				ConceptManagementService.AttachFolderToLabSection(labs.Theme, userId, labs.SubjectId);

			return labs;
		}

		public UserLabFiles SaveUserLabFiles(UserLabFiles userLabFiles, IList<Attachment> attachments)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			if (!string.IsNullOrEmpty(userLabFiles.Attachments))
			{
				var deleteFiles =
					repositoriesContainer.AttachmentRepository.GetAll(
						new Query<Attachment>(e => e.PathName == userLabFiles.Attachments)).ToList().Where(e => attachments.All(x => x.Id != e.Id)).ToList();

				foreach (var attachment in deleteFiles)
				{
					FilesManagementService.DeleteFileAttachment(attachment);
				}
			}
			else
			{
				userLabFiles.Attachments = GetGuidFileName();
			}

			FilesManagementService.SaveFiles(attachments.Where(e => e.Id == 0), userLabFiles.Attachments);

			foreach (var attachment in attachments)
			{
				if (attachment.Id == 0)
				{
					attachment.PathName = userLabFiles.Attachments;
					repositoriesContainer.AttachmentRepository.Save(attachment);
				}
			}

			repositoriesContainer.RepositoryFor<UserLabFiles>().Save(userLabFiles);
			repositoriesContainer.ApplyChanges();

			return userLabFiles;
		}

		public Labs GetLabs(int id)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				return repositoriesContainer.LabsRepository.GetBy(new Query<Labs>(e => e.Id == id).Include(e => e.Subject));
			}
		}

		public Practical GetPractical(int id)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				return repositoriesContainer.PracticalRepository.GetBy(new Query<Practical>(e => e.Id == id).Include(e => e.Subject));
			}
		}

		public Practical SavePractical(Practical practical, IList<Attachment> attachments, int userId)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			if (!string.IsNullOrEmpty(practical.Attachments))
			{
				var deleteFiles =
					repositoriesContainer.AttachmentRepository.GetAll(
						new Query<Attachment>(e => e.PathName == practical.Attachments)).ToList().Where(e => attachments.All(x => x.Id != e.Id)).ToList();

				foreach (var attachment in deleteFiles)
				{
					FilesManagementService.DeleteFileAttachment(attachment);
				}
			}
			else
			{
				practical.Attachments = GetGuidFileName();
			}

			FilesManagementService.SaveFiles(attachments.Where(e => e.Id == 0), practical.Attachments);

			foreach (var attachment in attachments)
			{
				if (attachment.Id == 0)
				{
					attachment.PathName = practical.Attachments;
					repositoriesContainer.AttachmentRepository.Save(attachment);
				}
			}

			repositoriesContainer.PracticalRepository.Save(practical);
			repositoriesContainer.ApplyChanges();

			if (practical.IsNew && practical.Subject.SubjectModules.Any(m => m.Module.ModuleType == ModuleType.Practical))
				ConceptManagementService.AttachFolderToLabSection(practical.Theme, userId, practical.SubjectId);

			return practical;
		}

		public void SaveDateLectures(int subjectId, DateTime date)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			repositoriesContainer.RepositoryFor<LecturesScheduleVisiting>().Save(new LecturesScheduleVisiting
			{
				Date = date,
				SubjectId = subjectId
			});
			repositoriesContainer.ApplyChanges();
		}

		public List<LecturesScheduleVisiting> GetScheduleVisitings(Query<LecturesScheduleVisiting> query)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			return repositoriesContainer.RepositoryFor<LecturesScheduleVisiting>()
					.GetAll(query)
					.ToList();
		}

		public void SaveMarksCalendarData(List<LecturesVisitMark> lecturesVisitMarks)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			repositoriesContainer.RepositoryFor<LecturesVisitMark>().Save(lecturesVisitMarks);
			repositoriesContainer.ApplyChanges();
		}

		public void SaveScheduleProtectionLabsDate(int subGroupId, DateTime date)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			repositoriesContainer.RepositoryFor<ScheduleProtectionLabs>().Save(new ScheduleProtectionLabs
			{
				SuGroupId = subGroupId,
				Date = date,
				Id = 0
			});
			repositoriesContainer.ApplyChanges();
		}

		public void SaveScheduleProtectionPracticalDate(ScheduleProtectionPractical scheduleProtectionPractical)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			repositoriesContainer.RepositoryFor<ScheduleProtectionPractical>().Save(scheduleProtectionPractical);
			repositoriesContainer.ApplyChanges();
		}

		public SubGroup GetSubGroup(int subGroupId)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				return
					repositoriesContainer.RepositoryFor<SubGroup>().GetBy(new Query<SubGroup>(e => e.Id == subGroupId)
					.Include(e => e.SubjectStudents.Select(x => x.Student.ScheduleProtectionLabMarks)).Include(e => e.SubjectGroup.Group));
			}
		}

		public Group GetGroup(int groupId)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				return
					repositoriesContainer.RepositoryFor<Group>().GetBy(new Query<Group>(e => e.Id == groupId)
					.Include(e => e.Students.Select(x => x.ScheduleProtectionLabMarks)));
			}
		}

		public void SaveLabsVisitingData(ScheduleProtectionLabMark protectionLabMarks)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			repositoriesContainer.RepositoryFor<ScheduleProtectionLabMark>().Save(protectionLabMarks);
			repositoriesContainer.ApplyChanges();
		}

		public void SaveStudentLabsMark(StudentLabMark studentLabMark)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			repositoriesContainer.RepositoryFor<StudentLabMark>().Save(studentLabMark);
			repositoriesContainer.ApplyChanges();
		}

		public void SavePracticalVisitingData(List<ScheduleProtectionPracticalMark> protectionPracticalMarks)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			repositoriesContainer.RepositoryFor<ScheduleProtectionPracticalMark>().Save(protectionPracticalMarks);
			repositoriesContainer.ApplyChanges();
		}

		public void SavePracticalMarks(List<StudentPracticalMark> studentPracticalMarks)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			repositoriesContainer.RepositoryFor<StudentPracticalMark>().Save(studentPracticalMarks);
			repositoriesContainer.ApplyChanges();
		}

		public List<string> GetLecturesAttachments(int subjectId)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var model = new List<string>();
				model.AddRange(
					repositoriesContainer.LecturesRepository.GetAll(new Query<Lectures>(e => e.SubjectId == subjectId).Include(e => e.Attachments)).Select(e => e.Attachments).ToList());
				return model;
			}
		}

		public List<string> GetLabsAttachments(int subjectId)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var model = new List<string>();
				model.AddRange(
					repositoriesContainer.LabsRepository.GetAll(new Query<Labs>(e => e.SubjectId == subjectId).Include(e => e.Attachments)).Select(e => e.Attachments).ToList());
				return model;
			}
		}

		public List<string> GetPracticalsAttachments(int subjectId)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var model = new List<string>();
				model.AddRange(
					repositoriesContainer.PracticalRepository.GetAll(new Query<Practical>(e => e.SubjectId == subjectId).Include(e => e.Attachments)).Select(e => e.Attachments).ToList());
				return model;
			}
		}

		public void DeleteSubject(int id)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var model = repositoriesContainer.SubjectRepository.GetBy(new Query<Subject>(e => e.Id == id));
				model.IsArchive = true;
				repositoriesContainer.SubjectRepository.Save(model);
				repositoriesContainer.ApplyChanges();
			}
		}

		public List<Subject> GetSubjects()
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				return repositoriesContainer.SubjectRepository.GetAll().ToList();
			}
		}

		public List<UserLabFiles> GetUserLabFiles(int userId, int subjectId)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			if (userId == 0)
			{
				return repositoriesContainer.RepositoryFor<UserLabFiles>().GetAll(new Query<UserLabFiles>(e => e.SubjectId == subjectId)).ToList();
			}

			return repositoriesContainer.RepositoryFor<UserLabFiles>().GetAll(new Query<UserLabFiles>(e => e.UserId == userId && e.SubjectId == subjectId)).ToList();
		}

		public UserLabFiles GetUserLabFile(int id)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			return repositoriesContainer.RepositoryFor<UserLabFiles>().GetBy(new Query<UserLabFiles>(e => e.Id == id));
		}

		public UserLabFiles GetUserLabFile(string path)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			return repositoriesContainer.RepositoryFor<UserLabFiles>().GetBy(new Query<UserLabFiles>(e => e.Attachments == path));
		}

		public void DeleteUserLabFile(int id)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			var model = repositoriesContainer.RepositoryFor<UserLabFiles>()
				.GetBy(new Query<UserLabFiles>(e => e.Id == id));
			repositoriesContainer.RepositoryFor<UserLabFiles>().Delete(model);
			repositoriesContainer.ApplyChanges();
		}

		public void DeleteNonReceivedUserFiles(int groupId)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var studentsIds = repositoriesContainer.RepositoryFor<Student>()
					.GetAll(new Query<Student>(e => e.GroupId == groupId)).Select(x => x.User.Id).ToList();

				foreach (var studentId in studentsIds)
				{
					var model = repositoriesContainer.RepositoryFor<UserLabFiles>().
						GetAll(new Query<UserLabFiles>(e => e.UserId == studentId && !e.IsReceived));
					repositoriesContainer.RepositoryFor<UserLabFiles>().Delete(model);
				}

				repositoriesContainer.ApplyChanges();
			}
		}

		public Lectures SaveLectures(Lectures lectures, IList<Attachment> attachments, int userId)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();

			if (!string.IsNullOrEmpty(lectures.Attachments))
			{
				var deleteFiles =
					repositoriesContainer.AttachmentRepository.GetAll(
						new Query<Attachment>(e => e.PathName == lectures.Attachments)).ToList().Where(e => attachments.All(x => x.Id != e.Id)).ToList();

				foreach (var attachment in deleteFiles)
				{
					FilesManagementService.DeleteFileAttachment(attachment);
				}
			}
			else
			{
				lectures.Attachments = GetGuidFileName();
			}

			FilesManagementService.SaveFiles(attachments.Where(e => e.Id == 0), lectures.Attachments);

			foreach (var attachment in attachments)
			{
				if (attachment.Id == 0)
				{
					attachment.PathName = lectures.Attachments;
					repositoriesContainer.AttachmentRepository.Save(attachment);
				}
			}

			repositoriesContainer.LecturesRepository.Save(lectures);
			repositoriesContainer.ApplyChanges();

			if (lectures.IsNew && lectures.Subject.SubjectModules.Any(s => s.Module.ModuleType == ModuleType.Lectures))
				ConceptManagementService.AttachFolderToLectSection(lectures.Theme, userId, lectures.SubjectId);

			return lectures;
		}

		private string GetGuidFileName()
		{
			return $"P{Guid.NewGuid().ToString("N").ToUpper()}";
		}

		public bool IsSubjectName(string name, string id, int userId)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				return repositoriesContainer.SubjectRepository.IsSubjectName(name, id, userId);
			}
		}

		public bool IsSubjectShortName(string name, string id, int userId)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				return repositoriesContainer.SubjectRepository.IsSubjectShortName(name, id, userId);
			}
		}

		public void DisableNews(int subjectId, bool disable)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			repositoriesContainer.SubjectRepository.DisableNews(subjectId, disable);
		}

		public List<ProfileCalendarModel> GetLabEvents(int userId)
		{
			var model = new List<ProfileCalendarModel>();
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var subjects = repositoriesContainer.SubjectRepository.GetSubjects(lecturerId: userId);

				foreach (var subject in subjects)
				{
					var name = subject.ShortName;

					foreach (var group in subject.SubjectGroups)
					{
						foreach (var subGroup in group.SubGroups)
						{
							foreach (var scheduleProtectionLabse in subGroup.ScheduleProtectionLabs)
							{
								model.Add(new ProfileCalendarModel()
										 {
											 Start = scheduleProtectionLabse.Date.ToString("yyyy-MM-dd"),
											 Title = string.Format("{0} -  Лаб.работа (Гр. {1})", name, group.Group.Name),
											 SubjectId = subject.Id,
											 Color = subject.Color
										 });
							}
						}
					}
				}
			}

			return model;
		}

		public List<ProfileCalendarModel> GetGroupsLabEvents(int groupId, int userId)
		{
			var model = new List<ProfileCalendarModel>();
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var subjects = repositoriesContainer.SubjectRepository.GetSubjects(groupId: groupId);

				foreach (var subject in subjects)
				{
					var name = subject.ShortName;

					foreach (var group in subject.SubjectGroups)
					{
						foreach (var subGroup in group.SubGroups.Where(e => e.SubjectStudents.Any(x => x.StudentId == userId)))
						{
							foreach (var scheduleProtectionLabse in subGroup.ScheduleProtectionLabs)
							{
								model.Add(new ProfileCalendarModel()
											{
												Start = scheduleProtectionLabse.Date.ToString("yyyy-MM-dd"),
												Title = string.Format("{0} -  Лаб.работа", name),
												SubjectId = subject.Id,
												Color = subject.Color
											});
							}
						}
					}
				}
			}

			return model;
		}

		public void UpdateUserLabFile(int userFileId, bool isReceived)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			var userFile = repositoriesContainer.RepositoryFor<UserLabFiles>()
				.GetBy(new Query<UserLabFiles>(e => e.Id == userFileId));
			userFile.IsReceived = isReceived;
			repositoriesContainer.RepositoryFor<UserLabFiles>().Save(userFile);
		}

		public List<ProfileCalendarModel> GetLecturesEvents(int userId)
		{
			var model = new List<ProfileCalendarModel>();
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var subjects = repositoriesContainer.SubjectRepository.GetSubjects(lecturerId: userId);

				foreach (var subject in subjects)
				{
					var name = subject.ShortName;

					foreach (var lecturesScheduleVisiting in subject.LecturesScheduleVisitings)
					{
						model.Add(new ProfileCalendarModel()
						{
							Start = lecturesScheduleVisiting.Date.ToString("yyyy-MM-dd"),
							Title = string.Format("{0} -  Лекция", name),
							Color = subject.Color,
							SubjectId = subject.Id,
						});
					}
				}
			}

			return model;
		}

		public List<ProfileCalendarModel> GetLecturesEvents(int groupId, int userId)
		{
			var model = new List<ProfileCalendarModel>();
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var subjects = repositoriesContainer.SubjectRepository.GetSubjects(groupId);

				foreach (var subject in subjects)
				{
					var name = subject.ShortName;

					foreach (var lecturesScheduleVisiting in subject.LecturesScheduleVisitings)
					{
						model.Add(new ProfileCalendarModel()
						{
							Start = lecturesScheduleVisiting.Date.ToString("yyyy-MM-dd"),
							Title = string.Format("{0} -  Лекция", name),
							Color = subject.Color,
							SubjectId = subject.Id,
						});
					}
				}
			}

			return model;
		}

		public List<Subject> GetSubjectsByLector(int userId)
		{
			List<Subject> model;

			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				model = repositoriesContainer.SubjectRepository.GetSubjects(lecturerId: userId).Where(e => !e.IsArchive).ToList();
			}

			return model;
		}

		public List<Subject> GetSubjectsByLectorOwner(int userId)
		{
			using var repositoriesContainer = new LmPlatformRepositoriesContainer();
			var model = repositoriesContainer.RepositoryFor<SubjectLecturer>()
			.GetAll(new Query<SubjectLecturer>(e => e.LecturerId == userId && e.Owner == null)
			.Include(e => e.Subject.SubjectGroups.Select(x => x.SubjectStudents))
			.Include(e => e.Subject.LecturesScheduleVisitings)
			.Include(e => e.Subject.Labs)
			.Include(e => e.Subject.SubjectGroups.Select(x => x.SubGroups.Select(t => t.ScheduleProtectionLabs)))).Select(e => e.Subject).Where(e => !e.IsArchive).ToList();
			return model;
		}

		public List<Subject> GetSubjectsByStudent(int userId)
		{
			List<Subject> model;

			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var student = repositoriesContainer.StudentsRepository.GetStudent(userId);
				model = repositoriesContainer.SubjectRepository.GetSubjects(groupId: student.GroupId).Where(e => !e.IsArchive).ToList();
			}

			return model;
		}

		public int LabsCountByStudent(int userId)
		{
			var count = 0;

			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var student = repositoriesContainer.StudentsRepository.GetStudent(userId);
				var subjects = repositoriesContainer.SubjectRepository.GetSubjects(groupId: student.GroupId).Where(e => !e.IsArchive).ToList();

				count += subjects.Sum(subject => subject.Labs.Count);
			}

			return count;
		}

		public int LabsCountByLector(int userId)
		{
			var count = 0;

			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var subjects = repositoriesContainer.SubjectRepository.GetSubjects(lecturerId: userId).Where(e => !e.IsArchive).ToList();

				count += subjects.Sum(subject => subject.Labs.Count);
			}

			return count;
		}

		public int StudentAttendance(int userId)
		{
			int count = 0;

			//using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			//{
			//    var user =
			//        repositoriesContainer.StudentsRepository.GetBy(
			//            new Query<Student>(e => e.Id == userId).Include(
			//                e => e.ScheduleProtectionLabMarks.Select(x => x.ScheduleProtectionLab)));

			//    var hours = 0;

			//    foreach (var scheduleProtectionLabMark in user.ScheduleProtectionLabMarks)
			//    {
			//        if (!string.IsNullOrEmpty(scheduleProtectionLabMark.Mark))
			//        {
			//            //hours   
			//        }   
			//    }

			//    if (isDate)
			//    {
			//        var numberDates = dates.Count;
			//        dates.Sort((a, b) => a.CompareTo(b));
			//        var nowDate = DateTime.Now.Date;

			//        var countDone = 0;

			//        foreach (var dateTime in dates)
			//        {
			//            if (nowDate > dateTime)
			//            {
			//                countDone += 1;
			//            }
			//        }

			//        count = Math.Round(((decimal)countDone / numberDates) * 100, 0);
			//    }
			//}

			return count;
		}

		public decimal GetSubjectCompleting(int subjectId, string user, Student student)
		{
			decimal count = 0;

			if (user == "S")
			{
				using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
				{
					var subject = repositoriesContainer.SubjectRepository.GetBy(
						new Query<Subject>(e => e.Id == subjectId)
						.Include(x => x.Labs));

					var labs = subject.Labs.ToList();
					var labsCount = labs.Count;

					var marks = repositoriesContainer.StudentsRepository
													.GetBy(new Query<Student>(e => e.Id == student.Id).Include(e => e.StudentLabMarks)).StudentLabMarks.Where(e => labs.Any(x => x.Id == e.LabId)).ToList().Count;//student.StudentLabMarks.Count;

					count = marks == 0 ? 0 : Math.Round(((decimal)marks / labsCount) * 100, 0);
				}
			}
			else
			{
				using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
				{
					var subject = repositoriesContainer.SubjectRepository.GetBy(
						new Query<Subject>(e => e.Id == subjectId).Include(
							x => x.SubjectGroups.Select(t => t.SubGroups.Select(c => c.ScheduleProtectionLabs))));

					var dates = new List<DateTime>();

					var isDate = false;

					foreach (var subjectGroup in subject.SubjectGroups)
					{
						foreach (var subGroup in subjectGroup.SubGroups)
						{
							if (subGroup.ScheduleProtectionLabs != null)
							{
								foreach (var scheduleProtectionLabs in subGroup.ScheduleProtectionLabs)
								{
									isDate = true;
									dates.Add(scheduleProtectionLabs.Date);
								}
							}
						}
					}

					if (isDate)
					{
						var numberDates = dates.Count;
						dates.Sort((a, b) => a.CompareTo(b));
						var nowDate = DateTime.Now.Date;

						var countDone = 0;

						foreach (var dateTime in dates)
						{
							if (nowDate > dateTime)
							{
								countDone += 1;
							}
						}

						count = Math.Round(((decimal)countDone / numberDates) * 100, 0);
					}
				}
			}

			

			return count;
		}
	}
}