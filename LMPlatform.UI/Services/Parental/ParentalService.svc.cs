using System;
using System.Linq;

namespace LMPlatform.UI.Services.Parental
{
    using Application.Core;
    using Application.Core.Data;
    using Application.Infrastructure.GroupManagement;
    using Application.Infrastructure.KnowledgeTestsManagement;
    using Application.Infrastructure.SubjectManagement;
    using LMPlatform.Data.Repositories;
    using LMPlatform.Models;
    using LMPlatform.UI.Services.Modules.Parental;
    using LMPlatform.UI.Services.Parental.Models;

    public class ParentalService : IParentalService
    {
        private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();
        private readonly LazyDependency<IGroupManagementService> groupManagementService = new LazyDependency<IGroupManagementService>();
        private readonly LazyDependency<ITestPassingService> _testPassingService = new LazyDependency<ITestPassingService>();

        public IGroupManagementService GroupManagementService
        {
            get
            {
                return groupManagementService.Value;
            }
        }

        public ISubjectManagementService SubjectManagementService
        {
            get
            {
                return subjectManagementService.Value;
            }
        }

        public ITestPassingService TestPassingService
        {
            get
            {
                return _testPassingService.Value;
            }
        }

        public SubjectListResult GetGroupSubjects(string groupId)
        {
            try
            {
                var group = int.Parse(groupId);
                var model = SubjectManagementService.GetGroupSubjects(group);

                var result = new SubjectListResult
                {
                    Subjects = model.Select(e => new SubjectViewData(e)).ToList(),
                    Message = "Данные успешно загружены",
                    Code = "200"
                };

                return result;
            }
            catch (Exception e)
            {
                return new SubjectListResult
                {
                    Message = "Произошла ошибка при получении данных",
                    Code = "500"
                };
            }
        }

        public ParentalResult LoadGroup(string groupId)
        {
            try
            {
                using (var repositoriesContainer = new LmPlatformRepositoriesContainer())
                {
                    var id = int.Parse(groupId);
                    var group = repositoriesContainer.GroupsRepository.GetBy(new Query<Group>(e => e.Id == id)
                                                                    .Include(e => e.Students.Select(x => x.LecturesVisitMarks.Select(t => t.LecturesScheduleVisiting)))
                                                                    .Include(e => e.Students.Select(x => x.ScheduleProtectionLabMarks.Select(t => t.ScheduleProtectionLab).Select(f => f.SubGroup).Select(s => s.SubjectGroup)))
                                                                    .Include(e => e.Students.Select(x => x.StudentLabMarks.Select(t => t.Lab))));
                    var subjects = SubjectManagementService.GetGroupSubjects(id);
                    var students = group.Students.ToList();
                    students.Sort((arg1, arg2) => { return string.Compare(arg1.FullName, arg2.FullName); });



                    return new ParentalResult
                    {
                        Students = students.Select(e => new ParentalUser(e,subjects)).ToList(),
                        GroupName = group.Name,
                        Message = "Ok",
                        Code = "200"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ParentalResult
                {
                    Message = "Произошла ошибка при получении данных",
                    Code = "500"
                };
            }

        }
    }
}
