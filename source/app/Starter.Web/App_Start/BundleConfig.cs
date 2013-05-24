using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Optimization;

namespace DQF.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // two rows below allow to use .min versions of files
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);


            #region Scripts

            bundles.Add(new ScriptBundle("~/bundles/scripts/jquery")
                            .Include("~/Content/js/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/jqueryui")
                            .Include("~/Content/js/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/jqueryvalidate")
                            .Include("~/Content/js/jquery.unobtrusive*")
                            .Include("~/Content/js/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/bootstrap")
                            .Include("~/Content/js/bootstrap.js")
                            .Include("~/Content/js/bootstrap-datepicker.js")
                            .Include("~/Content/js/bootstrap-timepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/knockout")
                            .Include("~/Content/js/knockout-*")
                            .Include("~/Content/js/knockout.mapping-latest.js")
                            .Include("~/Content/js/knockout.extensions.js")
                            .Include("~/Content/js/perpetuum.knockout.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/other")
                            //.Include("~/Content/js/jquery.fileupload.js")
                            //.Include("~/Content/js/jquery.iframe-transport.js")
                            //.Include("~/Content/js/jquery.ui.widget.js")
                            //.Include("~/Content/js/sammy-0.7.1.min.js")
                            
                            );

            bundles.Add(new ScriptBundle("~/bundles/scripts/custom")
                            .IncludeDirectory("~/Content/js/custom", "*.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/scripts/modernizr")
                            .Include("~/Content/js/modernizr-*"));

            #endregion Scripts
            #region Stylesheets

            bundles.Add(new StyleBundle("~/bundles/css/custom")
                            .IncludeDirectory("~/Content/css/custom/", "*.css"));

            bundles.Add(new StyleBundle("~/bundles/css/bootstrap")
                            .Include("~/Content/css/bootstrap.css")
                            .Include("~/Content/css/bootstrap-responsive.css")
                            .Include("~/Content/css/bootstrap-datepicker.css")
                            .Include("~/Content/css/bootstrap-timepicker.css"));

            bundles.Add(new StyleBundle("~/bundles/css/style")
                            .Include("~/Content/css/style.css")
                            .Include("~/Content/css/style-responsive.css"));

            bundles.Add(new StyleBundle("~/bundles/css/all")
                            .IncludeDirectory("~/Content/css/", "*.css"));

            #endregion Stylesheets
        }


        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new ArgumentNullException("ignoreList");

            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
        }
    }
}