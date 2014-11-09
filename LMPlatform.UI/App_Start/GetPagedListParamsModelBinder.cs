using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Script.Serialization;
using Application.Core.Data;
using Application.Core.Extensions;

namespace LMPlatform.UI
{
    public class GetPagedListParamsModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var nvc = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);
            var model = new GetPagedListParams
            {
                PageSize = int.Parse(nvc["count"]),
                Page = int.Parse(nvc["page"]),
                SortExpression = "Id"
            };

            var sortingKey = nvc.AllKeys.FirstOrDefault(x => x.StartsWith("sorting["));
            if (sortingKey != null)
            {
                model.SortExpression = sortingKey.RemoveStringEntries("sorting[", "]") + " " + nvc[sortingKey];
            }

            model.Filters = nvc.AllKeys.Where(x => x.StartsWith("filter["))
                .ToDictionary(x => x.RemoveStringEntries("filter[", "]"), y => nvc[y]);

            if (nvc.AllKeys.Contains("filter"))
            {
                model.Filters = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(nvc["filter"]);
            }

            bindingContext.Model = model;
            return true;
        }
    }
}