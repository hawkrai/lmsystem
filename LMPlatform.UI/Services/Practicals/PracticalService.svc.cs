using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Core.Data;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;
using LMPlatform.UI.Services.Modules;
using LMPlatform.UI.Services.Modules.CoreModels;
using LMPlatform.UI.Services.Modules.Practicals;
using Newtonsoft.Json;
using WebMatrix.WebData;

namespace LMPlatform.UI.Services.Practicals
{
    public class PracticalService : IPracticalService
    {
        private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();

        public ISubjectManagementService SubjectManagementService => subjectManagementService.Value;

        private const int PracticalModuleId = 13;

        public PracticalsResult GetLabs(string subjectId)
        {
            try
            {
	            var id = int.Parse(subjectId);
	            var query = new Query<Subject>(s => s.Id == id)
	                .Include(s => s.SubjectModules)
	                .Include(s => s.Practicals);
                var sub = SubjectManagementService.GetSubject(query);
                var model = new List<PracticalsViewData>();
                if (sub.SubjectModules.Any(e => e.ModuleId == PracticalModuleId))
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
            catch
            {
                return new PracticalsResult
                {
                    Message = "Произошла ошибка при получении практических занятий",
                    Code = "500"
                };
            }
        }

        public ResultViewData Save(int subjectId, int id, string theme, int duration, int order, string shortName, string pathFile, string attachments)
        {
            try
            {
                var attachmentsModel = JsonConvert.DeserializeObject<List<Attachment>>(attachments).ToList();
                SubjectManagementService.SavePractical(new Practical
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
                    Message = "Практическое занятие успешно сохранено",
                    Code = "200"
                };
            }
            catch
            {
                return new ResultViewData
                {
                    Message = "Произошла ошибка при сохранении практического занятия",
                    Code = "500"
                };
            }
        }

        public ResultViewData Delete(int id, int subjectId)
        {
            try
            {
                SubjectManagementService.DeletePracticals(id);
                return new ResultViewData
                {
                    Message = "Практическое занятие успешно удалено",
                    Code = "200"
                };
            }
            catch (Exception e)
            {
                return new ResultViewData
                {
                    Message = "Произошла ошибка при удалении практического занятия" + e.Message,
                    Code = "500"
                };
            }
        }

        public ResultViewData SaveScheduleProtectionDate(int groupId, string date, int subjectId)
        {
            try
            {
                SubjectManagementService.SaveScheduleProtectionPracticalDate(new ScheduleProtectionPractical
                {
                    GroupId = groupId,
                    Date = DateTime.Parse(date),
                    SubjectId = subjectId
                });
                return new ResultViewData
                {
                    Message = "Дата успешно добавлены",
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

                return new ResultViewData
                {
                    Message = "Данные успешно изменены",
                    Code = "200"
                };
            }
            catch
            {
                return new ResultViewData
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

                return new ResultViewData
                {
                    Message = "Данные успешно изменены",
                    Code = "200"
                };
            }
            catch
            {
                return new ResultViewData
                {
                    Message = "Произошла ошибка при изменении данных",
                    Code = "500"
                };
            }
        }

        public ResultViewData DeleteVisitingDate(int id)
        {
            try
            {
                SubjectManagementService.DeletePracticalsVisitingDate(id);

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
    }
}
