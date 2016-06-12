using Application.Core;
using Application.Infrastructure.CPManagement;
using Application.Infrastructure.CTO;
using LMPlatform.Models;
using LMPlatform.UI.Services.Modules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebMatrix.WebData;

namespace LMPlatform.UI.ApiControllers.CP
{
    public class CourseProjectNewsController : ApiController
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private readonly LazyDependency<ICPManagementService> _cpManagementService = new LazyDependency<ICPManagementService>();

        private ICPManagementService CpManagementService
        {
            get { return _cpManagementService.Value; }
        }

        [HttpGet]
        public List<NewsData> Get(int id)
        {
            return CpManagementService.GetNewses(WebSecurity.CurrentUserId, id);
        }

        //[System.Web.Http.HttpPost]
        //public System.Web.Mvc.JsonResult Save(string subjectId, string id, string title, string body, string disabled,
        //    string isOldDate, string pathFile, string attachments)
        //{
        //    var attachmentsModel = JsonConvert.DeserializeObject<List<Attachment>>(attachments).ToList();
        //    var subject = int.Parse(subjectId);
        //    try
        //    {
        //        CpManagementService.SaveNews(new Models.CourseProjectNews
        //        {
        //            SubjectId = subject,
        //            Id = int.Parse(id),
        //            Attachments = pathFile,
        //            Title = title,
        //            Body = body,
        //            Disabled = bool.Parse(disabled),
        //            EditDate = DateTime.Now,
        //        }, attachmentsModel, WebSecurity.CurrentUserId);
        //        return new System.Web.Mvc.JsonResult()
        //        {
        //            Data = new
        //            {
        //                Message = "Новость успешно сохранена",
        //                Error = false
        //            }
        //        };
        //    }
        //    catch (Exception)
        //    {
        //        return new System.Web.Mvc.JsonResult()
        //        {
        //            Data = new
        //            {
        //                Message = "Произошла ошибка при сохранении новости",
        //                Error = true
        //            }
        //        };
        //    }
        //}



        [System.Web.Http.HttpDelete]
        public System.Web.Mvc.JsonResult Delete([FromBody]CourseProjectNews deleteData)
        {
            try
            {
                
                CpManagementService.DeleteNews(deleteData);
                return new System.Web.Mvc.JsonResult()
                {
                    Data = new
                    {
                        Message = "Объявление успешно удалено",
                        Error = false
                    }
                };
            }
            catch (Exception)
            {
                return new System.Web.Mvc.JsonResult()
                {
                    Data = new
                    {
                        Message = "Произошла ошибка при удалении новости",
                        Error = true
                    }
                };
            }
        }
    }
}
