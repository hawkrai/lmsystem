using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Web;
using System.Text;
using Application.Core;
using Application.Infrastructure.GroupManagement;
using Application.Infrastructure.LecturerManagement;
using Application.Infrastructure.ProjectManagement;
using Application.Infrastructure.StudentManagement;
using Application.SearchEngine.SearchMethods;
using LMPlatform.Models;
using Newtonsoft.Json;
using Message = System.ServiceModel.Channels.Message;

namespace LMPlatform.UI.Services.Search
{    
    public class SearchService : ISearchService
    {
        private readonly LazyDependency<IStudentManagementService> _studentRepository = new LazyDependency<IStudentManagementService>();
        private readonly LazyDependency<IGroupManagementService>  _groupRepository = new LazyDependency<IGroupManagementService>();
        private readonly LazyDependency<ILecturerManagementService> _lecturerRepository = new LazyDependency<ILecturerManagementService>();
        private readonly LazyDependency<IProjectManagementService> _projectRepository = new LazyDependency<IProjectManagementService>();

        public Message SearchStudents(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;            
            
            var searchMethod = new StudentSearchMethod();

            if (!searchMethod.IsIndexExist())
                searchMethod.AddToIndex(_studentRepository.Value.GetStudents());

            var searchResult = searchMethod.Search(text);

            var data = new Dictionary<string, IEnumerable<Student>> {{"student", searchResult}};
            var result = new Dictionary<string, Dictionary<string, IEnumerable<Student>>> { { "data", data } };            

            return GetJsonStream(result);
        }

        public Message SearchProjects(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            var searchMethod = new ProjectSearchMethod();

            if(!searchMethod.IsIndexExist())
                searchMethod.AddToIndex(_projectRepository.Value.GetProjects());

            var searchResult = searchMethod.Search(text);

            var data = new Dictionary<string, IEnumerable<Project>> { { "project", searchResult } };
            var result = new Dictionary<string, Dictionary<string, IEnumerable<Project>>> { { "data", data } };

            return GetJsonStream(result);
        }

        public Message SearchGroups(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            var searchMethod = new GroupSearchMethod();

            if(!searchMethod.IsIndexExist())
                searchMethod.AddToIndex(_groupRepository.Value.GetGroups());

            var searchResult = searchMethod.Search(text);

            var data = new Dictionary<string, IEnumerable<Group>> { { "group", searchResult } };
            var result = new Dictionary<string, Dictionary<string, IEnumerable<Group>>> { { "data", data } };

            return GetJsonStream(result);
        }

        public Message SearchLecturers(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;            

            var searchMethod = new LecturerSearchMethod();

            if(!searchMethod.IsIndexExist())
                searchMethod.AddToIndex(_lecturerRepository.Value.GetLecturers());                    

            var searchResult = searchMethod.Search(text);

            var data = new Dictionary<string, IEnumerable<Lecturer>> { { "lecturer", searchResult } };
            var result = new Dictionary<string, Dictionary<string, IEnumerable<Lecturer>>> { { "data", data } };

            return GetJsonStream(result);
        }

        public Message GetJsonStream(object obj)
        {
            string jsonSerialized = JsonConvert.SerializeObject(obj);            
            MemoryStream memoryStream = new MemoryStream(new UTF8Encoding().GetBytes(jsonSerialized));            
            memoryStream.Position = 0;            
            return WebOperationContext.Current.CreateStreamResponse(memoryStream, "application/json");
        }
    }
}
