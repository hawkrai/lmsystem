using Application.Core;
using Application.Infrastructure.DPManagement;
using Application.Infrastructure.DTO;
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

namespace LMPlatform.UI.ApiControllers.DP
{
    public class DiplomProjectNewsController : ApiController
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private readonly LazyDependency<IDpManagementService> _dpManagementService = new LazyDependency<IDpManagementService>();

        private IDpManagementService DpManagementService
        {
            get { return _dpManagementService.Value; }
        }

        [HttpGet]
        public List<NewsData> Get()
        {
            return DpManagementService.GetNewses(WebSecurity.CurrentUserId);
        }

        [System.Web.Http.HttpDelete]
        public System.Web.Mvc.JsonResult Delete([FromBody]DiplomProjectNews deleteData)
        {
            try
            {
                DpManagementService.DeleteNews(deleteData);
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
