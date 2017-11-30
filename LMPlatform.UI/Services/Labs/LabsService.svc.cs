using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Application.Core;
using Application.Infrastructure.FilesManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;
using LMPlatform.UI.PlagiateReference;
using LMPlatform.UI.Services.Modules;
using LMPlatform.UI.Services.Modules.Labs;
using LMPlatform.UI.Services.Modules.Lectures;
using Newtonsoft.Json;

namespace LMPlatform.UI.Services.Labs
{
    using System.Globalization;

    using ADL.SCORM.LOM;

    using Application.Core.Data;

    using LMPlatform.UI.Services.Modules.CoreModels;
    using Application.Infrastructure.ConceptManagement;
    using Application.Infrastructure.GroupManagement;
    using Application.Infrastructure.KnowledgeTestsManagement;

    using WebMatrix.WebData;

    using DateTime = System.DateTime;
	using System.Configuration;

    public class LabsService : ILabsService
    {
		private readonly LazyDependency<ITestPassingService> testPassingService = new LazyDependency<ITestPassingService>();

		public string PlagiarismUrl
		{
			get { return ConfigurationManager.AppSettings["PlagiarismUrl"]; }
		}

		public string PlagiarismTempPath
		{
			get { return ConfigurationManager.AppSettings["PlagiarismTempPath"]; }
		}

		public string FileUploadPath
		{
			get { return ConfigurationManager.AppSettings["FileUploadPath"]; }
		}
		
		public ITestPassingService TestPassingService
		{
			get
			{
				return testPassingService.Value;
			}
		}

		private readonly LazyDependency<IGroupManagementService> groupManagementService = new LazyDependency<IGroupManagementService>();

		public IGroupManagementService GroupManagementService
		{
			get
			{
				return groupManagementService.Value;
			}
		}

