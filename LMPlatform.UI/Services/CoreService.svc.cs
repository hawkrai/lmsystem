using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Application.Infrastructure.FilesManagement;
using Application.Infrastructure.KnowledgeTestsManagement;
using LMPlatform.UI.Services.Modules;
using LMPlatform.UI.Services.Modules.Lectures;
using WebMatrix.WebData;

namespace LMPlatform.UI.Services
{
	using System.Data.Entity.ModelConfiguration.Conventions;
	using System.Globalization;

	using Application.Core;
	using Application.Core.Data;
	using Application.Infrastructure.GroupManagement;
	using Application.Infrastructure.SubjectManagement;

	using LMPlatform.Models;
	using LMPlatform.UI.Services.Modules.CoreModels;
	using LMPlatform.UI.Services.Modules.Labs;
	using LMPlatform.UI.Services.Modules.Practicals;

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

		private readonly LazyDependency<ITestPassingService> testPassingService = new LazyDependency<ITestPassingService>();

		public ITestPassingService TestPassingService
		{
			get
			{
				return testPassingService.Value;
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

		public GroupsResult GetGroups(string subjectId)
        {
            try
            {
                var id = int.Parse(subjectId);
                var groups =
                    GroupManagementService.GetGroups(new Query<Group>(e => e.SubjectGroups.Any(x => x.SubjectId == id))
                    .Include(e => e.Students.Select(x => x.LecturesVisitMarks))
                    .Include(e => e.Students.Select(x => x.StudentPracticalMarks))
					.Include(e => e.Students.Select(x => x.User))
                    .Include(e => e.Students.Select(x => x.ScheduleProtectionPracticalMarks))
                    .Include(e => e.ScheduleProtectionPracticals)).ToList();

                var model = new List<GroupsViewData>();

                var labsData = SubjectManagementService.GetSubject(int.Parse(subjectId)).Labs.OrderBy(e => e.Order).ToList();
				var practicalsData = new List<Practical>();
	            var isPractModule =
		            SubjectManagementService.GetSubject(int.Parse(subjectId)).SubjectModules.Any(e => e.ModuleId == 13);
				if (isPractModule)
	            {
					practicalsData = SubjectManagementService.GetSubject(int.Parse(subjectId)).Practicals.OrderBy(e => e.Order).ToList();    
	            }

                foreach (Group group in groups)
                {
                    IList<SubGroup> subGroups = this.SubjectManagementService.GetSubGroups(id, group.Id);

                    var subjectIntId = int.Parse(subjectId);

                    var userLabsFile = SubjectManagementService.GetUserLabFiles(0, subjectIntId);

                    var lecturesVisitingData = SubjectManagementService.GetScheduleVisitings(new Query<LecturesScheduleVisiting>(e => e.SubjectId == subjectIntId)).OrderBy(e => e.Date);

                    var lecturesVisiting = new List<LecturesMarkVisitingViewData>();

                    var scheduleProtectionPracticals =
                        group.ScheduleProtectionPracticals.Where(e => e.SubjectId == subjectIntId && e.GroupId == group.Id)
                            .ToList().OrderBy(e => e.Date)
                            .ToList();

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
							Login = student.User.UserName,
                            Marks = data
                        });
                    }

                    //first subGroupLabs
                    var labsFirstSubGroup = labsData.Select(e => new LabsViewData
                    {
                        Theme = e.Theme,
                        Order = e.Order,
                        Duration = e.Duration,
                        ShortName = e.ShortName,
                        ScheduleProtectionLabsRecomend = subGroups.Any() ? subGroups.FirstOrDefault().ScheduleProtectionLabs.OrderBy(x => x.Date)
                            .Select(x => new ScheduleProtectionLab { ScheduleProtectionId = x.Id, Mark = string.Empty }).ToList() : new List<ScheduleProtectionLab>()
                    }).ToList();

                    var durationCount = 0;

                    foreach (var lab in labsFirstSubGroup)
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
                                    if (mark != 0)
                                    {
                                        mark -= 1;
                                    }
                                }
                            }
                        }
                    }

                    //second subGroupLabs
                    var labsSecondSubGroup = labsData.Select(e => new LabsViewData
                    {
                        Theme = e.Theme,
                        Order = e.Order,
                        Duration = e.Duration,
                        ShortName = e.ShortName,
                        ScheduleProtectionLabsRecomend = subGroups.Any() ?
                            subGroups.LastOrDefault()
                            .ScheduleProtectionLabs.OrderBy(x => x.Date)
                            .Select(x => new ScheduleProtectionLab { ScheduleProtectionId = x.Id, Mark = string.Empty })
                            .ToList() : new List<ScheduleProtectionLab>()
                    }).ToList();
                    durationCount = 0;
                    foreach (var lab in labsSecondSubGroup)
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
                                    if (mark != 0)
                                    {
                                        mark -= 1;
                                    }
                                }
                            }
                        }
                    }

                    model.Add(new GroupsViewData
                                  {
                                      GroupId = group.Id,
                                      GroupName = group.Name,
                                      LecturesMarkVisiting = lecturesVisiting,
                                      ScheduleProtectionPracticals = scheduleProtectionPracticals.Select(e => new ScheduleProtectionPracticalViewData
                                      {
                                          GroupId = e.GroupId,
                                          Date = e.Date.ToShortDateString(),
                                          SubjectId = e.SubjectId,
                                          ScheduleProtectionPracticalId = e.Id
                                      }).ToList(),
									  Students = group.Students.OrderBy(e => e.LastName).Select(e => new StudentsViewData(null, e, null, scheduleProtectionPracticals, null, practicalsData)).ToList(),
                                      SubGroupsOne = subGroups.Any() ? new SubGroupsViewData
                                                         {
                                                             GroupId = group.Id,
                                                             Name = "Подгруппа 1",
                                                             Labs = labsFirstSubGroup,
                                                             ScheduleProtectionLabs = subGroups.FirstOrDefault().ScheduleProtectionLabs.OrderBy(e => e.Date).Select(e => new ScheduleProtectionLabsViewData(e)).ToList(),
                                                             SubGroupId = subGroups.FirstOrDefault().Id,
															 Students = subGroups.FirstOrDefault().SubjectStudents.OrderBy(e => e.Student.LastName).Select(e => new StudentsViewData(TestPassingService.GetStidentResults(subjectIntId, e.StudentId), e.Student, subGroups.FirstOrDefault().ScheduleProtectionLabs.OrderBy(x => x.Date).ToList(), null, labsData, null, userLabsFile.Where(x => x.UserId == e.StudentId).Select(t => new UserlabFilesViewData() { Comments = t.Comments, Date = t.Date != null ? t.Date.Value.ToString("dd.MM.yyyy HH:mm") : string.Empty, Id = t.Id, PathFile = t.Attachments, Attachments = FilesManagementService.GetAttachments(t.Attachments).ToList() }).ToList())).ToList()
                                                         }
                                                         : null,
                                      SubGroupsTwo = subGroups.Any() ? new SubGroupsViewData
                                                          {
                                                              GroupId = group.Id,
                                                              Name = "Подгруппа 2",
                                                              Labs = labsSecondSubGroup,
                                                              ScheduleProtectionLabs = subGroups.LastOrDefault().ScheduleProtectionLabs.OrderBy(e => e.Date).Select(e => new ScheduleProtectionLabsViewData(e)).ToList(),
                                                              SubGroupId = subGroups.LastOrDefault().Id,
                                                              Students = subGroups.LastOrDefault().SubjectStudents.OrderBy(e => e.Student.LastName).Select(e => new StudentsViewData(TestPassingService.GetStidentResults(subjectIntId, e.StudentId), e.Student, subGroups.LastOrDefault().ScheduleProtectionLabs.OrderBy(x => x.Date).ToList(), null, labsData, null, userLabsFile.Where(x => x.UserId == e.StudentId).Select(t => new UserlabFilesViewData() { Comments = t.Comments, Date = t.Date != null ? t.Date.Value.ToString("dd.MM.yyyy HH:mm") : string.Empty, Id = t.Id, PathFile = t.Attachments, Attachments = FilesManagementService.GetAttachments(t.Attachments).ToList() }).ToList())).ToList()
                                                          }
                                                          : null
                                  });
                }

	            if (!isPractModule)
	            {
					foreach (var groupsViewData in model)
					{
						foreach (var student in groupsViewData.Students)
						{
							student.PracticalVisitingMark = new List<PracticalVisitingMarkViewData>();
							student.PracticalMarkTotal = "-";
						}
					}    
	            }

                return new GroupsResult
                {
                    Groups = model,
                    Message = "Группы успешно загружены",
                    Code = "200"
                };
            }
            catch (Exception ex)
            {
                return new GroupsResult()
                {
                    Message = ex.Message + "\n" + ex.StackTrace,
                    Code = "500"
                };
            }
        }

		protected int CurrentUserId
		{
			get
			{
				return int.Parse(WebSecurity.CurrentUserId.ToString(CultureInfo.InvariantCulture));
			}
		}
	}
}
