using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LMPlatform.UI.Services
{
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
                    GroupManagementService.GetGroups(new Query<Group>(e => e.SubjectGroups.Any(x => x.SubjectId == id)).Include(e => e.Students))
                        .ToList();

                var model = new List<GroupsViewData>();

                foreach (Group group in groups)
                {
                    IList<SubGroup> subGroups = this.SubjectManagementService.GetSubGroups(id, group.Id);
                    model.Add(new GroupsViewData
                                  {
                                      GroupId = group.Id, 
                                      GroupName = group.Name, 
                                      Students = group.Students.Select(e => new StudentsViewData(e)).ToList(),
                                      SubGroupsOne = subGroups.Any() ? subGroups.FirstOrDefault().SubjectStudents.Select(e => new StudentsViewData(e.Student)).ToList() : new List<StudentsViewData>(),
                                      SubGroupsTwo = subGroups.Any() ? subGroups.LastOrDefault().SubjectStudents.Select(e => new StudentsViewData(e.Student)).ToList() : new List<StudentsViewData>()
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
