using Acc.Helpers;
using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acc.Controllers
{ 

    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }

        [HttpGet]
        public  JsonResult  GetLoginInfo()
        {
            try
            {


                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);


                UserLoginInfoVM Obj = new UserLoginInfoVM
                {
                    Name = UserInfo.Email,
                    CurrYear = UserInfo.CurrYear.ToString()
                };







                return Json(Obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UserLoginInfoVM Obj = new UserLoginInfoVM
                {
                    Name ="Error",
                    CurrYear ="0"
                };

                ViewBag.Error = ex.Message.ToString();
                return Json(Obj, JsonRequestBehavior.AllowGet);
            }


        }
        [AllowAnonymous]
        public ActionResult SetCulture(string culture)
        {
            // Validate input

            culture = CultureHelper.GetImplementedCulture(culture);
            //  RouteData.Values["culture"] = culture;  // set culture
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_cultureAcc"];
            if (cookie != null)
            { cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }  // update cookie value
            else
            {
                cookie = new HttpCookie("_cultureAcc");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            cookie.SameSite = (SameSiteMode.Lax);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index");
        }

        
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            DashBoardVM Obj = new DashBoardVM();
            if (UserInfo == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (UserInfo.fCompanyId != 0)
            {
                var CoInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                Obj.CompanyName = CoInfo.ArabicName;
                Obj.ActiveYear = UserInfo.CurrYear;
                Obj.WorkWithCostCenter = CoInfo.WorkWithCostCenter;
                Obj.TheDecimalPointForTheLocalCurrency = CoInfo.TheDecimalPointForTheLocalCurrency;
                Obj.TheDecimalPointForTheForeignCurrency = CoInfo.TheDecimalPointForTheForeignCurrency;
            }
            ViewBag.CurrentDate = DateTime.Now.ToString();
            string D = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString();

            if (Resources.Resource.CurLang == "Eng")
            {
                ViewBag.CuurentDay = D + "  " + DateTime.Now.ToString("dddd", CultureInfo.CreateSpecificCulture("en"));
            }
            else
            {
                ViewBag.CuurentDay = D + "  " + DateTime.Now.ToString("dddd", CultureInfo.CreateSpecificCulture("ar"));
            }
            return View(Obj);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public JsonResult GetShowChequeFund()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Today = DateTime.Now;
                Today = Today.Date;
                var UnpaidFundCheques = _unitOfWork.NativeSql.GetUnpaidFundCheques(UserInfo.fCompanyId, Today);
                return Json(UnpaidFundCheques, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<PaperFilterVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetShowChequesUnderCollection()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Today = DateTime.Now;
                Today = Today.Date;
                var UnpaidUnderCollectionCheques = _unitOfWork.NativeSql.GetUnpaidUnderCollectionCheques(UserInfo.fCompanyId, Today);
                return Json(UnpaidUnderCollectionCheques, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<PaperFilterVM>(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetShowPostdatedCheques()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Today = DateTime.Now;
                Today = Today.Date;
                var UnpaidPostdatedCheque = _unitOfWork.NativeSql.GetUnpaidPostdatedCheque(UserInfo.fCompanyId, Today);
                return Json(UnpaidPostdatedCheque, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<PaperFilterVM>(), JsonRequestBehavior.AllowGet);
            }
        }
    }
}