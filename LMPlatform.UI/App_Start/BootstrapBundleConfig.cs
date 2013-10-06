using System.Web.Optimization;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(LMPlatform.UI.App_Start.BootstrapBundleConfig), "RegisterBundles")]

namespace LMPlatform.UI.App_Start
{
	public class BootstrapBundleConfig
	{
		public static void RegisterBundles()
		{
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js"));

            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/bootstrapcontrols").Include("~/Scripts/bootstrap-datepicker.js"));

            BundleTable.Bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/bootstrap.css","~/Content/bootstrap-responsive.css"));

            BundleTable.Bundles.Add(new StyleBundle("~/Content/bootstrapcontrols").Include("~/Content/bootstrap-datepicker.css"));


		}
	}
}
