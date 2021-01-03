using DevExpress.Security.Resources;
using DevExpress.Web.Mvc;
using DevExpress.XtraReports.Security;
using System;
using System.Threading;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Acc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        
        protected void Application_Start()
        {
            DevExpress.XtraReports.Web.QueryBuilder.Native.QueryBuilderBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Disabled;
            DevExpress.XtraReports.Web.ReportDesigner.Native.ReportDesignerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Disabled;
            DevExpress.XtraReports.Web.WebDocumentViewer.Native.WebDocumentViewerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Disabled;


            // Images can be loaded only from the "C:\\StaticResources\\" file directory and "http://mysite.dev" site  
            DevExpress.Security.Resources.AccessSettings.StaticResources.TrySetRules(DirectoryAccessRule.Allow("C:\\AccWeb\\Acc\\Acc\\CompanyLogo"), UrlAccessRule.Allow("http://mysite.dev"));


            //DevExpress.XtraReports.Web.WebDocumentViewer.Native.WebDocumentViewerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Disabled;
            //DevExpress.Utils.UrlAccessSecurityLevelSetting.SecurityLevel = DevExpress.Utils.UrlAccessSecurityLevel.Unrestricted;


            //  DevExpress.Utils.UrlAccessSecurityLevelSetting.SecurityLevel = DevExpress.Utils.UrlAccessSecurityLevel.Unrestricted;
            //ScriptPermissionManager.GlobalInstance = new ScriptPermissionManager(ExecutionMode.Unrestricted);
            //    DevExpress.XtraReports.Web.WebDocumentViewer.Native.WebDocumentViewerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Disabled;
            //    DevExpress.XtraReports.Web.QueryBuilder.Native.QueryBuilderBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Disabled;
            //    DevExpress.XtraReports.Web.ReportDesigner.Native.ReportDesignerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Disabled;
            // Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTQ2MTg2QDMxMzcyZTMzMmUzMG1tbHFkVnRpNUd5TmhLNGI2NFJucW9Rb2xydkNMQkJmUGRPQWpDc0tzSFE9");
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ar-JO");
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern = "dd/MM/yyyy";
            //DevExpress.Web.Mvc.MVCxReportDesigner.StaticInitialize();
            DevExpress.Web.Mvc.MVCxWebDocumentViewer.StaticInitialize();
            DevExpress.Web.Mvc.MVCxReportDesigner.StaticInitialize();
            // ...
          //  DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension.RegisterExtensionGlobal(new CustomReportStorageWebExtension);
            // ..
          //  DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension.RegisterExtensionGlobal(new ReportStorageWebExtension1());
        }
        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            DevExpressHelper.Theme = "iOS";
        }
    }
}
