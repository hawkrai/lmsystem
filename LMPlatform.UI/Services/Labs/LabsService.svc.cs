using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Caching;
using Application.Core;
using Application.Infrastructure.FilesManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;
using LMPlatform.UI.Services.Modules;
using LMPlatform.UI.Services.Modules.Labs;
using LMPlatform.PlagiarismNet.Controllers;
using Newtonsoft.Json;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.KnowledgeTestsManagement;
using System.Globalization;
using WebMatrix.WebData;
using System.Configuration;
using Application.Core.Data;
using Application.Infrastructure.StudentManagement;
using LMPlatform.UI.Services.Modules.CoreModels;

namespace LMPlatform.UI.Services.Labs
{
    public class LabsService : ILabsService
    {
		private readonly LazyDependency<ITestPassingService> testPassingService = new LazyDependency<ITestPassingService>();

		public string PlagiarismTempPath => ConfigurationManager.AppSettings["PlagiarismTempPath"];

		public string FileUploadPath => ConfigurationManager.AppSettings["FileUploadPath"];

		public ITestPassingService TestPassingService => testPassingService.Value;

		private readonly LazyDependency<IGroupManagementService> groupManagementService = new LazyDependency<IGroupManagementService>();

		public IGroupManagementService GroupManagementService => groupManagementService.Value;

		private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();

        public ISubjectManagementService SubjectManagementService => subjectManagementService.Value;

        private readonly LazyDependency<IFilesManagementService> filesManagementService = new LazyDependency<IFilesManagementService>();

		public IFilesManagementService FilesManagementService => filesManagementService.Value;

		private readonly LazyDependency<ITestsManagementService> testsManagementService = new LazyDependency<ITestsManagementService>();

        public ITestsManagementService TestsManagementService => testsManagementService.Value;

        private readonly LazyDependency<IStudentManagementService> studentManagementService = new LazyDependency<IStudentManagementService>();

		public IStudentManagementService StudentManagementService => studentManagementService.Value;

		public LabsResult GetLabs(string subjectId)
        {
            try
            {
				var query = new Query<Subject>(s => s.Id == int.Parse(subjectId))
					.Include(s => s.Labs);
                var model = SubjectManagementService.GetSubject(query).Labs
	                .OrderBy(e => e.Order)
	                .Select(e => new LabsViewData(e)).ToList();
                return new LabsResult
                {
                    Labs = model,
                    Message = "Лабораторные работы успешно загружены",
                    Code = "200"
                };
            }
            catch
            {
                return new LabsResult
                {
                    Message = "Произошла ошибка при получении лабораторых работ",
                    Code = "500"
                };
            }
        }

		public StudentsMarksResult GetMarks(int subjectId, int groupId)
	    {
			try
			{
				var group = this.GroupManagementService.GetGroups(
					new Query<Group>(e => e.SubjectGroups.Any(x => x.SubjectId == subjectId && x.GroupId == groupId))
						.Include(e => e.Students.Select(x => x.StudentLabMarks))
						.Include(e => e.Students.Select(x => x.User))).ToList()[0];

				var labsData = this.SubjectManagementService.GetSubject(subjectId).Labs.OrderBy(e => e.Order).ToList();
				
				var students = new List<StudentsViewData>();

                var controlTests = TestsManagementService.GetTestsForSubject(subjectId, lite: true).Where(x => !x.ForSelfStudy);

                foreach (var student in group.Students)
				{
					students.Add(new StudentsViewData(this.TestPassingService.GetStidentResults(subjectId, student.Id).Where(x => controlTests.Any(y => y.Id == x.TestId)).ToList(), student, labs: labsData));
				}

				return new StudentsMarksResult
				{
					Students = students.Select(e => new StudentMark
					{
						FullName = e.FullName,
						StudentId = e.StudentId,
						LabsMarkTotal = e.LabsMarkTotal,
						TestMark = e.TestMark,
						Marks = e.StudentLabMarks
					}).ToList(),
					Message = "",
					Code = "200"
				};
			}
			catch
			{
				return new StudentsMarksResult
				{
					Message = "Произошла ошибка при получении результатов студентов",
					Code = "500"
				};
			}
	    }

