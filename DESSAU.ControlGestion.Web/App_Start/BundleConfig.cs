using System.Web;
using System.Web.Optimization;

namespace DESSAU.ControlGestion.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/myScripts").Include(
                      "~/Scripts/jquery-ui-{version}.js",
                      "~/Scripts/DESSAUControlGestionConfiguration.js",
                      "~/Scripts/jquery.cascadingDropDown.js", "~/Scripts/jquery.numeric.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      //"~/Content/site.css",
                      "~/Content/MySite.css",
                      "~/Content/ct/css/pe-icon-7-stroke.css"));

            bundles.Add(new StyleBundle("~/Content/startboostrapcss").Include(
                      "~/Content/bower_components/metisMenu/dist/metisMenu.css",
                      "~/Content/dist/css/sb-admin-2.css",
                      "~/Content/bower_components/morrisjs/morris.css",
                      "~/Content/bower_components/font-awesome/css/font-awesome.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/startboostrapjs").Include(
                      "~/Content/bower_components/metisMenu/dist/metisMenu.min.js",
                      "~/Content/bower_components/raphael/raphael-min.js",
                      "~/Content/bower_components/morrisjs/morris.min.js",
                      "~/Content/js/morris-data.js",
                      "~/Content/dist/js/sb-admin-2.js"));
        }
    }
}
