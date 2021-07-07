﻿using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LMPlatform.UI.App_Start;
using LMPlatform.UI.Helpers.TaskScheduler;

namespace LMPlatform.UI
{
    using Application.Infrastructure.AccountManagement;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            SimpleMembershipInitializer.Initialize();
            AreaRegistration.RegisterAllAreas();
			//MappingConfig.Initialize();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Bootstrapper.Initialize();
            TaskScheduler.Start();
        }
    }
}