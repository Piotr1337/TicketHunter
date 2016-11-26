using System.Web;
using System.Web.Optimization;

namespace TicketHunter
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            //SKRYPTY
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/scripts/timers").Include(
                      "~/scripts/Admin/moment-with-locales.min.js",
                      "~/scripts/Admin/bootstrap-datetimepicker.js",
                      "~/scripts/Admin/datetimepickerScripts.js"
                      ));

            bundles.Add(new ScriptBundle("~/scripts/summernote").Include(
                      "~/scripts/Admin/summernote.min.js",
                      "~/scripts/Admin/summernote-pl-PL.min.js",
                      "~/scripts/Admin/summernoteScripts.js"
                      ));

            bundles.Add(new ScriptBundle("~/scripts/chosen").Include(
                      "~/scripts/Admin/chosen.jquery.min.js",
                      "~/scripts/Admin/myMultiSelect.js"
                      ));

            bundles.Add(new ScriptBundle("~/scripts/fullCalendar").Include(
                      "~/scripts/Admin/fullcalendar.js",
                      "~/scripts/Admin/myCalendar.js",
                      "~/scripts/Admin/locale-all.js"
                      ));

            bundles.Add(new ScriptBundle("~/scripts/adminScripts").Include(
                      "~/scripts/Admin/myScriptsAdmin.js"
                      ));

            bundles.Add(new ScriptBundle("~/scripts/switch").Include(
                      "~/scripts/Admin/bootstrap-switch.min.js",
                      "~/scripts/Admin/switchScripts.js"
                      ));

            bundles.Add(new ScriptBundle("~/scripts/sweetAlert").Include(
                      "~/scripts/Admin/sweetalert.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/scripts/autocompleteScript").Include(
                      "~/scripts/jquery.easy-autocomplete.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/scripts/blazy").Include(
                       "~/scripts/blazy.min.js"
                       ));

            bundles.Add(new ScriptBundle("~/scripts/myscripts").Include(
                        "~/scripts/myScripts.js"
                        ));
            bundles.Add(new ScriptBundle("~/scripts/readmore").Include(
                        "~/scripts/readmore.min.js"
                        ));

            //STYLE
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/styles").Include(
                      "~/Content/ErrorStyles.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/metro-bootstrap.min.css"));
            
            bundles.Add(new StyleBundle("~/Content/stylesForAdmin").Include(
                     "~/Content/Admin/simple-sidebar.css",
                     "~/Content/Admin/adminStyles.css"
                ));

            bundles.Add(new StyleBundle("~/Content/fullcalendar").Include(
                     "~/Content/Admin/fullcalendar.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/datetimepicker").Include(
                     "~/Content/Admin/bootstrap-datetimepicker-build.css"
                ));

            bundles.Add(new StyleBundle("~/Content/sweetalert").Include(
                     "~/Content/sweetalert.css"
                ));

            bundles.Add(new StyleBundle("~/Content/summernote").Include(
                     "~/Content/summernote.css"
                ));
            bundles.Add(new StyleBundle("~/Content/chosen").Include(
                     "~/Content/Admin/chosen.css"
                ));
            bundles.Add(new StyleBundle("~/Content/switch").Include(
                      "~/Content/bootstrap-switch.min.css"
                 ));
            bundles.Add(new StyleBundle("~/Content/autocomplete").Include(
                      "~/Content/easy-autocomplete.min.css",
                      "~/Content/easy-autocomplete.themes.min.css"
                 ));
            bundles.Add(new StyleBundle("~/Content/mystyles").Include(
                      "~/Content/myStyles.min.css"
                 ));
        }
    }
}
