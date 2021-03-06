﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Application.Core;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;
using LMPlatform.UI.Services.Modules;
using LMPlatform.UI.Services.Modules.CoreModels;
using LMPlatform.UI.Services.Modules.Labs;
using LMPlatform.UI.Services.Modules.Practicals;
using Newtonsoft.Json;
using WebMatrix.WebData;
using Application.Infrastructure.ConceptManagement;

namespace LMPlatform.UI.Services.Practicals
{
    public class PracticalService : IPracticalService
    {
        private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return subjectManagementService.Value;
            }
        }

        public PracticalsResult GetLabs(string subjectId)
        {
            try
            {
                var sub = SubjectManagementService.GetSubject(int.Parse(subjectId));
                var model = new List<PracticalsViewData>();
                if (sub.SubjectModules.Any(e => e.ModuleId == 13))
                {
                    model = sub.Practicals.Select(e => new PracticalsViewData(e)).ToList();
                }

                return new PracticalsResult
                {
                    Practicals = model,
                    Message = "Практические занятия успешно загружены",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new PracticalsResult()
                {
                    Message = "Произошла ошибка при получении практических занятий",
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
                SubjectManagementService.SavePractical(new Practical
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
                    Message = "Практическое занятие успешно сохранено",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new ResultViewData()
                {
                    Message = "Произошла ошибка при сохранении практического занятия",
                    Code = "500"
                };
            }
        }

        public ResultViewData Delete(string id, string subjectId)
        {
            try
            {
                SubjectManagementService.DeletePracticals(int.Parse(id));
                return new ResultViewData()
                {
                    Message = "Практическое занятие успешно удалено",
                    Code = "200"
                };
            }
            catch (Exception e)
            {
                return new ResultViewData()
                {
                    Message = "Произошла ошибка при удалении практического занятия" + e.Message,
                    Code = "500"
                };
            }
        }

        public ResultViewData SaveScheduleProtectionDate(string groupId, string date, string subjectId)
        {
            try
            {
                SubjectManagementService.SaveScheduleProtectionPracticalDate(new ScheduleProtectionPractical
                {
                    GroupId = int.Parse(groupId),
                    Date = DateTime.Parse(date),
                    SubjectId = int.Parse(subjectId)
                });
                return new ResultViewData()
                {
                    Message = "Дата успешно добавлены",
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

        public List<PracticalVisitingMarkViewData> GetPracticalsVisitingData(string dateId, string subGroupId)
        {
            throw new NotImplementedException();
        }

        public ResultViewData SavePracticalsVisitingData(List<StudentsViewData> students)
        {
            try
            {
                foreach (var studentsViewData in students)
                {
                    SubjectManagementService.SavePracticalVisitingData(studentsViewData.PracticalVisitingMark.Select(e => new ScheduleProtectionPracticalMark
                    {
                        Comment = e.Comment,
                        Mark = e.Mark,
                        ScheduleProtectionPracticalId = e.ScheduleProtectionPracticalId,
                        Id = e.PracticalVisitingMarkId,
                        StudentId = e.StudentId
                    }).ToList());
                }

                return new ResultViewData()
                {
                    Message = "Данные успешно изменены",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new ResultViewData()
                {
                    Message = "Произошла ошибка при изменении данных",
                    Code = "500"
                };
            }
        }

        public ResultViewData SaveStudentPracticalsMark(List<StudentsViewData> students)
        {
            try
            {
                foreach (var studentsViewData in students)
                {
                    SubjectManagementService.SavePracticalMarks(studentsViewData.StudentPracticalMarks.Select(e => new StudentPracticalMark
                    {
                        Mark = e.Mark,
                        PracticalId = e.PracticalId,
                        Id = e.StudentPracticalMarkId,
                        StudentId = e.StudentId
                    }).ToList());
                }

                return new ResultViewData()
                {
                    Message = "Данные успешно изменены",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new ResultViewData()
                {
                    Message = "Произошла ошибка при изменении данных",
                    Code = "500"
                };
            }
        }

        public ResultViewData DeleteVisitingDate(string id)
        {
            try
            {
                SubjectManagementService.DeletePracticalsVisitingDate(int.Parse(id));

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
    }
}
