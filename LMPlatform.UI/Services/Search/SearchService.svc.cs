using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;
using Application.Core;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.LecturerManagement;
using Application.Infrastructure.ProjectManagement;
using Application.Infrastructure.StudentManagement;
using Application.SearchEngine.SearchMethods;
using LMPlatform.Models;

namespace LMPlatform.UI.Services.Search
{
    public class SearchService : ISearchService
    {
        private readonly LazyDependency<IStudentManagementService> _studentRepository = new LazyDependency<IStudentManagementService>();
        private readonly LazyDependency<IGroupManagementService>  _groupRepository = new LazyDependency<IGroupManagementService>();
        private readonly LazyDependency<ILecturerManagementService> _lecturerRepository = new LazyDependency<ILecturerManagementService>();
        private readonly LazyDependency<IProjectManagementService> _projectRepository = new LazyDependency<IProjectManagementService>();
        
        public Stream SearchStudents(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;        

            var searchMethod = new StudentSearchMethod();

            if (!searchMethod.IsIndexExist())
                searchMethod.AddToIndex(_studentRepository.Value.GetStudents());

            var searchResult = searchMethod.Search(text);

            var data = new Dictionary<string, IEnumerable<Student>> { { "student", searchResult } };
            var result = new Dictionary<string, Dictionary<string, IEnumerable<Student>>> { { "data", data } };

            return GetResultStream(result);
        }

        public Stream SearchProjects(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            var searchMethod = new ProjectSearchMethod();

            if(!searchMethod.IsIndexExist())
                searchMethod.AddToIndex(_projectRepository.Value.GetProjects());

            var searchResult = searchMethod.Search(text);

            var data = new Dictionary<string, IEnumerable<Project>> { { "project", searchResult } };
            var result = new Dictionary<string, Dictionary<string, IEnumerable<Project>>> { { "data", data } };

            return GetResultStream(result);
        }

        public Stream SearchGroups(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            var searchMethod = new GroupSearchMethod();

            if(!searchMethod.IsIndexExist())
                searchMethod.AddToIndex(_groupRepository.Value.GetGroups());

            var searchResult = searchMethod.Search(text);

            var data = new Dictionary<string, IEnumerable<Group>> { { "group", searchResult } };
            var result = new Dictionary<string, Dictionary<string, IEnumerable<Group>>> { { "data", data } };

            return GetResultStream(result);
        }

        public Stream SearchLecturers(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            var searchMethod = new LecturerSearchMethod();

            if(!searchMethod.IsIndexExist())
                searchMethod.AddToIndex(_lecturerRepository.Value.GetLecturers());

            var searchResult = searchMethod.Search(text);

            var data = new Dictionary<string, IEnumerable<Lecturer>> { { "lecturer", searchResult } };
            var result = new Dictionary<string, Dictionary<string, IEnumerable<Lecturer>>> { { "data", data } };

            return GetResultStream(result);
        }

        private Stream GetResultStream(object obj)
        {
            var jsonClient = new JavaScriptSerializer().Serialize(obj);
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            return new MemoryStream(Encoding.UTF8.GetBytes(jsonClient));
        }
    }   
}
