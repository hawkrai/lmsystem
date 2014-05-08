using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LMPlatform.UI.Services.Lectures
{
    using System.Globalization;

    using Application.Core;
    using Application.Infrastructure.SubjectManagement;

    using LMPlatform.Models;
    using LMPlatform.UI.Services.Modules;
    using LMPlatform.UI.Services.Modules.Lectures;
    using LMPlatform.UI.Services.Modules.News;

    using Newtonsoft.Json;

    public class LecturesService : ILecturesService
    {
        private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return subjectManagementService.Value;
            }
        }

        public LecturesResult GetLectures(string subjectId)
        {
            try
            {
                var model = SubjectManagementService.GetSubject(int.Parse(subjectId)).Lectures.Select(e => new LecturesViewData(e)).ToList();

                return new LecturesResult
                {
                    Lectures = model,
                    Message = "Лекции успешно загружены",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new LecturesResult()
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
                var entities =
                    SubjectManagementService.GetSubject(int.Parse(subjectId))
                        .LecturesScheduleVisitings.ToList().OrderBy(e => e.Date)
                        .ToList();
                var model = entities.Select(e => new CalendarViewData(e)).ToList();

                return new CalendarResult
                {
                    Calendar = model,
                    Message = "Рассписание лекций успешно загружено",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new CalendarResult()
                {
                    Message = "Произошла ошибка при получении рассписания лекций",
                    Code = "500"
                };
            }
        }

        public ResultViewData Save(string subjectId, string id, string theme, string duration, string pathFile, string attachments)
        {
            try
            {
                var attachmentsModel = JsonConvert.DeserializeObject<List<Attachment>>(attachments).ToList();

                SubjectManagementService.SaveLectures(new Lectures
                {
                    SubjectId = int.Parse(subjectId),
                    Duration = int.Parse(duration),
                    Theme = theme,
                    Order = 0,
                    Attachments = pathFile,
                    Id = int.Parse(id)
                }, attachmentsModel);
                return new ResultViewData()
                {
                    Message = "Лекция успешно сохранена",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new ResultViewData()
                {
                    Message = "Произошла ошибка при сохранении лекции",
                    Code = "500"
                };
            }
        }

        public ResultViewData Delete(string id, string subjectId)
        {
            throw new NotImplementedException();
        }

        public ResultViewData SaveDateLectures(string subjectId, string date)
        {
            try
            {
                SubjectManagementService.SaveDateLectures(int.Parse(subjectId), DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture));
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
    }
}
