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

namespace LMPlatform.UI.ApiControllers
{

    public class WatchingTimeResult
    {
        public string Name { get; set; }
        public List<WatchingTime> Views;
    }

    public class WatchingTimeController : ApiController
    {
        private readonly LazyDependency<IWatchingTimeService> _watchingTimeService = new LazyDependency<IWatchingTimeService>();
        private readonly LazyDependency<IConceptManagementService> _conceptManagementService = new LazyDependency<IConceptManagementService>();
        
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

        // GET api/<controller>/5
        public List<WatchingTimeResult> Get(int id)
        {
            var concepts = ConceptManagementService.GetElementsBySubjectId(id).Where(x => x.Container != null).ToList();
            List<WatchingTimeResult> result = new List<WatchingTimeResult>();
            foreach(var concept in concepts)
            {
                var views = WatchingTimeService.GetAllRecords(concept.Id);
                //views.ForEach(x => x.Concept = null);
                result.Add(new WatchingTimeResult()
                {
                    Name = concept.Name,
                    Views = views
                });
            }
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