	    public ResultViewData Save(int subjectId, int id, string theme, int duration, int order, string shortName, string pathFile, string attachments)
        {
            try
            {
                var attachmentsModel = JsonConvert.DeserializeObject<List<Attachment>>(attachments).ToList();
                SubjectManagementService.SaveLabs(new Models.Labs
                {
                    SubjectId = subjectId,
                    Duration = duration,
                    Theme = theme,
                    Order = order,
                    ShortName = shortName,
                    Attachments = pathFile,
                    Id = id
                }, attachmentsModel, WebSecurity.CurrentUserId);
                
                return new ResultViewData
                {
                    Message = "Лабораторная работа успешно сохранена",
                    Code = "200"
                };
            }
            catch
            {
                return new ResultViewData
                {
                    Message = "Произошла ошибка при сохранении лабораторной работы",
                    Code = "500"
                };
            }
        }

        public ResultViewData Delete(int id, int subjectId)
        {
            try
            {
                SubjectManagementService.DeleteLabs(id);
                return new ResultViewData
                {
                    Message = "Лабораторная работа успешно удалена",
                    Code = "200"
                };
            }
            catch (Exception e)
            {
                return new ResultViewData
                {
                    Message = "Произошла ошибка при удалении лабораторной работы" + e.Message,
                    Code = "500"
                };
            }
        }

