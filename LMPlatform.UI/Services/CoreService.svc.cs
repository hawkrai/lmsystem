using System;
using System.Collections.Generic;
using System.Linq;
using Application.Infrastructure.FilesManagement;
using Application.Infrastructure.KnowledgeTestsManagement;
using LMPlatform.UI.Services.Modules;
using LMPlatform.UI.Services.Modules.Lectures;
using WebMatrix.WebData;
using System.Globalization;
using Application.Core;
using Application.Core.Data;
using Application.Core.Extensions;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.LecturerManagement;
using Application.Infrastructure.StudentManagement;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.Models;
using LMPlatform.UI.Services.Modules.CoreModels;
using LMPlatform.UI.Services.Modules.Labs;
using LMPlatform.UI.Services.Modules.Parental;
using LMPlatform.UI.Services.Modules.Practicals;

namespace LMPlatform.UI.Services
{
	public class CoreService : ICoreService
	{
		private readonly LazyDependency<IGroupManagementService> groupManagementService = new LazyDependency<IGroupManagementService>();

		public IGroupManagementService GroupManagementService => groupManagementService.Value;

		private readonly LazyDependency<IStudentManagementService> studentManagementService = new LazyDependency<IStudentManagementService>();

		public IStudentManagementService StudentManagementService => studentManagementService.Value;

		private readonly LazyDependency<ITestPassingService> testPassingService = new LazyDependency<ITestPassingService>();

		public ITestPassingService TestPassingService => testPassingService.Value;

		private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();

		public ISubjectManagementService SubjectManagementService => subjectManagementService.Value;

		private readonly LazyDependency<IFilesManagementService> filesManagementService = new LazyDependency<IFilesManagementService>();

		public IFilesManagementService FilesManagementService => filesManagementService.Value;

		private readonly LazyDependency<ILecturerManagementService> lecturerManagementService = new LazyDependency<ILecturerManagementService>();

		public ILecturerManagementService LecturerManagementService => lecturerManagementService.Value;

		public ResultViewData DisjoinLector(int subjectId, int lectorId)
		{
			try
			{
				this.LecturerManagementService.DisjoinLector(subjectId, lectorId, CurrentUserId);

				return new ResultViewData
				{
					Message = "Связь успешно удалена",
					Code = "200"
				};
			}
			catch (Exception ex)
			{
				return new LectorResult
				{
					Message = ex.Message + "\n" + ex.StackTrace,
					Code = "500"
				};
			}	
		}

		public LectorResult GetJoinedLector(string subjectId)
		{
			try
			{
				var id = int.Parse(subjectId);
				var lectors = this.LecturerManagementService.GetJoinedLector(id, this.CurrentUserId);

				return new LectorResult
				{
					Lectors = lectors.Select(e => new LectorViewData(e)).ToList(),
					Message = "Присоединенные преподаватели успешно загружены",
					Code = "200"
				};
			}
			catch (Exception ex)
			{
				return new LectorResult
				{
					Message = ex.Message + "\n" + ex.StackTrace,
					Code = "500"
				};
			}
		}

		public ResultViewData JoinLector(int subjectId, int lectorId)
		{
			try
			{
				this.LecturerManagementService.Join(subjectId, lectorId, CurrentUserId);

				return new ResultViewData
				{
					Message = "Преподаватель успешно заприсоединен к предмету",
					Code = "200"
				};
			}
			catch (Exception ex)
			{
				return new LectorResult
				{
					Message = ex.Message + "\n" + ex.StackTrace,
					Code = "500"
				};
			}	
		}

		public LectorResult GetNoAdjointLectors(string subjectId)
		{
			try
			{
				var lectors = this.LecturerManagementService.GetLecturers()
					.Where(e => e.Id != this.CurrentUserId && !e.SubjectLecturers
						            .Any(x => x.SubjectId == int.Parse(subjectId) && x.Owner == this.CurrentUserId));

				return new LectorResult
				{
					Lectors = lectors.Select(e => new LectorViewData(e)).ToList(),
					Message = "Преподаватели успешно загружены",
					Code = "200"
				};
			}
			catch (Exception ex)
			{
				return new LectorResult
				{
					Message = ex.Message + "\n" + ex.StackTrace,
					Code = "500"
				};
			}
		}

