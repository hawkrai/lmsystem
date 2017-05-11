using Application.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Application.Infrastructure.WatchingTimeManagement;
using LMPlatform.Models;
using System.Web;
using WebMatrix.WebData;
using Application.Infrastructure.ConceptManagement;
using System.Runtime.Serialization;
using LMPlatform.UI.ViewModels.ComplexMaterialsViewModel;
using iTextSharp.text.pdf;
using Application.Infrastructure.FilesManagement;
using System.IO;
using System.Configuration;
using WMPLib;
using Application.Infrastructure.StudentManagement;

namespace LMPlatform.UI.ApiControllers
{
    public class ConceptResult
    {
        private static readonly LazyDependency<IWatchingTimeService> _watchingTimeService = new LazyDependency<IWatchingTimeService>();

        public static IWatchingTimeService WatchingTimeService
        {
            get
            {
                return _watchingTimeService.Value;
            }
        }

        public string Name;
        public List<ConceptResult> Children;
        public bool IsFile;
        public int? ViewTime;
        public int? Estimated;
        public int ConceptId;

        public ConceptResult()
        {
        }

        public static ConceptResult GetConceptResultTreeInViews(Concept concept, List<WatchingTimeResult> views)
        {
            if (concept == null || ((concept.Children == null || concept.Children.Count == 0) && !views.Any(x => x.ConceptId == concept.Id)))
                return null;

            var currentConcept = new ConceptResult()
            {
                Name = concept.Name,
                IsFile = true
            };

            if (concept.Children != null && concept.Children.Count != 0)
            {
                currentConcept.Children = new List<ConceptResult>();
                foreach (var child in concept.Children)
                {
                    var childConceptResult = GetConceptResultTreeInViews(child, views);
                    if (childConceptResult != null)
                        currentConcept.Children.Add(childConceptResult);
                }
                if (currentConcept.Children.Count == 0)
                    return null;
                currentConcept.IsFile = false;
            }
            else
            {
                var viewInfo = views.Find(x => x.ConceptId == concept.Id).Views;
                if(viewInfo == null)
                {
                    currentConcept.ViewTime = null;
                }
                else
                {
                    currentConcept.ViewTime = viewInfo.Time;
                }
                currentConcept.Estimated = WatchingTimeService.GetEstimatedTime(concept.Container);
            }
            currentConcept.ConceptId = concept.Id;
            return currentConcept;
        }
    }

    public class WatchingTimeResult
    {
        public int ConceptId;
        public int? ParentId;
        public string Name { get; set; }
        public WatchingTime Views;
        public int Estimated { get; set; }
    }

    public class StudentInfoResult
    {
        public string GroupNumber;
        public string StudentFullName;
        public List<ConceptResult> Tree;
    }

    public class WatchingTimeController : ApiController
    {
        private readonly LazyDependency<IStudentManagementService> _studentManagementService = new LazyDependency<IStudentManagementService>();
        private readonly LazyDependency<IWatchingTimeService> _watchingTimeService = new LazyDependency<IWatchingTimeService>();
        private readonly LazyDependency<IConceptManagementService> _conceptManagementService = new LazyDependency<IConceptManagementService>();


        public IStudentManagementService StudentManagementService
        {
            get { return _studentManagementService.Value; }
        }

        public IWatchingTimeService WatchingTimeService
        {
            get
            {
                return _watchingTimeService.Value;
            }
        }

        public IConceptManagementService ConceptManagementService
        {
            get
            {
                return _conceptManagementService.Value;
            }
        }

        private int? GetConceptParentId(Concept concept)
        {
            var temp = concept;
            while (temp.Parent != null)
            {
                temp = temp.Parent;
            }
            return temp.Id;
        }

        // GET api/<controller>/5
        public StudentInfoResult Get(int id)
        {
            int rootId = -1;
            int studentId = -1;
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);
            if (queryParams.ContainsKey("root"))
            {
                Int32.TryParse(queryParams["root"], out rootId);
            }
            if (queryParams.ContainsKey("studentId"))
            {
                Int32.TryParse(queryParams["studentId"], out studentId);
            }
            var studentInfo = StudentManagementService.GetStudent(studentId);
            var concepts = ConceptManagementService.GetElementsBySubjectId(id).Where(x => x.Container != null).ToList();
            List<WatchingTimeResult> viewsResult = new List<WatchingTimeResult>();
            foreach (var concept in concepts)
            {
                if (GetConceptParentId(concept) != rootId)
                    continue;
                var views = WatchingTimeService.GetAllRecords(concept.Id, studentId).FirstOrDefault();
                viewsResult.Add(new WatchingTimeResult()
                {
                    ConceptId = concept.Id,
                    ParentId = concept.ParentId,
                    Name = concept.Name,
                    Views = views,
                    Estimated = WatchingTimeService.GetEstimatedTime(concept.Container)
                });
            }
            var tree = ConceptManagementService.GetTreeConceptByElementId(rootId);
            var treeresult = ConceptResult.GetConceptResultTreeInViews(tree, viewsResult);
            var result = new StudentInfoResult()
            {
                Tree = treeresult.Children,
                StudentFullName = studentInfo.FullName,
                GroupNumber = studentInfo.Group.Name,
            };

            return result;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
            int userId = WebSecurity.GetUserId(HttpContext.Current.User.Identity.Name);
            Concept concept = ConceptManagementService.GetById(id);
            WatchingTimeService.SaveWatchingTime(new WatchingTime(userId, concept.Id, 10));
            //WatchingTimeService.SaveWatchingTime(new WatchingTime(userId, concept, 10));
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}