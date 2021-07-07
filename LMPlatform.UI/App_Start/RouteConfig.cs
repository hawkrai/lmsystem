using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace LMPlatform.UI
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("Views/{file}.html");

			routes.MapRoute(
				"Profile",
				"Lms/{userLogin}",
				new {controller = "Lms", action = "Index"});

			routes.MapRoute(
				"ProfilePage",
				"Profile/Page/{userLogin}",
				new {controller = "Profile", action = "Page"});

			if (bool.Parse(ConfigurationManager.AppSettings["jwt:enabled"]))
			{
				routes.MapRoute(
					"JwtLogin",
					"Account/Login",
					defaults: new { controller = "Account", action = "LoginJWT" },
					constraints: new { httpMethod = new HttpMethodConstraint("POST") });
			}

			routes.MapRoute(
				"Default",
				"{controller}/{action}/{id}",
				new {controller = "Home", action = "Index", id = UrlParameter.Optional});
		}
	}
}