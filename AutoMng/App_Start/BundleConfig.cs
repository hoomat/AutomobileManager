using System.Web;
using System.Web.Optimization;

namespace AutomobilMng
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

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/bootstrap.css"
                      //"~/Content/bootstrap-theme.min.css"
                //"~/Content/site.css"
                      ));




            //bundles.Add(new StyleBundle("~/Content/Ace").Include(
                  
            //           "~/Content/Ace/ace.min.css",
            //           //"~/Content/Ace/jquery-ui-1.10.3.custom.min.css",
            //           "~/Content/Ace/chosen.min.css"
            //            //"~/Content/Ace/bootstrap-responsive.min.css",
            //             //"~/Content/Ace/font-awesome.min.css"
            //         ));
            //bundles.Add(new ScriptBundle("~/bundles/Ace").Include(

            //        //"~/Scripts/Ace/jquery-ui-1.10.3.custom.min.js",
            //    "~/Scripts/Ace/ace-elements.min.js",
            //    "~/Scripts/Ace/ace.min.js"

            //    ));
        }
    }
}
