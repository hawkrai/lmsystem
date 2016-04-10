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
                "~/Scripts/spin.min.js",
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
                "~/Scripts/angular-locale_ru-ru.js",
                "~/Scripts/angular-route.js",
                "~/Scripts/angular-resource.js",
                "~/Scripts/ng-table.js",
                "~/Scripts/angular-bootstrap-duallistbox.js",
                "~/Scripts/select.js",
                "~/Scripts/ui-bootstrap-tpls.js",
				"~/Scripts/spinner-angular.js",
                "~/Scripts/xeditable.js"));

            bundles.Add(new ScriptBundle("~/bundles/textAngular").Include(
                "~/Scripts/textAngular/textAngular-sanitize.js",
                "~/Scripts/textAngular/textAngular.js",
                "~/Scripts/textAngular/textAngularSetup.js"));

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

            bundles.Add(new ScriptBundle("~/bundles/ajaxChosen").Include(
                "~/Scripts/chosen/chosen.jquery.js",
                "~/Scripts/chosen/ajax-chosen.js"));

            bundles.Add(new StyleBundle("~/Content/typeahead").Include(
                "~/Content/jquery.typeahead.css"));
            bundles.Add(new ScriptBundle("~/bundles/typeahead").Include(
                "~/Scripts/jquery.typeahead.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/bootbox").Include("~/Scripts/bootbox.js", "~/Scripts/bootbox.min.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css", "~/Content/font-awesome/font-awesome.css"));
            bundles.Add(new StyleBundle("~/Content/angular").Include("~/Content/xeditable.css", "~/Content/ng-table.css", "~/Content/select.css"));
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

            bundles.Add(new ScriptBundle("~/bundles/cpModule").Include(
    "~/Scripts/application/CP/cpModule.js",
    "~/Scripts/application/CP/controllers/homeController.js",
    "~/Scripts/application/CP/controllers/projectsController.js",
    "~/Scripts/application/CP/controllers/studentsController.js",
    "~/Scripts/application/CP/controllers/projectController.js",
    "~/Scripts/application/CP/controllers/visitStatsController.js",
                    "~/Scripts/application/CP/controllers/percentagesController.js",
                "~/Scripts/application/CP/controllers/percentageController.js",
                "~/Scripts/application/CP/controllers/percentageResultsController.js",
                 "~/Scripts/application/CP/controllers/taskSheetController.js",
    "~/Scripts/application/CP/services/projectService.js"));

            bundles.Add(new ScriptBundle("~/bundles/dpModule").Include(
                "~/Scripts/application/DP/dpModule.js",
                "~/Scripts/application/DP/controllers/homeController.js",
                "~/Scripts/application/DP/controllers/projectsController.js",
                "~/Scripts/application/DP/controllers/projectController.js",
                "~/Scripts/application/DP/controllers/studentsController.js",
                "~/Scripts/application/DP/controllers/taskSheetController.js",
                "~/Scripts/application/DP/controllers/percentagesController.js",
                "~/Scripts/application/DP/controllers/percentageController.js",
                "~/Scripts/application/DP/controllers/percentageResultsController.js",
                "~/Scripts/application/DP/controllers/visitStatsController.js",
                "~/Scripts/application/DP/services/projectService.js"));

            bundles.Add(new ScriptBundle("~/bundles/materialsApp").Include(
                "~/Scripts/application/Materials/controllers/homeController.js",
                "~/Scripts/application/Materials/controllers/catalogController.js",
                "~/Scripts/application/Materials/controllers/newController.js",
                "~/Scripts/application/Materials/materialsApp.js",
                "~/Scripts/application/Materials/services/materialsService.js",
                "~/Scripts/tinymce/tinymce.min.js",
                "~/Scripts/tinymce/scrollTo.js"));

            bundles.Add(new ScriptBundle("~/bundles/complexMaterialsApp").Include(
                "~/Scripts/application/ComplexMaterials/controllers/homeController.js",
                "~/Scripts/application/ComplexMaterials/controllers/catalogController.js",
                "~/Scripts/application/ComplexMaterials/controllers/mapController.js",
                "~/Scripts/application/ComplexMaterials/complexMaterialsApp.js",
                "~/Scripts/application/ComplexMaterials/services/complexMaterialsDataService.js",
                "~/Scripts/application/ComplexMaterials/services/navigationService.js",
                "~/Scripts/tinymce/tinymce.min.js",
                "~/Scripts/pdfjs/compatibility.js",
                "~/Scripts/pdfjs/l10n.js",
                "~/Scripts/pdfjs/pdf.js",
                "~/Scripts/d3/d3.min.js",
                "~/Scripts/pdfjs/pdf.worker.js",
                "~/Scripts/tinymce/scrollTo.js"));

            bundles.Add(new ScriptBundle("~/bundles/knowledgeTesting").Include(
                "~/Scripts/linq.js",
                "~/Scripts/application/KnowledgeTesting/knowledgeTestingModule.js",
                "~/Scripts/application/KnowledgeTesting/Controllers/testsNavigationController.js",
                "~/Scripts/application/KnowledgeTesting/Controllers/testsController.js", 
                "~/Scripts/application/KnowledgeTesting/Controllers/questionsController.js",
                "~/Scripts/jqplot/jquery.jqplot.min.js",
                "~/Scripts/jqplot/jqplot.barRenderer.min.js",
                "~/Scripts/jqplot/jqplot.categoryAxisRenderer.min.js",
                "~/Scripts/jqplot/jqplot.dateAxisRenderer.min.js",
                "~/Scripts/jqplot/jqplot.canvasTextRenderer.min.js",
                "~/Scripts/jqplot/jqplot.canvasAxisTickRenderer.min.js",
                "~/Scripts/jqplot/jqplot.pointLabels.min.js",
                "~/Scripts/application/KnowledgeTesting/Controllers/passingController.js",
                "~/Scripts/application/KnowledgeTesting/Controllers/resultsController.js",
                "~/Scripts/application/KnowledgeTesting/Controllers/testDetailsController.js",
                "~/Scripts/application/KnowledgeTesting/Controllers/contentController.js",
                "~/Scripts/application/KnowledgeTesting/Controllers/controlController.js",
                "~/Scripts/application/KnowledgeTesting/Controllers/testUnlocksController.js",
                "~/Scripts/application/KnowledgeTesting/Controllers/questionDetailsController.js",
                "~/Scripts/application/KnowledgeTesting/Controllers/addFromAnotherTestController.js",
                "~/Scripts/application/KnowledgeTesting/kkcountdown.js"));

            bundles.Add(new ScriptBundle("~/bundles/studentsTesting").Include(
                "~/Scripts/linq.js",
                "~/Scripts/application/KnowledgeTesting/knowledgeTestingModule.js",
                "~/Scripts/application/KnowledgeTesting/Controllers/testPassingNavigationController.js",
                "~/Scripts/application/KnowledgeTesting/Controllers/studentTestsController.js",
                "~/Scripts/jqplot/jquery.jqplot.min.js",
                "~/Scripts/jqplot/jqplot.barRenderer.min.js",
                "~/Scripts/jqplot/jqplot.categoryAxisRenderer.min.js",
                "~/Scripts/jqplot/jqplot.pointLabels.min.js",
                "~/Scripts/application/KnowledgeTesting/Controllers/passingController.js",
                "~/Scripts/application/KnowledgeTesting/Controllers/studentResultsController.js",
                "~/Scripts/application/KnowledgeTesting/kkcountdown.js"));
        }
    }
}