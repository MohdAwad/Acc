using Acc.Helpers;
using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace Acc.Controllers
{
    [Authorize]
    public class FreezeTransactionController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public FreezeTransactionController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        [Authorize(Roles = "CoOwner,ShowFreezunfreezmonthtransaction")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            return View();
        }
        public JsonResult GetAllFreezeTransaction()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllFreezeTransaction = _unitOfWork.NativeSql.GetAllFreezeTransaction(UserInfo.fCompanyId);
                if (AllFreezeTransaction == null)
                {
                    return Json(new List<FreezeTransactionVM>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllFreezeTransaction, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<FreezeTransactionVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,FreezFreezunfreezmonthtransaction")]
        public JsonResult SaveFreezeTransaction(FreezeTransactionVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var FreezeTransactionObj = new FreezeTransaction();
                FreezeTransactionObj.CompanyID = UserInfo.fCompanyId;
                FreezeTransactionObj.InsDateTime = DateTime.Now;
                FreezeTransactionObj.InsUserID = userId;
                FreezeTransactionObj.MonthID = ObjToSave.MonthID;
                FreezeTransactionObj.CompanyYear = UserInfo.CurrYear;
                FreezeTransactionObj.MinmumDate = new DateTime(UserInfo.CurrYear, ObjToSave.MonthID, 1);
                FreezeTransactionObj.MaximumDate = FreezeTransactionObj.MinmumDate.AddMonths(1).AddDays(-1);

                if (!ModelState.IsValid)
                {
                    string Err = " ";
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (ModelError error in errors)
                    {
                        Err = Err + error.ErrorMessage + " * ";
                    }

                    Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + Err;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);

                }
                _unitOfWork.FreezeTransaction.Add(FreezeTransactionObj);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.FreezingSuccessfully;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,unFreezFreezunfreezmonthtransaction")]
        public JsonResult SaveUnFreezeTransaction(FreezeTransactionVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var UnFreezeTransactionObj = new FreezeTransaction();
                UnFreezeTransactionObj.CompanyID = UserInfo.fCompanyId;
                UnFreezeTransactionObj.InsDateTime = DateTime.Now;
                UnFreezeTransactionObj.InsUserID = userId;
                UnFreezeTransactionObj.MonthID = ObjToSave.MonthID;
                UnFreezeTransactionObj.CompanyYear = UserInfo.CurrYear;

                if (!ModelState.IsValid)
                {
                    string Err = " ";
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (ModelError error in errors)
                    {
                        Err = Err + error.ErrorMessage + " * ";
                    }

                    Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + Err;
                    Msg.Code = 0;
                    return Json(Msg, JsonRequestBehavior.AllowGet);

                }
                _unitOfWork.FreezeTransaction.Delete(UnFreezeTransactionObj);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.UnFreezingSuccessfully;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult CheckDateIsFreezeDate(DateTime id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != DateTime.MinValue)
            {
                int CheckCase = _unitOfWork.NativeSql.CheckDateIsFreezeDate(UserInfo.fCompanyId, id);
                return Json(CheckCase, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}