using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LMPlatform.UI.Services.Modules;
using LMPlatform.UI.Services.Modules.Lectures;

namespace LMPlatform.UI.Services
{
    using System.Data.Entity.ModelConfiguration.Conventions;

    using Application.Core;
    using Application.Core.Data;
    using Application.Infrastructure.GroupManagement;
    using Application.Infrastructure.SubjectManagement;

    using LMPlatform.Models;
    using LMPlatform.UI.Services.Modules.CoreModels;
    using LMPlatform.UI.Services.Modules.Labs;

    public class CoreService : ICoreService
    {
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

        public GroupsResult GetGroups(string subjectId)
        {
            try
            {
                var id = int.Parse(subjectId);
                var groups =
                    GroupManagementService.GetGroups(new Query<Group>(e => e.SubjectGroups.Any(x => x.SubjectId == id)).Include(e => e.Students.Select(x => x.LecturesVisitMarks)))
                        .ToList();

                var model = new List<GroupsViewData>();

                foreach (Group group in groups)
                {
                    IList<SubGroup> subGroups = this.SubjectManagementService.GetSubGroups(id, group.Id);

                    var subjectIntId = int.Parse(subjectId);

                    var lecturesVisitingData = SubjectManagementService.GetScheduleVisitings(new Query<LecturesScheduleVisiting>(e => e.SubjectId == subjectIntId)).OrderBy(e => e.Date);

                    var lecturesVisiting = new List<LecturesMarkVisitingViewData>();

                    foreach (var student in group.Students.OrderBy(e => e.FullName))
                    {
                        var data = new List<MarkViewData>();

                        foreach (var lecturesScheduleVisiting in lecturesVisitingData.OrderBy(e => e.Date))
                        {
                            if (
                                student.LecturesVisitMarks.Any(
                                    e => e.LecturesScheduleVisitingId == lecturesScheduleVisiting.Id))
                            {
                                data.Add(new MarkViewData
                                {
                                    Date = lecturesScheduleVisiting.Date.ToShortDateString(),
                                    LecuresVisitId = lecturesScheduleVisiting.Id,
                                    Mark = student.LecturesVisitMarks.FirstOrDefault(e => e.LecturesScheduleVisitingId == lecturesScheduleVisiting.Id).Mark,
                                    MarkId = student.LecturesVisitMarks.FirstOrDefault(e => e.LecturesScheduleVisitingId == lecturesScheduleVisiting.Id).Id
                                });
                            }
                            else
                            {
                                data.Add(new MarkViewData
                                {
                                    Date = lecturesScheduleVisiting.Date.ToShortDateString(),
                                    LecuresVisitId = lecturesScheduleVisiting.Id,
                                    Mark = string.Empty,
                                    MarkId = 0
                                });
                            }
                        }

                        lecturesVisiting.Add(new LecturesMarkVisitingViewData
                        {
                            StudentId = student.Id,
                            StudentName = student.FullName,
                            Marks = data
                        });
                    }

                    model.Add(new GroupsViewData
                                  {
                                      GroupId = group.Id,
                                      GroupName = group.Name,
                                      LecturesMarkVisiting = lecturesVisiting,
                                      Students = group.Students.Select(e => new StudentsViewData(e)).ToList(),
                                      SubGroupsOne = subGroups.Any() ? new SubGroupsViewData
                                                         {
                                                             GroupId = group.Id,
                                                             Name = "Подгруппа 1",
                                                             ScheduleProtectionLabs = subGroups.FirstOrDefault().ScheduleProtectionLabs.OrderBy(e => e.Date).Select(e => new ScheduleProtectionLabsViewData(e)).ToList(),
                                                             SubGroupId = subGroups.FirstOrDefault().Id,
                                                             Students = subGroups.FirstOrDefault().SubjectStudents.Select(e => new StudentsViewData(e.Student)).ToList()
                                                         } 
                                                         : null,
                                      SubGroupsTwo = subGroups.Any() ? new SubGroupsViewData
                                                          {
                                                              GroupId = group.Id,
                                                              Name = "Подгруппа 2",
                                                              ScheduleProtectionLabs = subGroups.LastOrDefault().ScheduleProtectionLabs.OrderBy(e => e.Date).Select(e => new ScheduleProtectionLabsViewData(e)).ToList(),
                                                              SubGroupId = subGroups.LastOrDefault().Id,
                                                              Students = subGroups.LastOrDefault().SubjectStudents.Select(e => new StudentsViewData(e.Student)).ToList()
                                                          } 
                                                          : null
                                  });
                }

                return new GroupsResult
                {
                    Groups = model,
                    Message = "Группы успешно загружены",
                    Code = "200"
                };
            }
            catch (Exception)
            {
                return new GroupsResult()
                {
                    Message = "Произошла ошибка при получении групп",
                    Code = "500"
                };
            }
        }
    }
}