		public SubjectsResult GetSubjectsByOwnerUser()
		{
			try
			{
				var subjects = this.SubjectManagementService.GetSubjectsByLectorOwner(this.CurrentUserId, lite: true);

				return new SubjectsResult
				{
					Subjects = subjects.Select(e => new SubjectViewData
					{
						Id = e.Id,
						Name = e.Name
					}).ToList(),
					Message = "Предметы успешно загружены",
					Code = "200"
				};
			}
			catch (Exception ex)
			{
				return new SubjectsResult
				{
					Message = ex.Message + "\n" + ex.StackTrace,
					Code = "500"
				};
			}
		}

		public StudentsResult СonfirmationStudent(string studentId)
		{
			try
			{
				var id = int.Parse(studentId);
				this.StudentManagementService.СonfirmationStudent(id);

				return new StudentsResult
				{
					Message = "Студент успешно подтвержден",
					Code = "200"
				};
			}
			catch (Exception ex)
			{
				return new StudentsResult
				{
					Message = ex.Message + "\n" + ex.StackTrace,
					Code = "500"
				};
			}
		}

		public StudentsResult UnConfirmationStudent(string studentId)
		{
			try
			{
				var id = int.Parse(studentId);
				this.StudentManagementService.UnConfirmationStudent(id);

				return new StudentsResult
				{
					Message = "Подтверждение отменено",
					Code = "200"
				};
			}
			catch (Exception ex)
			{
				return new StudentsResult
				{
					Message = ex.Message + "\n" + ex.StackTrace,
					Code = "500"
				};
			}
		}

		public StudentsResult GetStudentsByGroupId(string groupId)
		{
			try
			{
				var id = int.Parse(groupId);
				var students = this.GroupManagementService.GetGroups(
					new Query<Group>(g => g.Id == id).Include(g => g.Students))
					.Single().Students
					.OrderBy(e => e.FullName);

				return new StudentsResult
				{
					Students = students.Select(e => new StudentsViewData
					{
						StudentId = e.Id,
						FullName = e.FullName,
						Confirmed  = e.Confirmed == null || e.Confirmed.Value
					}).ToList(),
					Message = "Студенты успешно загружены",
					Code = "200"
				};
			}
			catch (Exception ex)
			{
				return new StudentsResult
				{
					Message = ex.Message + "\n" + ex.StackTrace,
					Code = "500"
				};
			}
		}

		public StudentsResult GetStudentsByStudentGroupId(string groupId, string subjectId)
        {
            try
            {
                var subGroups = this.SubjectManagementService.GetSubGroupsV3(int.Parse(subjectId), int.Parse(groupId));
                var students = new List<StudentsViewData>();
                var subGroupIndex = 0;
                foreach (var subGroup in subGroups)
                {
                    students.AddRange(subGroup.SubjectStudents.Select(e => new StudentsViewData
                    {
                        StudentId = e.Student.Id,
                        FullName = e.Student.FullName,
                        Confirmed = e.Student.Confirmed == null || e.Student.Confirmed.Value,
                        SubgroupId = subGroupIndex,
                        Login = e.Student.User.UserName
                    }));
                    subGroupIndex++;
                }

                return new StudentsResult
                {
                    Students = students,
                    Message = "Студенты успешно загружены",
                    Code = "200"
                };
            }
            catch (Exception ex)
            {
                return new StudentsResult
                {
                    Message = ex.Message + "\n" + ex.StackTrace,
                    Code = "500"
                };
            }
        }

