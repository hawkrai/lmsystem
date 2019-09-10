using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using Application.Core.Data;

namespace LMPlatform.UI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{action}");

			var provider = new SimpleModelBinderProvider(
            typeof(GetPagedListParams), new GetPagedListParamsModelBinder());
            config.Services.Insert(typeof(ModelBinderProvider), 0, provider);
        }
    }
}
