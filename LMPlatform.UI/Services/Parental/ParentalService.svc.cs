using System.Linq;
using Application.Core;
using Application.Infrastructure.SubjectManagement;
using LMPlatform.UI.Services.Modules.Parental;

namespace LMPlatform.UI.Services.Parental
{
	public class ParentalService : IParentalService
    {
        private readonly LazyDependency<ISubjectManagementService> subjectManagementService = new LazyDependency<ISubjectManagementService>();
        public ISubjectManagementService SubjectManagementService => subjectManagementService.Value;

        public SubjectListResult GetGroupSubjects(string groupId)
        {
            try
            {
                var group = int.Parse(groupId);
                var model = SubjectManagementService.GetGroupSubjectsLite(group); // lite

                var result = new SubjectListResult
                {
                    Subjects = model.Select(e => new SubjectViewData(e)).ToList(),
                    Message = "Данные успешно загружены",
                    Code = "200"
                };

                return result;
            }
            catch
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
