using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.FilesManagement;
using Application.Infrastructure.StudentManagement;
using LMPlatform.Data.Repositories;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;
using Application.Infrastructure.ConceptManagement;

namespace Application.Infrastructure.SubjectManagement
{
    public class SubjectManagementService : ISubjectManagementService
    {
        private readonly LazyDependency<IStudentManagementService> _studentManagementService = new LazyDependency<IStudentManagementService>();

        private readonly LazyDependency<IFilesManagementService> _filesManagementService =
            new LazyDependency<IFilesManagementService>();

        public IFilesManagementService FilesManagementService
        {
            get { return _filesManagementService.Value; }
        }

        public IStudentManagementService StudentManagementService
        {
            get
            {
                return _studentManagementService.Value;
            }
        }

        private readonly LazyDependency<IConceptManagementService> _conceptManagementService = new LazyDependency<IConceptManagementService>();


        public IConceptManagementService ConceptManagementService
        {
            get { return _conceptManagementService.Value; }
        }

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
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.SubjectRepository.GetSubjects(groupId: groupId).Where(e => !e.IsArchive).ToList();
            }
        }

        public Subject GetSubject(int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.SubjectRepository.GetBy(new Query<Subject>(e => e.Id == id)
                    .Include(e => e.SubjectModules.Select(x => x.Module))
                    .Include(e => e.SubjectNewses)
                    .Include(e => e.Lectures)
                    .Include(e => e.Labs)
					.Include(e => e.SubjectLecturers.Select(x => x.Lecturer.User))
                    .Include(e => e.Practicals)
                    .Include(e => e.LecturesScheduleVisitings)
                    .Include(e => e.SubjectGroups.Select(x => x.SubGroups.Select(v => v.ScheduleProtectionLabs))));
            }
        }

        public IPageableList<Subject> GetSubjectsLecturer(int lecturerId, string searchString = null, IPageInfo pageInfo = null, IEnumerable<ISortCriteria> sortCriterias = null)
        {
            var query = new PageableQuery<Subject>(pageInfo, e => e.SubjectLecturers.Any(x => x.LecturerId == lecturerId && !e.IsArchive));

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
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.SubjectRepository.Save(subject);

                repositoriesContainer.ApplyChanges();
            }

            return subject;
        }

        public SubjectNews SaveNews(SubjectNews news)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.SubjectRepository.SaveNews(news);

                repositoriesContainer.ApplyChanges();
            }

            return news;
        }

        public void DeleteNews(SubjectNews news)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.SubjectRepository.DeleteNews(news);
            }
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
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
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
        }

        public void DeletePracticalsVisitingDate(int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
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
        }

        public void DeleteLabsVisitingDate(int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
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
        }

        public void DeleteLabs(int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
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
        }

        public void DeletePracticals(int id)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
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

        public SubjectNews GetNews(int id, int subjecttId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return repositoriesContainer.SubjectRepository.GetBy(new Query<Subject>(e => e.Id == subjecttId).Include(e => e.SubjectNewses)).SubjectNewses.FirstOrDefault(e => e.Id == id);
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

        public void SaveSubGroup(int subjectId, int groupId, IList<int> firstInts, IList<int> secoInts)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                var subject = repositoriesContainer.SubjectRepository.GetBy(new Query<Subject>(e => e.Id == subjectId && e.SubjectGroups.Any(x => x.GroupId == groupId)).Include(e => e.SubjectGroups.Select(x => x.SubGroups)));
                var firstOrDefault = subject.SubjectGroups.FirstOrDefault(e => e.GroupId == groupId);
                if (firstOrDefault.SubGroups.Any())
                {
                    repositoriesContainer.SubGroupRepository.SaveStudents(subjectId, firstOrDefault.Id, firstInts, secoInts);
                }
                else
                {
                    repositoriesContainer.SubGroupRepository.CreateSubGroup(subjectId, firstOrDefault.Id, firstInts, secoInts);
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

        public Labs SaveLabs(Labs labs, IList<Attachment> attachments, Int32 userId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
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

                if (labs.IsNew && !labs.Subject.SubjectModules.Any(m => m.Module.ModuleType == ModuleType.Practical) &&
                    labs.Subject.SubjectModules.Any(m => m.Module.ModuleType == ModuleType.Labs))
                    ConceptManagementService.AttachFolderToLabSection(labs.Theme, userId, labs.SubjectId);
            }

            return labs;
        }

	    public UserLabFiles SaveUserLabFiles(UserLabFiles userLabFiles, IList<Attachment> attachments)
	    {
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
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
			}

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

        public Practical SavePractical(Practical practical, IList<Attachment> attachments, Int32 userId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
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
            }

            return practical;
        }

        public void SaveDateLectures(int subjectId, DateTime date)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.RepositoryFor<LecturesScheduleVisiting>().Save(new LecturesScheduleVisiting
                                                                                         {
                                                                                             Date = date,
                                                                                             SubjectId = subjectId
                                                                                         });
                repositoriesContainer.ApplyChanges();
            }
        }

        public List<LecturesScheduleVisiting> GetScheduleVisitings(Query<LecturesScheduleVisiting> query)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                return
                    repositoriesContainer.RepositoryFor<LecturesScheduleVisiting>()
                        .GetAll(query.Include(e => e.LecturesVisitMarks.Select(x => x.Student.User)))
                        .ToList();
            }
        }

        public void SaveMarksCalendarData(List<LecturesVisitMark> lecturesVisitMarks)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.RepositoryFor<LecturesVisitMark>().Save(lecturesVisitMarks);
                repositoriesContainer.ApplyChanges();
            }
        }

        public void SaveScheduleProtectionLabsDate(int subGroupId, DateTime date)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.RepositoryFor<ScheduleProtectionLabs>().Save(new ScheduleProtectionLabs
                                                                         {
                                                                            SuGroupId    = subGroupId,
                                                                            Date = date,
                                                                            Id = 0
                                                                         });
                repositoriesContainer.ApplyChanges();
            }
        }

        public void SaveScheduleProtectionPracticalDate(ScheduleProtectionPractical scheduleProtectionPractical)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.RepositoryFor<ScheduleProtectionPractical>().Save(scheduleProtectionPractical);
                repositoriesContainer.ApplyChanges();
            }
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

        public void SaveLabsVisitingData(List<ScheduleProtectionLabMark> protectionLabMarks)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.RepositoryFor<ScheduleProtectionLabMark>().Save(protectionLabMarks);
                repositoriesContainer.ApplyChanges();
            }
        }

        public void SaveStudentLabsMark(List<StudentLabMark> studentLabMark)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.RepositoryFor<StudentLabMark>().Save(studentLabMark);
                repositoriesContainer.ApplyChanges();
            }
        }

        public void SavePracticalVisitingData(List<ScheduleProtectionPracticalMark> protectionPracticalMarks)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.RepositoryFor<ScheduleProtectionPracticalMark>().Save(protectionPracticalMarks);
                repositoriesContainer.ApplyChanges();
            }
        }

        public void SavePracticalMarks(List<StudentPracticalMark> studentPracticalMarks)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
                repositoriesContainer.RepositoryFor<StudentPracticalMark>().Save(studentPracticalMarks);
                repositoriesContainer.ApplyChanges();
            }
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
		    using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
		    {
		        if (userId == 0)
		        {
                    return repositoriesContainer.RepositoryFor<UserLabFiles>().GetAll(new Query<UserLabFiles>(e => e.SubjectId == subjectId)).ToList();
		        }

				return repositoriesContainer.RepositoryFor<UserLabFiles>().GetAll(new Query<UserLabFiles>(e => e.UserId == userId && e.SubjectId == subjectId)).ToList();
		    }
	    }

	    public UserLabFiles GetUserLabFile(int id)
	    {
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				return repositoriesContainer.RepositoryFor<UserLabFiles>().GetBy(new Query<UserLabFiles>(e => e.Id == id));
			}
	    }

	    public void DeleteUserLabFile(int id)
	    {
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				var model = repositoriesContainer.RepositoryFor<UserLabFiles>().GetBy(new Query<UserLabFiles>(e => e.Id == id));

				repositoriesContainer.RepositoryFor<UserLabFiles>().Delete(model);
				repositoriesContainer.ApplyChanges();
			}
	    }

	    public Lectures SaveLectures(Lectures lectures, IList<Attachment> attachments, Int32 userId)
        {
            using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
            {
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

                if (lectures.IsNew && lectures.Subject.SubjectModules.Any(s=>s.Module.ModuleType==ModuleType.Lectures))
                    ConceptManagementService.AttachFolderToLectSection(lectures.Theme, userId, lectures.SubjectId);
            }

            return lectures;
        }

        private string GetGuidFileName()
        {
            return string.Format("P{0}", Guid.NewGuid().ToString("N").ToUpper());
        }

		public bool IsSubjectName(string name, string id)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				return repositoriesContainer.SubjectRepository.IsSubjectName(name, id);
			}
		}

		public bool IsSubjectShortName(string name, string id)
		{
			using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
			{
				return repositoriesContainer.SubjectRepository.IsSubjectShortName(name, id);
			}
		}
    }
}