using System.Web.Optimization;

namespace WebDbAppSettingsMaintenance
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/knockout/js").Include(
                    "~/Scripts/knockout-3.4.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/dataTables/js").Include(
                "~/Scripts/DataTables/jquery.dataTables.js",
                "~/Scripts/DataTables/dataTables.bootstrap.js"));

            bundles.Add(new StyleBundle("~/bundles/dataTables/css").Include(
                "~/Content/DataTables/css/jquery.dataTables.css",
                "~/Content/DataTables/css/dataTables.bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bower/sweetalerts/js").Include(
                "~/bower_components/sweetalert2/dist/sweetalert2.js"));

            bundles.Add(new StyleBundle("~/bower/sweetalerts/css").Include(
                "~/bower_components/sweetalert2/dist/sweetalert2.css"));

            bundles.Add(new ScriptBundle("~/bundles/Maintenance/js").IncludeDirectory(
                "~/Areas/DbAppSettings/Scripts", "*.js"));
        }
    }
}
