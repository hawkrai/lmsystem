using System.Web.Optimization;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(LMPlatform.UI.App_Start.BootstrapBundleConfig), "RegisterBundles")]

namespace LMPlatform.UI.App_Start
{
	public class BootstrapBundleConfig
	{
		public static void RegisterBundles()
		{
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js", "~/Scripts/bootstrap-multiselect.js"));

            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/bootstrapcontrols").Include(
                "~/Scripts/bootstrap-datepicker.js",
                "~/Scripts/alertify.js",
                "~/Scripts/wysihtml5-0.3.0.js",
                "~/Scripts/prettify.js",
                "~/Scripts/bootstrap-wysihtml5.js"));

            BundleTable.Bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/bootstrap.css"));

            BundleTable.Bundles.Add(new StyleBundle("~/Content/bootstrapcontrols").Include(
                "~/Content/datepicker.css",
                "~/Content/alertify.default.css",
                "~/Content/alertify.core.css",
                "~/Content/prettify.css",
                "~/Content/bootstrap-wysihtml5.css",
                "~/Content/wysiwyg-color.css"));
		}
	}
}
