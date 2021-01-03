using System.Web;
using System.Web.Optimization;

namespace Acc
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //var lessBundle = new Bundle("~/My/Less").IncludeDirectory("~/My", "*.less");
            //lessBundle.Transforms.Add(new LessTransform());
            //lessBundle.Transforms.Add(new CssMinify());
            //bundles.Add(lessBundle);
            BundleTable.EnableOptimizations = true;


            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                 "~/Scripts/app/job.js"


                ));


            bundles.Add(new ScriptBundle("~/bundles/JqueDev").Include(
              "~/Scripts/jquery-{version}.js",

    "~/Scripts/jquery.validate.js",
    "~/Scripts/jquery.validate.unobtrusive.js",
    "~/Scripts/jquery.unobtrusive-ajax.js",
    "~/Scripts/jquery-ui-1.10.3.js",
    "~/Scripts/globalize.js",
    "~/Scripts/knockout-3.0.0.js",
    "~/Scripts/dx.webappjs.js",
    "~/Scripts/data.js" 


             ));


            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                             //"~/Scripts/jquery-clockpicker.min.js",
                             "~/Scripts/jquery-{version}.js",

                                 "~/Scripts/sb-admin-2.js",
                             //"~/Scripts/jquery.min.js",
                             "~/Scripts/bootstrap.min.js",
                             "~/Scripts/metisMenu.min.js",
                             "~/Scripts/raphael.min.js",

                             "~/Scripts/bootbox.js",
                             "~/Scripts/sweetalert.min.js",
                             //"~/vendors/bower_components/sweetalert/dist/sweetalert.min.js",
                             "~/Scripts/sweetalert-data.js",
                              "~/Scripts/respond.js",
                               // "~/Scripts/jquery.js",
                               "~/Scripts/datatables/jquery.datatables.js",
                               "~/Scripts/datatables/datatables.bootstrap.js",

                              "~/Scripts/typeahead.bundle.js",
                              "~/Scripts/toastr.js",
                             "~/Scripts/bootstrap-datepicker.min.js",
                                "~/Scripts/jquery-1.11.3.min.js",
                                ////"~/Scripts/jquery-1.11.3.min.js",
                                "~/Scripts/bootstrap-datepicker.js",
                                 //"~/Scripts/jquery-1.12.4.js",


                                 // "~/Scripts/buttons.print.min.js",
                                 "~/Scripts/dataTables.buttons.min.js",
                                "~/Scripts/jszip.min.js",
                                 "~/Scripts/pdfmake.min.js",
                                 "~/Scripts/vfs_fonts.js",
                                 "~/Scripts/buttons.html5.min.js",
                                "~/Scripts/moment.js",
                                  // "~/Scripts/moment-with-locales.js",
                                  // "~/Scripts/animate.js",
                                  "~/Scripts/bootstrap-timepicker.min.js",
                                 "~/Scripts/Chart.js",
                                // "~/Scripts/jquery.quicksearch.js",

                                "~/Scripts/jquery.timepicker.min.js",
                                // "~/Scripts/bootstrap-clockpicker.min.js",
                                "~/Scripts/jquery-clockpicker.min.js",
                                 "~/Scripts/jquery.maskedinput.js",
                               "~/Scripts/jquery.slimscroll.js",
                                "~/Scripts/jquery.sparkline.min.js",
                                         "~/Scripts/sparkline-data.js",
                                            "~/Scripts/jquery-ui.js",
                                                                                  "~/Scripts/sum().js",
                                        // "~/vendors/bower_components/jquery/dist/jquery.min.js",
                                        //"~/vendors/bower_components/bootstrap/dist/js/bootstrap.min.js",
                                        "~/vendors/bower_components/jasny-bootstrap/dist/js/jasny-bootstrap.min.js",
                                        //"~/vendors/bower_components/datatables/media/js/jquery.dataTables.min.js",
                                        "~/vendors/bower_components/waypoints/lib/jquery.waypoints.min.js",
                                        "~/vendors/bower_components/jquery.counterup/jquery.counterup.min.js",
                                        "~/Scripts/dropdown-bootstrap-extended.js",
                                        "~/vendors/bower_components/owl.carousel/dist/owl.carousel.min.js",
                                        "~/vendors/bower_components/switchery/dist/switchery.min.js",
                                        "~/vendors/bower_components/echarts/dist/echarts-en.min.js",
                                        "~/vendors/echarts-liquidfill.min.js",
                                        "~/vendors/bower_components/jquery-toast-plugin/dist/jquery.toast.min.js",
                                        "~/Scripts/init.js",
                                              //"~/Scripts/dashboard-data.js",
                                              "~/Scripts/jquery-3.5.1.js",
                                           "~/Scripts/jquery.dataTables.min.js",
                                          "~/Scripts/dataTables.checkboxes.min.js",
                                             "~/Scripts/dataTables.select.min.js",



                                            "~/Scripts/jquery.multi-select.js",
                                              "~/Scripts/dropzone.min.js",
                                              "~/vendors/bower_components/eonasdan-bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min.js",
                                               "~/Scripts/jquery.maskedinput.min.js",
                                               "~/Scripts/jquery-resizable.min.js",
                                                 "~/Scripts/dropzone.min.js"





                            ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));



            bundles.Add(new StyleBundle("~/Content/AllMyCss").Include(

                                //        "~/Content/css",
                                //        "~/Content/bootstrap.min.css",
                                //        "~/Content/font-awesome/css/font-awesome.min.css",

                                //        "~/Content/morris.css",
                                //        "~/Content/datatables/css/datatables.bootstrap.css",
                                "~/Content/typeahead.css",
                //        "~/Content/fonts/font-awesome",
                //        "~/Content/font-awesome/fonts",
                //  "~/Content/toastr.css",
                //"~/Content/sb-admin-2.css",
                //        "~/Content/bootstrap-datepicker.css",
                //        "~/Content/jquery.dataTables.min.css",
                //        "~/Content/buttons.dataTables.min.css",
                //        "~/Content/timepicker.less.css",

                //        "~/Content/jquery.timepicker.min.css",
                //        "~/Content/bootstrap-clockpicker.min.css",

                //"~/Content/basic.css",
                //        "~/Content/dataTables.bootstrap.min.css",

                //         "~/Content/site.css",
                //            "~/Content/css/style.css" ,
                //               "~/vendors/bower_components/jasny-bootstrap/dist/css/jasny-bootstrap.min.css",
                //"~/vendors/bower_components/datatables/media/css/jquery.dataTables.min.css",
                //"~/vendors/bower_components/jquery-toast-plugin/dist/jquery.toast.min.css",
                //"~/dist/css/style.css"
                //  "~/Content/metisMenu.css",
                "~/Content/vendors/bower_components/datatables/media/css/jquery.dataTables.min.css",

                "~/Content/vendors/bower_components/jquery-toast-plugin/dist/jquery.toast.min.css",

                "~/Content/dist/css/style.css",
                      "~/Content/dist/css/style.scss",
                            "~/Content/metisMenu.css",

                             "~/Content/toastr.css",

 "~/Content/vendors/bower_components/switchery/dist/switchery.min.css",
   "~/Content/dist/css/fancy-buttons.css",
    "~/Content/multi-select.css",
                                     "~/Content/multi-select.dev.css",
                                     "~/Content/multi-select.dist.css",
                                    "~/Content/dataTables.checkboxes.css",
                                      "~/Content/jquery.dataTables.min.css",
                                         "~/Content/select.dataTables.min.css",
                                         "~/Content/NaghamStyle.css"








                      ));


            bundles.Add(new StyleBundle("~/Content/AllMyCss-rtl").Include(

                                //        "~/Content/css",
                                //        "~/Content/bootstrap.min.css",
                                //        "~/Content/font-awesome/css/font-awesome.min.css",

                                //        "~/Content/morris.css",
                                //        "~/Content/datatables/css/datatables.bootstrap.css",

                                "~/Content/typeahead.css",
                //        "~/Content/fonts/font-awesome",
                //        "~/Content/font-awesome/fonts",
                //  "~/Content/toastr.css",
                //"~/Content/sb-admin-2.css",
                //        "~/Content/bootstrap-datepicker.css",
                //        "~/Content/jquery.dataTables.min.css",
                //        "~/Content/buttons.dataTables.min.css",
                //        "~/Content/timepicker.less.css",

                //        "~/Content/jquery.timepicker.min.css",
                //        "~/Content/bootstrap-clockpicker.min.css",
                //"~/Content/dropzone.css",
                //"~/Content/basic.css",
                //        "~/Content/dataTables.bootstrap.min.css",

                //         "~/Content/site.css",
                //            "~/Content/css/style.css" ,
                //               "~/vendors/bower_components/jasny-bootstrap/dist/css/jasny-bootstrap.min.css",
                //"~/vendors/bower_components/datatables/media/css/jquery.dataTables.min.css",
                //"~/vendors/bower_components/jquery-toast-plugin/dist/jquery.toast.min.css",
                //"~/dist/css/style.css"
                //  "~/Content/metisMenu.css",
                "~/Content/vendors/bower_components/datatables/media/css/jquery.dataTables.min.css",

                "~/Content/vendors/bower_components/jquery-toast-plugin/dist/jquery.toast.min.css",

                "~/Content/RTL/dist/css/style.css",
                      "~/Content/style-rtl.scss",
                            "~/Content/metisMenu.css",

                             "~/Content/toastr.css",

 "~/Content/vendors/bower_components/switchery/dist/switchery.min.css",
   "~/Content/RTL/dist/css/fancy-buttons.css",
    "~/Content/multi-select.css",
                                     "~/Content/multi-select.dev.css",
                                     "~/Content/multi-select.dist.css",
                                     "~/Content/dataTables.checkboxes.css",
                                     "~/Content/jquery.dataTables.min.css",
                                         "~/Content/select.dataTables.min.css",
                                         "~/Content/NaghamStyle.css"




        ));

        }
    }
}
