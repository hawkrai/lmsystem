using System.Web;
using System.Web.Optimization;

namespace LMPlatform.UI.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*", 
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/shared").Include(
                "~/Scripts/application/shared.js", 
                "~/Scripts/application/jQueryExtensions.js",
                "~/Scripts/application/masterPageManagement.js",
                "~/Scripts/spin.js",
                "~/Scripts/application/spinFunction.js"));

            bundles.Add(new ScriptBundle("~/bundle/subjectManagement").Include(
                "~/Scripts/application/subjectManagement.js"));

            bundles.Add(new ScriptBundle("~/bundle/subjectWorking").Include(
                "~/Scripts/application/subjectWorking.js"));

            bundles.Add(new ScriptBundle("~/bundle/accountManagement").Include(
                "~/Scripts/application/accountManagement.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                "~/Scripts/DataTables-1.9.4/media/js/jquery.dataTables.js", 
                "~/Scripts/application/datatable-bootstrappagination.js", 
                "~/Scripts/application/dataTables.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockoutBundles").Include(
                "~/Scripts/knockout-3.0.0.js",
                "~/Scripts/knockoutWrapper.js",
                "~/Scripts/knockout.mapping-latest.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.js",
                "~/Scripts/angular-route.js",
                "~/Scripts/ng-table.js",
                "~/Scripts/ng-table.js",
                "~/Scripts/angular-bootstrap-duallistbox.js",
                "~/Scripts/ui-bootstrap-tpls-0.10.0.js",
                "~/Scripts/xeditable.js"));

            bundles.Add(new ScriptBundle("~/bundles/fileupload").Include(
                "~/Scripts/mvcfileupload/vendor/jquery.ui.widget.js",
                "~/Scripts/mvcfileupload/tmpl.js",
                "~/Scripts/mvcfileupload/load-image.js",
                "~/Scripts/mvcfileupload/canvas-to-blob.js",
                "~/Scripts/mvcfileupload/jquery.iframe-transport.js",
                "~/Scripts/mvcfileupload/jquery.fileupload.js",
                "~/Scripts/mvcfileupload/jquery.fileupload-fp.js",
                "~/Scripts/mvcfileupload/jquery.fileupload-ui.js",
                "~/Scripts/mvcfileupload/main.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/bootbox").Include("~/Scripts/bootbox.js", "~/Scripts/bootbox.min.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css", "~/Content/font-awesome/font-awesome.css"));
            bundles.Add(new StyleBundle("~/Content/angular").Include("~/Content/xeditable.css"));
            bundles.Add(new StyleBundle("~/fileuploader/css").Include("~/Content/mvcfileupload/jquery.fileupload-bui.css"));
            bundles.Add(new StyleBundle("~/admin-style/css").Include(
                "~/Content/admin-icons.css",
                "~/Content/admin.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                "~/Content/themes/base/jquery.ui.core.css", 
                "~/Content/themes/base/jquery.ui.resizable.css", 
                "~/Content/themes/base/jquery.ui.selectable.css", 
                "~/Content/themes/base/jquery.ui.accordion.css", 
                "~/Content/themes/base/jquery.ui.autocomplete.css", 
                "~/Content/themes/base/jquery.ui.button.css", 
                "~/Content/themes/base/jquery.ui.dialog.css", 
                "~/Content/themes/base/jquery.ui.slider.css", 
                "~/Content/themes/base/jquery.ui.tabs.css", 
                "~/Content/themes/base/jquery.ui.datepicker.css", 
                "~/Content/themes/base/jquery.ui.progressbar.css", 
                "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new ScriptBundle("~/bundles/dpModule").Include(
                "~/Scripts/application/DP/dpModule.js",
                "~/Scripts/application/DP/controllers/homeController.js",
                "~/Scripts/application/DP/controllers/projectsController.js",
                "~/Scripts/application/DP/controllers/projectController.js",
                "~/Scripts/application/DP/controllers/studentsController.js",
                "~/Scripts/application/DP/controllers/taskSheetController.js",
                "~/Scripts/application/DP/services/projectService.js"));
        }
    }
}