using Acc.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Acc.Controllers
{
    public class BaseController : Controller
    {
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_cultureAcc"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
                        Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
                        null;
            // Validate culture name


            CultureInfo currentCI = new System.Globalization.CultureInfo("en-US");
            NumberFormatInfo nfi = currentCI.NumberFormat;
            string[] nativeDigitList = nfi.NativeDigits;


            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe


            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ar-JO");
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern = "dd/MM/yyyy";


            Thread.CurrentThread.CurrentCulture.NumberFormat.NativeDigits = nativeDigitList;

            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cultureName);

            //if(User.)
            //var user = User.Identity;
            //ApplicationDbContext context = new ApplicationDbContext();
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //var s = UserManager.GetRoles(user.GetUserId());
            //if (s[0].ToString() == "CoOwner")
            //{
            //    ViewBag.displayMenu = "Yes";
            //}
            //else
            //{
            //    ViewBag.displayMenu = "No";
            //}


            return base.BeginExecuteCore(callback, state);
        }
    }
}