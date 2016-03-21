using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace LMPlatform.UI.Services.Parental
{
    using Application.Core;
    using Application.Infrastructure.GroupManagement;
    using Application.Infrastructure.SubjectManagement;

    using LMPlatform.UI.Services.Modules;
    using LMPlatform.UI.Services.Modules.Parental;

    using WebMatrix.WebData;

    public class ParentalService : IParentalService
    {
        private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();
        private readonly LazyDependency<IGroupManagementService> groupManagementService = new LazyDependency<IGroupManagementService>();

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
            catch (Exception)
            {
                return new SubjectListResult
                {
                    Message = "Произошла ошибка при получении данных",
                    Code = "500"
                };
            }
        }
    }
}