        public ResultViewData SaveScheduleProtectionDate(int subGroupId, string date)
        {
            try
            {
				SubjectManagementService.SaveScheduleProtectionLabsDate(subGroupId, DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                return new ResultViewData
                {
                    Message = "Дата успешно добавлена",
                    Code = "200"
                };
            }
            catch
            {
                return new ResultViewData
                {
                    Message = "Произошла ошибка при добавлении даты",
                    Code = "500"
                };
            }
        }

		public ResultViewData SaveLabsVisitingDataSingle(int dateId, string mark, string comment, int studentsId, int id)
		{
			try
			{
				SubjectManagementService.SaveLabsVisitingData(new ScheduleProtectionLabMark(id, studentsId, comment, mark, dateId));

				return new ResultViewData
				{
					Message = "Данные успешно добавлены",
					Code = "200"
				};
			}
			catch
			{
				return new ResultViewData
				{
					Message = "Произошла ошибка при добавлении данных",
					Code = "500"
				};
			}
		}

        public ResultViewData SaveLabsVisitingData(int dateId, List<string> marks, List<string> comments, List<int> studentsId, List<int> Id, List<StudentsViewData> students)
        {
            try
            {
                var count = studentsId.Count;

                for (var i = 0; i < count; i++)
                {
                    var currentMark = marks[i];
                    var currentComment = comments[i];
                    var currentStudentId = studentsId[i];
                    var currentId = Id[i];

                    foreach (var student in students)
                    {
                        if (student.StudentId == currentStudentId)
                        {
                            foreach (var labVisiting in student.LabVisitingMark)
                            {
                                if (labVisiting.ScheduleProtectionLabId == dateId)
                                {
                                    SubjectManagementService.SaveLabsVisitingData(new ScheduleProtectionLabMark(currentId, currentStudentId, currentComment, currentMark, dateId));
                                }
                            }
                        }

                    }
                }

                return new ResultViewData
                {
                    Message = "Данные успешно добавлены",
                    Code = "200"
                };
            }
            catch
            {
                return new ResultViewData
                {
                    Message = "Произошла ошибка при добавлении данных",
                    Code = "500"
                };
            }
        }

        public ResultViewData SaveStudentLabsMark(int studentId, int labId, string mark, string comment, string date, int id, List<StudentsViewData> students)
        {
            try
            {
				SubjectManagementService.SaveStudentLabsMark(new StudentLabMark(labId, studentId, mark, comment, date, id));

                return new ResultViewData
                {
                    Message = "Данные успешно добавлены",
                    Code = "200"
                };
            }
            catch
            {
                return new ResultViewData
                {
                    Message = "Произошла ошибка при добавлении данных",
                    Code = "500"
                };
            }
        }

		public ResultViewData DeleteVisitingDate(int id)
        {
            try
            {
                SubjectManagementService.DeleteLabsVisitingDate(id);

                return new ResultViewData
                {
                    Message = "Дата успешно удалена",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new ResultViewData
                {
                    Message = "Произошла ошибка при удалении даты",
                    Code = "500"
                };
            }
        }

        public UserLabFilesResult GetFilesLab(int userId, int subjectId, bool isCoursPrj = false)
        {
            try
            {
	            var model = new List<UserlabFilesViewData>();
	            var data = SubjectManagementService.GetUserLabFiles(userId, subjectId);
	            model = data.Select(e => new UserlabFilesViewData
	            {
		            Comments = e.Comments,
					Id = e.Id,
					PathFile = e.Attachments,
					IsReceived = e.IsReceived,
	                IsReturned = e.IsReturned,
	                IsCoursProject = e.IsCoursProject,
                    Date = e.Date != null ? e.Date.Value.ToString("dd.MM.yyyy HH:mm") : string.Empty,
		            Attachments = FilesManagementService.GetAttachments(e.Attachments).ToList()
	            }).Where(x => x.IsCoursProject == isCoursPrj).ToList();
                return new UserLabFilesResult
                {
					UserLabFiles = model,
                    Message = "Данные получены",
                    Code = "200"
                };
            }
            catch
            {
                return new UserLabFilesResult
                {
                    Message = "Произошла ошибка при получении данных",
                    Code = "500"
                };
            }
        }

		public ResultViewData SendFile(int subjectId, int userId, int id, string comments, string pathFile, string attachments, bool isCp = false, bool isRet = false)
		{
			try
			{
				var attachmentsModel = JsonConvert.DeserializeObject<List<Attachment>>(attachments).ToList();

				SubjectManagementService.SaveUserLabFiles(new UserLabFiles
				{
					SubjectId = subjectId,
                    Date = DateTime.Now,
					UserId = userId,
					Comments = comments,
					Attachments = pathFile,
					Id = id,
				    IsCoursProject = isCp,
				    IsReceived = false,
				    IsReturned = isRet
                }, attachmentsModel);

				return new ResultViewData
				{
					Message = "Файл(ы) успешно отправлен(ы)",
					Code = "200"
				};
			}
			catch
			{
				return new ResultViewData
				{
					Message = "Произошла ошибка",
					Code = "500"
				};
			}
		}

		public ResultViewData DeleteUserFile(int id)
		{
			try
			{
				SubjectManagementService.DeleteUserLabFile(id);
				return new ResultViewData
				{
					Message = "Работа удалена",
					Code = "200"
				};
			}
			catch (Exception e)
			{
				return new ResultViewData
				{
					Message = "Произошла ошибка при удалении работы - " + e.Message,
					Code = "500"
				};
			}
		}

		public StudentsMarksResult GetMarksV2(int subjectId, int groupId)
		{
			try
			{
				var group = this.GroupManagementService.GetGroups(new Query<Group>(e => e.SubjectGroups.Any(x => x.SubjectId == subjectId && x.GroupId == groupId))
					.Include(e => e.Students.Select(x => x.StudentLabMarks))
					.Include(e => e.Students.Select(x => x.ScheduleProtectionLabMarks))
					.Include(e => e.Students.Select(x => x.User))).ToList()[0];

				IList<SubGroup> subGroups = this.SubjectManagementService.GetSubGroupsV2(subjectId, group.Id);

				IList<SubGroup> subGroupsWithSchedule = this.SubjectManagementService.GetSubGroupsV2WithScheduleProtectionLabs(subjectId, group.Id).ToList();

				var labsData = this.SubjectManagementService.GetSubject(subjectId).Labs.OrderBy(e => e.Order).ToList();

				var students = new List<StudentsViewData>();

                var controlTests = TestsManagementService.GetTestsForSubject(subjectId).Where(x => !x.ForSelfStudy && !x.BeforeEUMK && !x.ForEUMK && !x.ForNN);

                foreach (var student in group.Students.Where(e => e.Confirmed == null || e.Confirmed.Value).OrderBy(e => e.LastName))
				{
					var scheduleProtectionLabs = subGroups.Any()
													 ? subGroups.FirstOrDefault(x => x.Name == "first").SubjectStudents.Any(x => x.StudentId == student.Id)
														   ? subGroupsWithSchedule.FirstOrDefault(x => x.Name == "first").ScheduleProtectionLabs.OrderBy(
								                               x => x.Date).ToList()
														   : subGroups.FirstOrDefault(x => x.Name == "second").SubjectStudents.Any(x => x.StudentId == student.Id)
																 ? subGroupsWithSchedule.FirstOrDefault(x => x.Name == "second").ScheduleProtectionLabs.OrderBy(
									                                 x => x.Date).ToList()
																 : subGroups.FirstOrDefault(x => x.Name == "third").SubjectStudents.Any(x => x.StudentId == student.Id)
																	? subGroupsWithSchedule.FirstOrDefault(x => x.Name == "third").ScheduleProtectionLabs.OrderBy(
																		x => x.Date).ToList()
																		: new List<ScheduleProtectionLabs>()
						                             : new List<ScheduleProtectionLabs>();
					students.Add(new StudentsViewData(this.TestPassingService.GetStidentResults(subjectId, student.Id).Where(x => controlTests.Any(y => y.Id == x.TestId)).ToList(), student, scheduleProtectionLabs: scheduleProtectionLabs, labs: labsData));
				}

				return new StudentsMarksResult
				{
					Students = students.Select(e => new StudentMark
					{
						FullName = e.FullName,
						Login = e.Login,
						SubGroup = subGroups
							.FirstOrDefault(x => x.Name == "first").SubjectStudents
							.Any(x => x.StudentId == e.StudentId) ? 1 : subGroups
							.FirstOrDefault(x => x.Name == "second").SubjectStudents
							.Any(x => x.StudentId == e.StudentId) ? 2 : subGroups
							.FirstOrDefault(x => x.Name == "third").SubjectStudents
							.Any(x => x.StudentId == e.StudentId) ? 3 : 4,
						StudentId = e.StudentId,
						LabsMarkTotal = e.LabsMarkTotal,
						TestMark = e.TestMark,
						LabVisitingMark = e.LabVisitingMark,
						Marks = e.StudentLabMarks,
					}).ToList(),
					Message = "",
					Code = "200"
				};
			}
			catch
			{
				return new StudentsMarksResult
				{
					Message = "Произошла ошибка при получении результатов студентов",
					Code = "500"
				};
			}
		}

		public StudentsMarksResult GetFilesV2(int subjectId, int groupId, bool isCp)
		{
			try
			{
				var group = this.GroupManagementService.GetGroups(new Query<Group>(e => e.SubjectGroups.Any(x => x.SubjectId == subjectId && x.GroupId == groupId))
					.Include(e => e.Students.Select(x => x.User))).ToList()[0];
				IList<SubGroup> subGroups = this.SubjectManagementService.GetSubGroupsV2(subjectId, group.Id);
				var students = new List<StudentMark>();

				foreach (var student in group.Students.Where(e => e.Confirmed == null || e.Confirmed.Value).OrderBy(e => e.LastName))
				{
					var files =
						SubjectManagementService.GetUserLabFiles(student.Id, subjectId).Select(
							t =>
							new UserlabFilesViewData
							{
								Comments = t.Comments,
								Date = t.Date != null ? t.Date.Value.ToString("dd.MM.yyyy HH:mm") : string.Empty,
								Id = t.Id,
								PathFile = t.Attachments,
								IsReceived = t.IsReceived,
							    IsReturned = t.IsReturned,
							    IsCoursProject = t.IsCoursProject,
                                Attachments = FilesManagementService.GetAttachments(t.Attachments).ToList()
							}).Where(x => x.IsCoursProject == isCp).ToList();
					students.Add(new StudentMark
					{
						StudentId = student.Id,
						FullName = student.FullName,
						SubGroup = subGroups.FirstOrDefault(x => x.Name == "first").SubjectStudents.Any(x => x.StudentId == student.Id) ? 1 : subGroups.FirstOrDefault(x => x.Name == "second").SubjectStudents.Any(x => x.StudentId == student.Id) ? 2 : subGroups.FirstOrDefault(x => x.Name == "third").SubjectStudents.Any(x => x.StudentId == student.Id) ? 3 : 4,
						FileLabs = files
					});
				}

				return new StudentsMarksResult
				{
					Students = students,
					Message = "",
					Code = "200"
				};
			}
			catch
			{
				return new StudentsMarksResult
				{
					Message = "Произошла ошибка при получении результатов студентов",
					Code = "500"
				};
			}
		}

		public LabsResult GetLabsV2(int subjectId, int groupId)
		{
			try
			{
				var labs = this.SubjectManagementService.GetLabsV2(subjectId).OrderBy(e => e.Order);

				var subGroups = this.SubjectManagementService.GetSubGroupsV2WithScheduleProtectionLabs(subjectId, groupId);

				var labsSubOne = labs.Select(e => new LabsViewData
				{
					Theme = e.Theme,
					Order = e.Order,
					Duration = e.Duration,
					ShortName = e.ShortName,
					LabId = e.Id,
					SubjectId = e.SubjectId,
					SubGroup = 1,
					ScheduleProtectionLabsRecomend = subGroups.Any() ? subGroups
						.FirstOrDefault(x => x.Name == "first").ScheduleProtectionLabs
						.OrderBy(x => x.Date)
						.Select(x => new ScheduleProtectionLab
						{
							ScheduleProtectionId = x.Id,
							Mark = string.Empty
						}).ToList() : new List<ScheduleProtectionLab>()
				}).ToList();


				var durationCount = 0;

				foreach (var lab in labsSubOne)
				{
					var mark = 10;
					durationCount += lab.Duration / 2;
					for (int i = 0; i < lab.ScheduleProtectionLabsRecomend.Count; i++)
					{
						if (i + 1 > durationCount - (lab.Duration / 2))
						{
							lab.ScheduleProtectionLabsRecomend[i].Mark = mark.ToString(CultureInfo.InvariantCulture);

							if (i + 1 >= durationCount)
							{
								if (mark != 1)
								{
									mark -= 1;
								}
							}
						}
					}
				}

				var labsSubTwo = labs.Select(e => new LabsViewData
				{
					Theme = e.Theme,
					Order = e.Order,
					Duration = e.Duration,
					ShortName = e.ShortName,
					LabId = e.Id,
					SubjectId = e.SubjectId,
					SubGroup = 2,
					ScheduleProtectionLabsRecomend = subGroups.Any() ? subGroups
						.FirstOrDefault(x => x.Name == "second").ScheduleProtectionLabs
						.OrderBy(x => x.Date)
						.Select(x => new ScheduleProtectionLab
						{
							ScheduleProtectionId = x.Id,
							Mark = string.Empty
						}).ToList() : new List<ScheduleProtectionLab>()
				}).ToList();

				durationCount = 0;
				foreach (var lab in labsSubTwo)
				{
					var mark = 10;
					durationCount += lab.Duration / 2;
					for (int i = 0; i < lab.ScheduleProtectionLabsRecomend.Count; i++)
					{
						if (i + 1 > durationCount - (lab.Duration / 2))
						{
							lab.ScheduleProtectionLabsRecomend[i].Mark = mark.ToString(CultureInfo.InvariantCulture);

							if (i + 1 >= durationCount)
							{
								if (mark != 1)
								{
									mark -= 1;
								}
							}
						}
					}
				}

				var labsSubThird = labs.Select(e => new LabsViewData
				{
					Theme = e.Theme,
					Order = e.Order,
					Duration = e.Duration,
					ShortName = e.ShortName,
					LabId = e.Id,
					SubjectId = e.SubjectId,
					SubGroup = 3,
					ScheduleProtectionLabsRecomend = subGroups.Any() ? subGroups
						.FirstOrDefault(x => x.Name == "third").ScheduleProtectionLabs
						.OrderBy(x => x.Date)
						.Select(x => new ScheduleProtectionLab
						{
							ScheduleProtectionId = x.Id,
							Mark = string.Empty
						}).ToList() : new List<ScheduleProtectionLab>()
				}).ToList();

				durationCount = 0;
				foreach (var lab in labsSubThird)
				{
					var mark = 10;
					durationCount += lab.Duration / 2;
					for (int i = 0; i < lab.ScheduleProtectionLabsRecomend.Count; i++)
					{
						if (i + 1 > durationCount - (lab.Duration / 2))
						{
							lab.ScheduleProtectionLabsRecomend[i].Mark = mark.ToString(CultureInfo.InvariantCulture);

							if (i + 1 >= durationCount)
							{
								if (mark != 1)
								{
									mark -= 1;
								}
							}
						}
					}
				}

				labsSubOne.AddRange(labsSubTwo);
				labsSubOne.AddRange(labsSubThird);

				var scheduleProtectionLabsOne =
					subGroups.FirstOrDefault() != null ? subGroups
						.FirstOrDefault(e => e.Name == "first").ScheduleProtectionLabs
						.OrderBy(e => e.Date)
						.Select(
					e => new ScheduleProtectionLabsViewData(e)).ToList() : new List<ScheduleProtectionLabsViewData>();

				scheduleProtectionLabsOne.ForEach(e => e.SubGroup = 1);

				var scheduleProtectionLabsTwo =
					subGroups.LastOrDefault() != null ? subGroups
						.FirstOrDefault(e => e.Name == "second").ScheduleProtectionLabs
						.OrderBy(e => e.Date)
						.Select(
					e => new ScheduleProtectionLabsViewData(e)).ToList() : new List<ScheduleProtectionLabsViewData>();

				scheduleProtectionLabsTwo.ForEach(e => e.SubGroup = 2);

				var scheduleProtectionLabsThird =
					subGroups.LastOrDefault() != null ? subGroups
						.FirstOrDefault(e => e.Name == "third").ScheduleProtectionLabs
						.OrderBy(e => e.Date)
						.Select(
					e => new ScheduleProtectionLabsViewData(e)).ToList() : new List<ScheduleProtectionLabsViewData>();

				scheduleProtectionLabsThird.ForEach(e => e.SubGroup = 3);

				scheduleProtectionLabsOne.AddRange(scheduleProtectionLabsTwo);

				scheduleProtectionLabsOne.AddRange(scheduleProtectionLabsThird);

				return new LabsResult
				{
					Labs = labsSubOne,
					ScheduleProtectionLabs = scheduleProtectionLabsOne,
					Message = "Лабораторные работы успешно загружены",
					Code = "200"
				};
			}
			catch
			{
				return new LabsResult
				{
					Message = "Произошла ошибка при получении лабораторых работ",
					Code = "500"
				};
			}
		}

		public ResultViewData ReceivedLabFile(int userFileId)
		{
			try
			{
				this.SubjectManagementService.UpdateUserLabFile(userFileId, true);
				return new ResultViewData
				{
					Message = "Файл(ы) перемещен(ы) в архив",
					Code = "200"
				};
			}
			catch
			{
				return new ResultViewData
				{
					Message = "Произошла ошибка переноса файла в архив",
					Code = "500"
				};
			}
		}

		public ResultViewData CancelReceivedLabFile(int userFileId)
		{
			try
			{
				this.SubjectManagementService.UpdateUserLabFile(userFileId, false);
				return new ResultViewData
				{
					Message = "Файл(ы) перемещен(ы) из архива",
					Code = "200"
				};
			}
			catch
			{
				return new ResultViewData
				{
					Message = "Произошла ошибка переноса файла из архива",
					Code = "500"
				};
			}
		}

		public ResultPSubjectViewData CheckPlagiarismSubjects(string subjectId, int type, int threshold, bool isCp = false) 
		{
			try
			{
				ClearCache();

				var path = Guid.NewGuid().ToString("N");

				var subjectName = this.SubjectManagementService.GetSubject(int.Parse(subjectId)).ShortName;

				Directory.CreateDirectory(this.PlagiarismTempPath + path);
								
				var usersFiles = this.SubjectManagementService.GetUserLabFiles(0, int.Parse(subjectId)).Where(e => e.IsReceived && e.IsCoursProject == isCp);

				var filesPaths = usersFiles.Select(e => e.Attachments);

				var key = 0;

				if (filesPaths.Count() == 0)
				{
					return new ResultPSubjectViewData
					{
						Message = "Отсутствуют принятые работы для проверки на плагиат",
						Code = "200"
					};
				}

				foreach (var filesPath in filesPaths)
				{
					if (Directory.Exists(this.FileUploadPath + filesPath))
					{
						foreach (var srcPath in Directory.GetFiles(this.FileUploadPath + filesPath))
						{
							File.Copy(srcPath,
								srcPath.Replace(this.FileUploadPath + filesPath, this.PlagiarismTempPath + path), true);
						}
					}
					key += filesPath.GetHashCode();
				}

				var plagiarismController = new PlagiarismController();
				var result = plagiarismController.CheckByDirectory(new []{ PlagiarismTempPath + path }.ToList(), threshold, 10, type);

				var data = new ResultPlagSubjectClu
				{
					clusters = new ResultPlagSubject[result.Count]
				};

				for (int i = 0; i < result.Count; ++i)
				{
					data.clusters[i] = new ResultPlagSubject();

					var correctDocs = new List<ResultPlag>();

					foreach (var doc in result[i].Docs)
					{
						var resultS = new ResultPlag();
						
						var fileName = Path.GetFileName(doc);

						resultS.DocFileName = fileName;

						var name = this.FilesManagementService.GetFileDisplayName(fileName);
						
						resultS.subjectName = subjectName;
						
						resultS.doc = name;
						
						var pathName = this.FilesManagementService.GetPathName(fileName);

						resultS.DocPathName = pathName;

						var userFileT = this.SubjectManagementService.GetUserLabFile(pathName);

						var userId = userFileT.UserId;

						var user = this.StudentManagementService.GetStudent(userId);

						resultS.author = user.FullName;

						resultS.groupName = user.Group.Name;

						correctDocs.Add(resultS);
					}
					data.clusters[i].correctDocs = correctDocs.OrderBy(x => x.groupName).ThenBy(x => x.author).ToList();
				}
				HttpContext.Current.Session.Add(key.ToString(), data.clusters.ToList());
				
				return new ResultPSubjectViewData
				{
					DataD = data.clusters.ToList(),
					Message = "Проверка успешно завершена",
					Code = "200"
				};
			}
			catch (Exception e)
			{
				return new ResultPSubjectViewData
				{
					Message = e.Message + "   " + e,
					Code = "500"
				};
			}
		}

		public ResultViewData CheckPlagiarism(int userFileId, int subjectId, bool isCp = false)
		{
			try
			{
				ClearCache();

				var path = Guid.NewGuid().ToString("N");

				var subjectName = this.SubjectManagementService.GetSubject(new Query<Subject>(e => e.Id == subjectId)).ShortName;

				var key = 0;

				Directory.CreateDirectory(this.PlagiarismTempPath + path);

				var userFile = this.SubjectManagementService.GetUserLabFile(userFileId);

				var usersFiles = this.SubjectManagementService.GetUserLabFiles(0, subjectId)
					.Where(e => e.IsReceived && e.Id != userFile.Id && e.IsCoursProject == isCp);

				var filesPaths = usersFiles.Select(e => e.Attachments);

				if (filesPaths.Count() == 0) 
				{
					return new ResultViewData
					{
						Message = "Отсутствуют принятые работы для проверки на плагиат",
						Code = "200"
					};
				}

				foreach (var filesPath in filesPaths)
				{
					foreach (var srcPath in Directory.GetFiles(this.FileUploadPath + filesPath))
					{
						File.Copy(srcPath, srcPath.Replace(this.FileUploadPath + filesPath, this.PlagiarismTempPath + path), true);
					}

					key += filesPath.GetHashCode();
				}

				string firstFileName =
					Directory.GetFiles(FileUploadPath + userFile.Attachments)
					.Select(fi => fi)
					.FirstOrDefault();

				var plagiarismController = new PlagiarismController();
				var result = plagiarismController.CheckBySingleDoc(firstFileName, new[] { PlagiarismTempPath + path }.ToList(), 10, 10);

				var data = new List<ResultPlag>();

				foreach (var res in result)
				{
					var resPlag = new ResultPlag();

					var fileName = Path.GetFileName(res.Doc);

					resPlag.DocFileName = fileName;

					var name = FilesManagementService.GetFileDisplayName(fileName);

					resPlag.doc = name;

					resPlag.subjectName = subjectName;

					resPlag.coeff = res.Coeff.ToString();

					var pathName = FilesManagementService.GetPathName(fileName);

					resPlag.DocPathName = pathName;

					var userFileT = SubjectManagementService.GetUserLabFile(pathName);

					var userId = userFileT.UserId;

					var user = StudentManagementService.GetStudent(userId);

					resPlag.author = user.FullName;

					resPlag.groupName = user.Group.Name;

					data.Add(resPlag);
				}

				HttpContext.Current.Session.Add(key.ToString(), data.ToList());

				return new ResultViewData
				{
					DataD = data.OrderByDescending(x => int.Parse(x.coeff)).ToList(),
					Message = "Проверка успешно завершена",
					Code = "200"
				};
			}
			catch (Exception e)
			{
				return new ResultViewData
				{
					Message = e.Message + "   " + e,
					Code = "500"
				};
			}
		}

		private static void ClearCache()
		{
			foreach (DictionaryEntry entry_loopVariable in HttpContext.Current.Cache)
			{
				var entry = entry_loopVariable;
				HttpContext.Current.Cache.Remove(entry.Key.ToString());
			}

			IDictionaryEnumerator enumerator = HttpContext.Current.Cache.GetEnumerator();

			while (enumerator.MoveNext())
			{
				HttpContext.Current.Cache.Remove(enumerator.Key.ToString());
			}
			HttpContext.Current.Response.ClearHeaders();
			HttpContext.Current.Response.Expires = 0;
			HttpContext.Current.Response.CacheControl = "no-cache";
			HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
			HttpContext.Current.Response.Cache.SetNoStore();
			HttpContext.Current.Response.Buffer = true;
			HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
			HttpContext.Current.Response.AppendHeader("Pragma", "no-cache");
			HttpContext.Current.Response.AppendHeader("", "");
			HttpContext.Current.Response.AppendHeader("Cache-Control", "no-cache"); //HTTP 1.1
			HttpContext.Current.Response.AppendHeader("Cache-Control", "private"); // HTTP 1.1
			HttpContext.Current.Response.AppendHeader("Cache-Control", "no-store"); // HTTP 1.1
			HttpContext.Current.Response.AppendHeader("Cache-Control", "must-revalidate"); // HTTP 1.1
			HttpContext.Current.Response.AppendHeader("Cache-Control", "max-stale=0"); // HTTP 1.1
			HttpContext.Current.Response.AppendHeader("Cache-Control", "post-check=0"); // HTTP 1.1
			HttpContext.Current.Response.AppendHeader("Cache-Control", "pre-check=0"); // HTTP 1.1
			HttpContext.Current.Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.1
			HttpContext.Current.Response.AppendHeader("Keep-Alive", "timeout=3, max=993"); // HTTP 1.1
		}
	}
}
