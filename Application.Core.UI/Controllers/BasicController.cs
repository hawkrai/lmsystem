using System;
using System.IO;
using System.Web.Mvc;

namespace Application.Core.UI.Controllers
{
    public abstract class BasicController : Controller
    {
		public TService ApplicationService<TService>()
		{
			return UnityWrapper.Resolve<TService>();
		}

		public string PartialViewToString(string viewName, object model)
		{
			if (string.IsNullOrEmpty(viewName))
			{
				viewName = ControllerContext.RouteData.GetRequiredString("action");
			}

			ViewData.Model = model;

			using (var stringWriter = new StringWriter())
			{
				var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
				var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, stringWriter);
				viewResult.View.Render(viewContext, stringWriter);
				return stringWriter.GetStringBuilder().ToString();
			}
		}
        public string PartialViewToString(string viewName, object model, bool active)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }

            ViewData.Model = model;


            using (var stringWriter = new StringWriter())
            {
                if (active)
                {
                    var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                    var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData,
                        stringWriter);
                    viewResult.View.Render(viewContext, stringWriter);
                }

                return stringWriter.GetStringBuilder().ToString();
            }
        }

        protected TResult Execute<TResult>(Func<TResult> action)
		{
			var result = action();

			return result;
		}
    }
}
