using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Application.Core;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;
using LMPlatform.UI.Services.Modules;
using LMPlatform.UI.Services.Modules.Labs;
using LMPlatform.UI.Services.Modules.Lectures;
using Newtonsoft.Json;

namespace LMPlatform.UI.Services.Labs
{
    using System.Globalization;

    using LMPlatform.UI.Services.Modules.CoreModels;

    public class LabsService : ILabsService
    {
        private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return subjectManagementService.Value;
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

        public ResultViewData Save(string subjectId, string id, string theme, string duration, string order, string shortName, string pathFile, string attachments)
        {
            try
            {
                var attachmentsModel = JsonConvert.DeserializeObject<List<Attachment>>(attachments).ToList();

                SubjectManagementService.SaveLabs(new Models.Labs
                {
                    SubjectId = int.Parse(subjectId),
                    Duration = int.Parse(duration),
                    Theme = theme,
                    Order = int.Parse(order),
                    ShortName = shortName,
                    Attachments = pathFile,
                    Id = int.Parse(id)
                }, attachmentsModel);
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
            throw new NotImplementedException();
        }

        public ResultViewData SaveScheduleProtectionDate(string subGroupId, string date)
        {
            try
            {
                SubjectManagementService.SaveScheduleProtectionLabsDate(int.Parse(subGroupId), DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture));
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

        public ResultViewData SaveLabsVisitingData(List<StudentsViewData> students)
        {
            try
            {
                foreach (var student in students)
                {
                    SubjectManagementService.SaveLabsVisitingData(student.LabVisitingMark.Select(e => new ScheduleProtectionLabMark { Id = e.LabVisitingMarkId, Comment = e.Comment, Mark = e.Mark, StudentId = e.StudentId, ScheduleProtectionLabId = e.ScheduleProtectionLabId }).ToList());    
                }
                
                return new ResultViewData()
                {
                    Message = "Данные успешно добавлена",
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

        public ResultViewData SaveStudentLabsMark(List<StudentsViewData> students)
        {
            try
            {
                foreach (var student in students)
                {
                    SubjectManagementService.SaveStudentLabsMark(student.StudentLabMarks.Select(e => new StudentLabMark { Id = e.StudentLabMarkId, LabId = e.LabId, StudentId = e.StudentId, Mark = e.Mark }).ToList());    
                }

                return new ResultViewData()
                {
                    Message = "Данные успешно добавлена",
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
    }
}
