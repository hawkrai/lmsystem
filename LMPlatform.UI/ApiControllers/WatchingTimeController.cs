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

namespace LMPlatform.UI.ApiControllers
{

    public class WatchingTimeResult
    {
        public string Name { get; set; }
        public List<WatchingTime> Views;
        public int Estimated { get; set; }
    }

    public class WatchingTimeController : ApiController
    {
        private readonly LazyDependency<IWatchingTimeService> _watchingTimeService = new LazyDependency<IWatchingTimeService>();
        private readonly LazyDependency<IConceptManagementService> _conceptManagementService = new LazyDependency<IConceptManagementService>();
        private readonly LazyDependency<IFilesManagementService> _filesManagementService =
            new LazyDependency<IFilesManagementService>();

        public IFilesManagementService FilesManagementService
        {
            get { return _filesManagementService.Value; }
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
            while(temp.Parent != null)
            {
                temp = temp.Parent;
            }
            return temp.Id;
        }

        // GET api/<controller>/5
        public List<WatchingTimeResult> Get(int id)
        {
            int rootId = -1;
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);
            if (queryParams.ContainsKey("root"))
            {
                Int32.TryParse(queryParams["root"], out rootId);
            }
            var concepts = ConceptManagementService.GetElementsBySubjectId(id).Where(x => x.Container != null).ToList();
            List<WatchingTimeResult> result = new List<WatchingTimeResult>();
            foreach(var concept in concepts)
            {
                if (GetConceptParentId(concept) != rootId)
                    continue;
                var views = WatchingTimeService.GetAllRecords(concept.Id);
                //views.ForEach(x => x.Concept = null);
                result.Add(new WatchingTimeResult()
                {
                    Name = concept.Name,
                    Views = views,
                    Estimated = GetEstimatedTime(concept.Container)
                });
            }

            return result;
        }

        private int GetEstimatedTime(string container)
        {
            var attachments = FilesManagementService.GetAttachments(container);
            if (attachments.Count == 0)
                return 0;
            string path = ConfigurationManager.AppSettings["FileUploadPath"] + attachments[0].PathName + "\\" + attachments[0].FileName;
            if (!File.Exists(path))
                return 0;
            try
            {
                PdfReader pdfReader = new PdfReader(path);
                int numberOfPages = pdfReader.NumberOfPages;
                return numberOfPages * 30; //30 сек страница временно тут
            } catch
            {
                var player = new WindowsMediaPlayer();
                var clip = player.newMedia(path);
                return (int)clip.duration;
            }
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