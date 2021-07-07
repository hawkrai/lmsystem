using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core.Data;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.UI.Services.Modules.Lectures;
using System.Globalization;
using Newtonsoft.Json;
using WebMatrix.WebData;
using Application.Core;

namespace LMPlatform.UI.Services.Lectures
{
    using Models;
    using Modules;

    public class LecturesService : ILecturesService
    {
        private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();
        private readonly LazyDependency<IGroupManagementService> groupManagementService = new LazyDependency<IGroupManagementService>();

        public IGroupManagementService GroupManagementService => groupManagementService.Value;

        public ISubjectManagementService SubjectManagementService => subjectManagementService.Value;

        public LecturesResult GetLectures(string subjectId)
        {
            try
            {
	            var id = int.Parse(subjectId);
	            var lecturesQuery = new Query<Subject>(e => e.Id == id).Include(e => e.Lectures);
                var model = SubjectManagementService.GetSubject(lecturesQuery).Lectures.Select(e => new LecturesViewData(e)).ToList();

                return new LecturesResult
                {
                    Lectures = model.OrderBy(e => e.Order).ToList(),
                    Message = "Лекции успешно загружены",
                    Code = "200"
                };
            }
            catch
            {
                return new LecturesResult
                {
                    Message = "Произошла ошибка при получении лекций",
                    Code = "500"
                };
            }
        }

        public CalendarResult GetCalendar(string subjectId)
        {
            try
            {
	            var id = int.Parse(subjectId); 
				var lecturesScheduleVisitingsQuery = new Query<Subject>(e => e.Id == id)
					.Include(e => e.LecturesScheduleVisitings);

                var entities = SubjectManagementService.GetSubject(lecturesScheduleVisitingsQuery)
                        .LecturesScheduleVisitings
                        .OrderBy(e => e.Date);
                var model = entities.Select(e => new CalendarViewData(e)).ToList();

                return new CalendarResult
                {
                    Calendar = model,
                    Message = "Рассписание лекций успешно загружено",
                    Code = "200"
                };
            }
            catch
            {
                return new CalendarResult
                {
                    Message = "Произошла ошибка при получении рассписания лекций",
                    Code = "500"
                };
            }
        }

        public ResultViewData Save(int subjectId, int id, string theme, int duration, int order, string pathFile, string attachments)
        {
            try
            {
                var attachmentsModel = JsonConvert.DeserializeObject<List<Attachment>>(attachments).ToList();
                SubjectManagementService.SaveLectures(new Lectures
                {
                    SubjectId = subjectId,
                    Duration = duration,
                    Theme = theme,
                    Order = order,
                    Attachments = pathFile,
                    Id = id
                }, attachmentsModel, WebSecurity.CurrentUserId);

                return new ResultViewData
                {
                    Message = "Лекция успешно сохранена",
                    Code = "200"
                };
            }
            catch (Exception e)
            {
                return new ResultViewData
                {
                    Message = "Произошла ошибка при сохранении лекции." + e.Message,
                    Code = "500"
                };
            }
        }

        public ResultViewData Delete(int id, int subjectId)
        {
            try
            {
                SubjectManagementService.DeleteLection(new Lectures { Id = id });
                return new ResultViewData
                {
                    Message = "Лекция успешно удалена",
                    Code = "200"
                };
            }
            catch (Exception e)
            {
                return new ResultViewData
                {
                    Message = "Произошла ошибка при удалении лекции." + e.Message,
                    Code = "500"
                };
            }
        }

        public ResultViewData SaveDateLectures(int subjectId, string date)
        {
            try
            {
				SubjectManagementService.SaveDateLectures(subjectId, DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
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

        public StudentMarkForDateResult GetMarksCalendarData(int dateId, int subjectId, int groupId)
        {
            try
            {
                var visitingDate =
                    SubjectManagementService.GetScheduleVisitings(
                        new Query<LecturesScheduleVisiting>(e => e.SubjectId == subjectId && e.Id == dateId)).FirstOrDefault();

                var group = GroupManagementService.GetGroup(groupId);
                var model = new List<StudentMarkForDateViewData>();
                foreach (var student in group.Students.OrderBy(e => e.FullName))
                {
                    if (student.LecturesVisitMarks.Any(e => e.LecturesScheduleVisitingId == visitingDate.Id))
                    { 
	                    model.Add(new StudentMarkForDateViewData
						{
							MarkId = student.LecturesVisitMarks.FirstOrDefault(e => e.LecturesScheduleVisitingId == visitingDate.Id).Id,
							StudentId = student.Id,
							Login = student.User.UserName,
							StudentName = student.FullName,
							Mark = student.LecturesVisitMarks.FirstOrDefault(e => e.LecturesScheduleVisitingId == visitingDate.Id).Mark
						});
                    }
                    else
                    {
                        model.Add(new StudentMarkForDateViewData
                        {
                            MarkId = 0,
                            StudentId = student.Id,
                            StudentName = student.FullName,
							Login = student.User.UserName,
                            Mark = string.Empty
                        });
                    }
                }

                return new StudentMarkForDateResult
                {
                    DateId = dateId,
                    Date = visitingDate.Date.ToShortDateString(),
                    StudentMarkForDate = model,
                    Message = "Данные успешно загружены",
                    Code = "200"
                };
            }
            catch
            {
                return new StudentMarkForDateResult
                {
                    Message = "Произошла ошибка",
                    Code = "500"
                };
            }
        }

        public ResultViewData SaveMarksCalendarData(List<LecturesMarkVisitingViewData> lecturesMarks)
        {
            try
            {
                foreach (var student in lecturesMarks)
                {
                    SubjectManagementService.SaveMarksCalendarData(student.Marks
					.Select(e => 
						new LecturesVisitMark
						{
							Id = e.MarkId,
							Mark = e.Mark,
							LecturesScheduleVisitingId = e.LecuresVisitId,
							StudentId = student.StudentId
						}).ToList());
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

		public ResultViewData SaveMarksCalendarDataSingle(int markId, string mark, int lecuresVisitId, int studentId)
		{
			try
			{
				SubjectManagementService.SaveMarksCalendarData(new List<LecturesVisitMark> 
				{
					new LecturesVisitMark
					{
						Id = markId,
						Mark = mark,
						LecturesScheduleVisitingId = lecuresVisitId,
						StudentId = studentId
					}
				});

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
                SubjectManagementService.DeleteLectionVisitingDate(id);

                return new ResultViewData
                {
                    Message = "Дата успешно удалена",
                    Code = "200"
                };
            }
            catch
            {
                return new ResultViewData
                {
                    Message = "Произошла ошибка при удалении даты",
                    Code = "500"
                };
            }
        }

		public ResultViewData DeleteVisitingDates(List<int> dateIds)
		{
			try
			{
				dateIds.ForEach(e => SubjectManagementService.DeleteLectionVisitingDate(e));

				return new ResultViewData
				{
					Message = "Даты успешно удалены",
					Code = "200"
				};
			}
			catch
			{
				return new ResultViewData
				{
					Message = "Произошла ошибка при удалении дат",
					Code = "500"
				};
			}
		}
    }
}
