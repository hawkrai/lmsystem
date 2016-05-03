using Application.Core;
using Application.Infrastructure.CPManagement;
using Application.Infrastructure.CTO;
using LMPlatform.Models;
using LMPlatform.UI.Services.Modules;
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

        [System.Web.Http.HttpPost]
        public System.Web.Mvc.JsonResult Save([FromBody]CourseProjectNews model)
        {
            try
            {
                model.EditDate = DateTime.Now;
                CpManagementService.SaveNews(model);
                return new System.Web.Mvc.JsonResult()
                {
                    Data = new
                    {
                        Message = "Новость успешно сохранена",
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
                        Message = "Произошла ошибка при сохранении новости",
                        Error = true
                    }
                };
            }
        }



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