        public GroupsResult GetAllGroupsLite()
		{
			try
			{
				var groups = this.GroupManagementService.GetLecturesGroups(WebSecurity.CurrentUserId);

				var groupsViewModel = new List<GroupsViewData>();

				foreach (var @group in groups.DistinctBy(e => e.Id))
				{
					var students = this.StudentManagementService.GetGroupStudents(@group.Id).Count(e => e.Confirmed != null && !e.Confirmed.Value);

					groupsViewModel.Add(new GroupsViewData()
					{
						CountUnconfirmedStudents = students,
						GroupId = @group.Id,
						GroupName = students > 0 ? @group.Name + " - (" + students + ")" : @group.Name
					});
				}

				return new GroupsResult
				{
					Groups = groupsViewModel.ToList(),
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

        public GroupsResult GetOnlyGroups(string subjectId)
        {
            try
            {
                var id = int.Parse(subjectId);
                var groups = GroupManagementService.GetGroups(new Query<Group>(e => e.SubjectGroups.Any(x => x.SubjectId == id && x.IsActiveOnCurrentGroup)));
                return new GroupsResult
                {
                    Groups = groups.Select(e => new GroupsViewData
                    {
	                    GroupId = e.Id,
	                    GroupName = e.Name
                    }).ToList(),
                    Message = "Группы успешно загружены",
                    Code = "200"
                };
            }
            catch (Exception ex)
            {
                return new GroupsResult
                {
                    Message = ex.Message + "\n" + ex.StackTrace,
                    Code = "500"
                };
            }
        }

		public GroupsResult GetGroupsV2(string subjectId)
		{
			try
			{
				var id = int.Parse(subjectId);
				var groups = this.GroupManagementService.GetGroups(new Query<Group>(e => e.SubjectGroups.Any(x => x.SubjectId == id && x.IsActiveOnCurrentGroup)));


				var groupsViewData = new List<GroupsViewData>();

				foreach (var @group in groups)
				{
					var subGroups = this.SubjectManagementService.GetSubGroupsV2(id, @group.Id);
					groupsViewData.Add(new GroupsViewData
					{
						GroupId = @group.Id,
						GroupName = @group.Name,
						SubGroupsOne = subGroups.Any(x => x.Name == "first") ? new SubGroupsViewData
							               {
											   Name = "Подгруппа 1",
											   SubGroupId = subGroups.First(e => e.Name == "first").Id
							               } : new SubGroupsViewData(),
						SubGroupsTwo = subGroups.Any(x => x.Name == "second") ? new SubGroupsViewData
						{
							Name = "Подгруппа 2",
							SubGroupId = subGroups.First(e => e.Name == "second").Id
						} : new SubGroupsViewData(),
						SubGroupsThird = subGroups.Any(x => x.Name == "third") ? new SubGroupsViewData
						{
							Name = "Подгруппа 3",
							SubGroupId = subGroups.First(e => e.Name == "third").Id
						} : new SubGroupsViewData(),
					});
				}

				return new GroupsResult
                {
					
					Groups = groupsViewData.OrderBy(e => e.GroupName).ToList(),
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

        public GroupsResult GetGroupsV3(string subjectId)
        {
            try
            {
                var id = int.Parse(subjectId);
                var groups = this.GroupManagementService.GetGroups(new Query<Group>(e => e.SubjectGroups.Any(x => x.SubjectId == id && !x.IsActiveOnCurrentGroup)));


                var groupsViewData = new List<GroupsViewData>();

                foreach (var @group in groups)
                {
                    var subGroups = this.SubjectManagementService.GetSubGroupsV2(id, @group.Id);
                    groupsViewData.Add(new GroupsViewData
                    {
                        GroupId = @group.Id,
                        GroupName = @group.Name,
                        SubGroupsOne = subGroups.Any(x => x.Name == "first") ? new SubGroupsViewData
                        {
                            Name = "Подгруппа 1",
                            SubGroupId = subGroups.First(e => e.Name == "first").Id
                        } : new SubGroupsViewData(),
                        SubGroupsTwo = subGroups.Any(x => x.Name == "second") ? new SubGroupsViewData
                        {
                            Name = "Подгруппа 2",
                            SubGroupId = subGroups.First(e => e.Name == "second").Id
                        } : new SubGroupsViewData(),
                        SubGroupsThird = subGroups.Any(x => x.Name == "third") ? new SubGroupsViewData
                        {
                            Name = "Подгруппа 3",
                            SubGroupId = subGroups.First(e => e.Name == "third").Id
                        } : new SubGroupsViewData(),
                    });
                }

                return new GroupsResult
                {
                    Groups = groupsViewData,
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

        public LecturesMarkVisitingResult GetLecturesMarkVisitingV2(int subjectId, int groupId)
		{
			try
			{
				var groups = this.GroupManagementService.GetGroup(groupId);

				var lecturesVisitingData = SubjectManagementService.GetScheduleVisitings(new Query<LecturesScheduleVisiting>(e => e.SubjectId == subjectId)).OrderBy(e => e.Date);

				var lecturesVisiting = new List<LecturesMarkVisitingViewData>();

				foreach (var student in groups.Students.Where(e => e.Confirmed == null || e.Confirmed.Value).OrderBy(e => e.FullName))
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

				var dataResulet = new List<LecturesGroupsVisitingViewData>()
					                  {
						                  new LecturesGroupsVisitingViewData()
							                  {
								                  GroupId =
									                  groupId,
								                  LecturesMarksVisiting
									                  =
									                  lecturesVisiting
							                  }
					                  };

				return new LecturesMarkVisitingResult
				{
					GroupsVisiting = dataResulet,
					Message = "",
					Code = "200"
				};
			}
			catch (Exception ex)
			{
				return new LecturesMarkVisitingResult()
				{
					Message = ex.Message + "\n" + ex.StackTrace,
					Code = "500"
				};
			}
		}

		public GroupsResult GetGroupsByUser(string userId)
		{
			try
			{
				var id = int.Parse(userId);
				var groups = this.GroupManagementService.GetLecturesGroups(id);

				var groupsViewModel = new List<GroupsViewData>();

				foreach (var group in groups.DistinctBy(e => e.Id))
				{
					groupsViewModel.Add(new GroupsViewData
					{
						GroupId = group.Id,
						GroupName = group.Name
					});
				}

				return new GroupsResult
				{
					Groups = groupsViewModel.ToList(),
					Message = "Группы успешно загружены",
					Code = "200"
				};
			}
			catch (Exception ex)
			{
				return new GroupsResult
				{
					Message = ex.Message + "\n" + ex.StackTrace,
					Code = "500"
				};
			}
		}

		public GroupsResult GetGroups(string subjectId, string groupId)
        {
            try
            {
				var id = int.Parse(subjectId);

				Query<Group> query;

				if (!string.IsNullOrEmpty(groupId))
				{
					var groupdId = int.Parse(groupId);
					query = (Query<Group>)new Query<Group>(e => e.SubjectGroups.Any(x => x.SubjectId == id && x.GroupId == groupdId))
						.Include(e => e.Students.Select(x => x.LecturesVisitMarks))
						.Include(e => e.Students.Select(x => x.StudentPracticalMarks)).Include(e => e.Students.Select(x => x.User))
						.Include(e => e.Students.Select(x => x.ScheduleProtectionPracticalMarks))
						.Include(e => e.ScheduleProtectionPracticals);
				}
				else
				{
					query = (Query<Group>)new Query<Group>(e => e.SubjectGroups.Any(x => x.SubjectId == id))
						.Include(e => e.Students.Select(x => x.LecturesVisitMarks))
						.Include(e => e.Students.Select(x => x.StudentPracticalMarks)).Include(e => e.Students.Select(x => x.User))
						.Include(e => e.Students.Select(x => x.ScheduleProtectionPracticalMarks))
						.Include(e => e.ScheduleProtectionPracticals);
				}

				var groups =
					GroupManagementService.GetGroups(query).ToList();

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
					
                    var lecturesVisitingData = SubjectManagementService.GetScheduleVisitings(new Query<LecturesScheduleVisiting>(e => e.SubjectId == subjectIntId)).OrderBy(e => e.Date);

                    var lecturesVisiting = new List<LecturesMarkVisitingViewData>();

                    var scheduleProtectionPracticals =
                        group.ScheduleProtectionPracticals.Where(e => e.SubjectId == subjectIntId && e.GroupId == group.Id)
                            .ToList().OrderBy(e => e.Date)
                            .ToList();

					foreach (var student in group.Students.Where(e => e.Confirmed == null || e.Confirmed.Value).OrderBy(e => e.FullName))
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
                        LabId = e.Id,
                        SubjectId = e.SubjectId,
						ScheduleProtectionLabsRecomend = subGroups.Any() ? subGroups.FirstOrDefault(x => x.Name == "first").ScheduleProtectionLabs.OrderBy(x => x.Date)
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
                                    if (mark != 1)
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
                        LabId = e.Id,
                        SubjectId = e.SubjectId,
                        ScheduleProtectionLabsRecomend = subGroups.Any() ?
							subGroups.FirstOrDefault(x => x.Name == "second")
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
                                    if (mark != 1)
                                    {
                                        mark -= 1;
                                    }
                                }
                            }
                        }
                    }

					//second subGroupLabs
					var labsThirdSubGroup = labsData.Select(e => new LabsViewData
					{
						Theme = e.Theme,
						Order = e.Order,
						Duration = e.Duration,
						ShortName = e.ShortName,
						LabId = e.Id,
						SubjectId = e.SubjectId,
						ScheduleProtectionLabsRecomend = subGroups.Any() ?
							subGroups.FirstOrDefault(x => x.Name == "third")
							.ScheduleProtectionLabs.OrderBy(x => x.Date)
							.Select(x => new ScheduleProtectionLab { ScheduleProtectionId = x.Id, Mark = string.Empty })
							.ToList() : new List<ScheduleProtectionLab>()
					}).ToList();
					durationCount = 0;
					foreach (var lab in labsThirdSubGroup)
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
									  Students = group.Students.Where(e => e.Confirmed == null || e.Confirmed.Value).OrderBy(e => e.LastName).Select(e => new StudentsViewData(TestPassingService.GetStidentResults(subjectIntId, e.User.Id), e, null, scheduleProtectionPracticals, null, practicalsData)).ToList(),
                                      SubGroupsOne = subGroups.Any() ? new SubGroupsViewData
                                                         {
                                                             GroupId = group.Id,
                                                             Name = "Подгруппа 1",
                                                             Labs = labsFirstSubGroup,
															 ScheduleProtectionLabs = subGroups.FirstOrDefault(x => x.Name == "first").ScheduleProtectionLabs.OrderBy(e => e.Date).Select(e => new ScheduleProtectionLabsViewData(e)).ToList(),
															 SubGroupId = subGroups.FirstOrDefault(x => x.Name == "first").Id,
															 Students = subGroups.FirstOrDefault(x => x.Name == "first").SubjectStudents.Where(e => e.Student.Confirmed == null || e.Student.Confirmed.Value).OrderBy(e => e.Student.LastName).Select(e => new StudentsViewData(TestPassingService.GetStidentResults(subjectIntId, e.StudentId), e.Student, subGroups.FirstOrDefault(x => x.Name == "first").ScheduleProtectionLabs.OrderBy(x => x.Date).ToList(), null, labsData)).ToList()
                                                         }
                                                         : null,
                                      SubGroupsTwo = subGroups.Any() ? new SubGroupsViewData
                                                          {
                                                              GroupId = group.Id,
                                                              Name = "Подгруппа 2",
                                                              Labs = labsSecondSubGroup,
															  ScheduleProtectionLabs = subGroups.FirstOrDefault(x => x.Name == "second").ScheduleProtectionLabs.OrderBy(e => e.Date).Select(e => new ScheduleProtectionLabsViewData(e)).ToList(),
															  SubGroupId = subGroups.FirstOrDefault(x => x.Name == "second").Id,
															  Students = subGroups.FirstOrDefault(x => x.Name == "second").SubjectStudents.Where(e => e.Student.Confirmed == null || e.Student.Confirmed.Value).OrderBy(e => e.Student.LastName).Select(e => new StudentsViewData(TestPassingService.GetStidentResults(subjectIntId, e.StudentId), e.Student, subGroups.FirstOrDefault(x => x.Name == "second").ScheduleProtectionLabs.OrderBy(x => x.Date).ToList(), null, labsData)).ToList()
                                                          }
                                                          : null,
									  SubGroupsThird = subGroups.Any() ? new SubGroupsViewData
																		{
																			GroupId = group.Id,
																			Name = "Подгруппа 3",
																			Labs = labsThirdSubGroup,
																			ScheduleProtectionLabs = subGroups.FirstOrDefault(x => x.Name == "third").ScheduleProtectionLabs.OrderBy(e => e.Date).Select(e => new ScheduleProtectionLabsViewData(e)).ToList(),
																			SubGroupId = subGroups.FirstOrDefault(x => x.Name == "third").Id,
																			Students = subGroups.FirstOrDefault(x => x.Name == "third").SubjectStudents.Where(e => e.Student.Confirmed == null || e.Student.Confirmed.Value).OrderBy(e => e.Student.LastName).Select(e => new StudentsViewData(TestPassingService.GetStidentResults(subjectIntId, e.StudentId), e.Student, subGroups.FirstOrDefault(x => x.Name == "third").ScheduleProtectionLabs.OrderBy(x => x.Date).ToList(), null, labsData)).ToList()
																		}
														: null

                                  });
                }

	            if (!isPractModule)
	            {
					foreach (var groupsViewData in model)
					{
						foreach (var student in groupsViewData.Students.Where(e => e.Confirmed == null || e.Confirmed.Value))
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

        public LectorResult GetLecturers()
        {
            var lecturers = LecturerManagementService.GetLecturers(e => e.LastName, lite: true)
	            .Select(e => new LectorViewData(e))
	            .ToList();
            return new LectorResult
            {
                Lectors = lecturers
            };
        }

        protected int CurrentUserId => WebSecurity.CurrentUserId;
	}
}
