using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LMPlatform.UI
{
    using System.ServiceModel.Activation;

    using LMPlatform.UI.Services.News;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Views/{file}.html");

			routes.MapRoute(
				name: "Profile",
				url: "Lms/{userLogin}",
				defaults: new { controller = "Lms", action = "Index"});

			//routes.MapRoute(
			//	name: "ProfilePage",
			//	url: "Profile/Page/{userName}",
			//	defaults: new { controller = "Profile", action = "Page" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}