        private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return subjectManagementService.Value;
            }
        }

		private readonly LazyDependency<IFilesManagementService> filesManagementService = new LazyDependency<IFilesManagementService>();

		public IFilesManagementService FilesManagementService
		{
			get
			{
				return filesManagementService.Value;
			}
		}

		public LabsResult GetLabs(string subjectId)
        {
            try
            {
                var model = SubjectManagementService.GetSubject(int.Parse(subjectId)).Labs.OrderBy(e => e.Order).Select(e => new LabsViewData(e)).ToList();
                return new LabsResult
                {
                    Labs = model,
                    Message = "Лабораторные работы успешно загружены",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new LabsResult()
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
				var group = this.GroupManagementService.GetGroups(new Query<Group>(e => e.SubjectGroups.Any(x => x.SubjectId == subjectId && x.GroupId == groupId))
                    .Include(e => e.Students.Select(x => x.StudentLabMarks))
					.Include(e => e.Students.Select(x => x.User))).ToList()[0];

				var labsData = this.SubjectManagementService.GetSubject(subjectId).Labs.OrderBy(e => e.Order).ToList();
				
				var students = new List<StudentsViewData>();
				
				foreach (var student in group.Students)
				{
					students.Add(new StudentsViewData(this.TestPassingService.GetStidentResults(subjectId, student.Id), student, labs: labsData));
				}

				return new StudentsMarksResult
				{
					Students = students.Select(e => new StudentMark()
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
			catch (Exception)
			{
				return new StudentsMarksResult()
				{
					Message = "Произошла ошибка при получении результатов студентов",
					Code = "500"
				};
			}
	    }

	    public ResultViewData Save(string subjectId, string id, string theme, string duration, string order, string shortName, string pathFile, string attachments)
        {
            try
            {
                var attachmentsModel = JsonConvert.DeserializeObject<List<Attachment>>(attachments).ToList();
                var subject = int.Parse(subjectId);
                SubjectManagementService.SaveLabs(new Models.Labs
                {
                    SubjectId = subject,
                    Duration = int.Parse(duration),
                    Theme = theme,
                    Order = int.Parse(order),
                    ShortName = shortName,
                    Attachments = pathFile,
                    Id = int.Parse(id)
                }, attachmentsModel, WebSecurity.CurrentUserId);
                
                return new ResultViewData()
                {
                    Message = "Лабораторная работа успешно сохранена",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new ResultViewData()
                {
                    Message = "Произошла ошибка при сохранении лабораторной работы",
                    Code = "500"
                };
            }
        }

        public ResultViewData Delete(string id, string subjectId)
        {
            try
            {
                SubjectManagementService.DeleteLabs(int.Parse(id));
                return new ResultViewData()
                {
                    Message = "Лабораторная работа успешно удалена",
                    Code = "200"
                };
            }
            catch (Exception e)
            {
                return new ResultViewData()
                {
                    Message = "Произошла ошибка при удалении лабораторной работы" + e.Message,
                    Code = "500"
                };
            }
        }

        public ResultViewData SaveScheduleProtectionDate(string subGroupId, string date)
        {
            try
            {
				SubjectManagementService.SaveScheduleProtectionLabsDate(int.Parse(subGroupId), DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                return new ResultViewData()
                {
                    Message = "Дата успешно добавлена",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new ResultViewData()
                {
                    Message = "Произошла ошибка при добавлении даты",
                    Code = "500"
                };
            }
        }

        public ResultViewData SaveLabsVisitingData(int dateId, List<string> marks, List<string> comments, List<int> studentsId, List<int> Id, List<StudentsViewData> students)
        {
            try
            {
                int count = studentsId.Count;
                string currentMark, currentComment;
                int currentStudentId, currentId;

                for (int i = 0; i < count; i++)
                {
                    currentMark = marks[i];
                    currentComment = comments[i];
                    currentStudentId = studentsId[i];
                    currentId = Id[i];

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

                return new ResultViewData()
                {
                    Message = "Данные успешно добавлены",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new ResultViewData()
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

                return new ResultViewData()
                {
                    Message = "Данные успешно добавлены",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new ResultViewData()
                {
                    Message = "Произошла ошибка при добавлении данных",
                    Code = "500"
                };
            }
        }

		public ResultViewData DeleteVisitingDate(string id)
        {
            try
            {
                SubjectManagementService.DeleteLabsVisitingDate(int.Parse(id));

                return new ResultViewData()
                {
                    Message = "Дата успешно удалена",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new ResultViewData()
                {
                    Message = "Произошла ошибка при удалении даты",
                    Code = "500"
                };
            }
        }

        public UserLabFilesResult GetFilesLab(string userId, string subjectId)
        {
            try
            {
	            var model = new List<UserlabFilesViewData>();
	            var data = SubjectManagementService.GetUserLabFiles(int.Parse(userId), int.Parse(subjectId));
	            model = data.Select(e => new UserlabFilesViewData()
	            {
		            Comments = e.Comments,
					Id = e.Id,
					PathFile = e.Attachments,
					IsReceived = e.IsReceived,
                    Date = e.Date != null ? e.Date.Value.ToString("dd.MM.yyyy HH:mm") : string.Empty,
		            Attachments = FilesManagementService.GetAttachments(e.Attachments).ToList()
	            }).ToList();
                return new UserLabFilesResult()
                {
					UserLabFiles = model,
                    Message = "Данные получены",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new UserLabFilesResult()
                {
                    Message = "Произошла ошибка при получении данных",
                    Code = "500"
                };
            }
        }

		public ResultViewData SendFile(string subjectId, string userId, string id, string comments, string pathFile, string attachments)
		{
			try
			{
				var attachmentsModel = JsonConvert.DeserializeObject<List<Attachment>>(attachments).ToList();

				SubjectManagementService.SaveUserLabFiles(new Models.UserLabFiles()
				{
					SubjectId = int.Parse(subjectId),
                    Date = DateTime.Now,
					UserId = int.Parse(userId),
					Comments = comments,
					Attachments = pathFile,
					Id = int.Parse(id)
				}, attachmentsModel);

				return new ResultViewData()
				{
					Message = "Файл(ы) успешно отправлен(ы)",
					Code = "200"
				};
			}
			catch (Exception)
			{
				return new ResultViewData()
				{
					Message = "Произошла ошибка",
					Code = "500"
				};
			}
		}

		public ResultViewData DeleteUserFile(string id)
		{
			try
			{
				SubjectManagementService.DeleteUserLabFile(int.Parse(id));
				return new ResultViewData()
				{
					Message = "Работа удалена",
					Code = "200"
				};
			}
			catch (Exception e)
			{
				return new ResultViewData()
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
					students.Add(new StudentsViewData(this.TestPassingService.GetStidentResults(subjectId, student.Id), student, scheduleProtectionLabs: scheduleProtectionLabs, labs: labsData));
				}

				return new StudentsMarksResult
				{
					Students = students.Select(e => new StudentMark()
					{
						FullName = e.FullName,
						Login = e.Login,
						SubGroup = subGroups.FirstOrDefault(x => x.Name == "first").SubjectStudents.Any(x => x.StudentId == e.StudentId) ? 1 : subGroups.FirstOrDefault(x => x.Name == "second").SubjectStudents.Any(x => x.StudentId == e.StudentId) ? 2 : subGroups.FirstOrDefault(x => x.Name == "third").SubjectStudents.Any(x => x.StudentId == e.StudentId) ? 3 : 4,
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
			catch (Exception)
			{
				return new StudentsMarksResult()
				{
					Message = "Произошла ошибка при получении результатов студентов",
					Code = "500"
				};
			}
		}

		public StudentsMarksResult GetFilesV2(int subjectId, int groupId)
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
							new UserlabFilesViewData()
							{
								Comments = t.Comments,
								Date = t.Date != null ? t.Date.Value.ToString("dd.MM.yyyy HH:mm") : string.Empty,
								Id = t.Id,
								PathFile = t.Attachments,
								IsReceived = t.IsReceived,
								Attachments = FilesManagementService.GetAttachments(t.Attachments).ToList()
							}).ToList();
					students.Add(new StudentMark
						             {
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
			catch (Exception)
			{
				return new StudentsMarksResult()
				{
					Message = "Произошла ошибка при получении результатов студентов",
					Code = "500"
				};
			}
		}

		public LabsResult GetLabsV2(string subjectId, int groupId)
		{
			try
			{
				var id = int.Parse(subjectId);
				var labs = this.SubjectManagementService.GetLabsV2(id).OrderBy(e => e.Order);

				IList<SubGroup> subGroups = this.SubjectManagementService.GetSubGroupsV2WithScheduleProtectionLabs(id, groupId);

				var labsSubOne = labs.Select(e => new LabsViewData
				{
					Theme = e.Theme,
					Order = e.Order,
					Duration = e.Duration,
					ShortName = e.ShortName,
					LabId = e.Id,
					SubjectId = e.SubjectId,
					SubGroup = 1,
					ScheduleProtectionLabsRecomend = subGroups.Any() ? subGroups.FirstOrDefault(x => x.Name == "first").ScheduleProtectionLabs.OrderBy(x => x.Date).Select(x => new ScheduleProtectionLab { ScheduleProtectionId = x.Id, Mark = string.Empty }).ToList() : new List<ScheduleProtectionLab>()
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
					ScheduleProtectionLabsRecomend = subGroups.Any() ? subGroups.FirstOrDefault(x => x.Name == "second").ScheduleProtectionLabs.OrderBy(x => x.Date).Select(x => new ScheduleProtectionLab { ScheduleProtectionId = x.Id, Mark = string.Empty }).ToList() : new List<ScheduleProtectionLab>()
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
					ScheduleProtectionLabsRecomend = subGroups.Any() ? subGroups.FirstOrDefault(x => x.Name == "third").ScheduleProtectionLabs.OrderBy(x => x.Date).Select(x => new ScheduleProtectionLab { ScheduleProtectionId = x.Id, Mark = string.Empty }).ToList() : new List<ScheduleProtectionLab>()
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
					subGroups.FirstOrDefault() != null ? subGroups.FirstOrDefault(e => e.Name == "first").ScheduleProtectionLabs.OrderBy(e => e.Date).Select(
					e => new ScheduleProtectionLabsViewData(e)).ToList() : new List<ScheduleProtectionLabsViewData>();

				scheduleProtectionLabsOne.ForEach(e => e.SubGroup = 1);

				var scheduleProtectionLabsTwo =
					subGroups.LastOrDefault() != null ? subGroups.FirstOrDefault(e => e.Name == "second").ScheduleProtectionLabs.OrderBy(e => e.Date).Select(
					e => new ScheduleProtectionLabsViewData(e)).ToList() : new List<ScheduleProtectionLabsViewData>();

				scheduleProtectionLabsTwo.ForEach(e => e.SubGroup = 2);

				var scheduleProtectionLabsThird =
					subGroups.LastOrDefault() != null ? subGroups.FirstOrDefault(e => e.Name == "third").ScheduleProtectionLabs.OrderBy(e => e.Date).Select(
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
			catch (Exception)
			{
				return new LabsResult()
				{
					Message = "Произошла ошибка при получении лабораторых работ",
					Code = "500"
				};
			}
		}

		public ResultViewData ReceivedLabFile(string userFileId)
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
			catch (Exception e)
			{
				return new ResultViewData
				{
					Message = "Произошла ошибка переноса файла в архив",
					Code = "500"
				};
			}
		}

		public ResultViewData CancelReceivedLabFile(string userFileId)
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
			catch (Exception e)
			{
				return new ResultViewData
							{
								Message = "Произошла ошибка переноса файла из архива",
								Code = "500"
							};
			}
		}

		public ResultViewData CheckPlagiarism(string userFileId, string subjectId)
		{
			try
			{

				var path = Guid.NewGuid().ToString("N");

				Directory.CreateDirectory(this.PlagiarismTempPath + path);

				var userFile = this.SubjectManagementService.GetUserLabFile(Int32.Parse(userFileId));

				var usersFiles = this.SubjectManagementService.GetUserLabFiles(0, Int32.Parse(subjectId)).Where(e => e.IsReceived && e.Id != userFile.Id);

				var filesPaths = usersFiles.Select(e => e.Attachments);

				foreach (var filesPath in filesPaths)
				{
					foreach (var srcPath in Directory.GetFiles(this.FileUploadPath + filesPath))
					{
						File.Copy(srcPath, srcPath.Replace(this.FileUploadPath + filesPath, this.PlagiarismTempPath + path), true);
					}
				}

				string firstFileName =
					Directory.GetFiles(this.FileUploadPath + userFile.Attachments)
					.Select(fi => fi)
					.FirstOrDefault();

				var service = new SoapWSClient();

				var result = service.checkBySingleDoc(firstFileName, new string[] { this.PlagiarismTempPath + path }, 70, 5);

				List<ResultPlag> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResultPlag>>(result);

				foreach (var resultPlag in data)
				{
					var fileName = Path.GetFileName(resultPlag.doc);

					var name = this.FilesManagementService.GetFileDisplayName(fileName);

					resultPlag.doc = name;
				}

				Directory.Delete(this.PlagiarismTempPath + path, true);

				return new ResultViewData
				{
					DataD = data,
					Message = "Проверка успешно завершена",
					Code = "200"
				};
			}
			catch (Exception e)
			{
				return new ResultViewData
				{
					Message = "Проверка завершилась неудачей",
					Code = "500"
				};
			}
		}
    }
}
