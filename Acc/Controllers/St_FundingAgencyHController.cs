using Acc.Helpers;
using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acc.Controllers
{
    [Authorize]
    public class St_FundingAgencyHController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_FundingAgencyHController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            return View();
        }

        public JsonResult GetAllFundingAgencyH()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllFundingAgencyH = _unitOfWork.St_FundingAgencyH.GetAllSt_FundingAgencyH(UserInfo.fCompanyId);
                if (AllFundingAgencyH == null)
                {
                    return Json(new List<St_FundingAgencyH>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllFundingAgencyH, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_FundingAgencyH>(), JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult Update(int id)
        {
            try
            {
                if (id != 0)
                {
                    var userId = User.Identity.GetUserId();

                    var UserInfo = _unitOfWork.User.GetUserByID(userId);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }
                    var Obj = _unitOfWork.St_FundingAgencyH.GetSt_FundingAgencyHByID(UserInfo.fCompanyId, id);
                    var St_FundingAgencyObj = new St_FundingAgencyHVM();
                    St_FundingAgencyObj.FundingAgencyID = Obj.FundingAgencyID;
                    St_FundingAgencyObj.CommissionAccount = Obj.CommissionAccount;
                    St_FundingAgencyObj.CommissionAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, St_FundingAgencyObj.CommissionAccount);
                    St_FundingAgencyObj.FlatCommission12 = Obj.FlatCommission12;
                    St_FundingAgencyObj.FlatCommission24 = Obj.FlatCommission24;
                    St_FundingAgencyObj.FlatCommission36 = Obj.FlatCommission36;
                    St_FundingAgencyObj.FlatCommission48 = Obj.FlatCommission48;
                    St_FundingAgencyObj.FlatCommission60 = Obj.FlatCommission60;
                    St_FundingAgencyObj.PhoneOfPersonInCharge = Obj.PhoneOfPersonInCharge;
                    St_FundingAgencyObj.NameOfPersonInCharge = Obj.NameOfPersonInCharge;
                    St_FundingAgencyObj.EmailOfPersonInCharge = Obj.EmailOfPersonInCharge;
                    St_FundingAgencyObj.ArabicName = Obj.ArabicName;
                    St_FundingAgencyObj.EnglishName = Obj.EnglishName;
                    return PartialView("Update", St_FundingAgencyObj);
                }
                return PartialView("Update", new St_FundingAgencyHVM());
            }

            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [HttpPost]

        public JsonResult UpdateFundingAgencyH(St_FundingAgencyHVM ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                ObjUpdate.CompanyID = UserInfo.fCompanyId;
                ObjUpdate.InsDateTime = DateTime.Now;
                ObjUpdate.InsUserID = userId;
                if (String.IsNullOrEmpty(ObjUpdate.EnglishName))
                    ObjUpdate.EnglishName = ObjUpdate.ArabicName;
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
                _unitOfWork.St_FundingAgencyH.Update(ObjUpdate);
                _unitOfWork.Complete();

                Msg.Code = 1;
                Msg.Msg = Resources.Resource.UpdatedSuccessfully;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult Delete(int id)
        {
            try
            {
                if (id != 0)
                {
                    var userId = User.Identity.GetUserId();

                    var UserInfo = _unitOfWork.User.GetUserByID(userId);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }

                    var Obj = _unitOfWork.St_FundingAgencyH.GetSt_FundingAgencyHByID(UserInfo.fCompanyId, id);
                    var St_FundingAgencyObj = new St_FundingAgencyHVM();
                    St_FundingAgencyObj.FundingAgencyID = Obj.FundingAgencyID;
                    St_FundingAgencyObj.CommissionAccount = Obj.CommissionAccount;
                    St_FundingAgencyObj.CommissionAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, St_FundingAgencyObj.CommissionAccount);
                    St_FundingAgencyObj.FlatCommission12 = Obj.FlatCommission12;
                    St_FundingAgencyObj.FlatCommission24 = Obj.FlatCommission24;
                    St_FundingAgencyObj.FlatCommission36 = Obj.FlatCommission36;
                    St_FundingAgencyObj.FlatCommission48 = Obj.FlatCommission48;
                    St_FundingAgencyObj.FlatCommission60 = Obj.FlatCommission60;
                    St_FundingAgencyObj.PhoneOfPersonInCharge = Obj.PhoneOfPersonInCharge;
                    St_FundingAgencyObj.NameOfPersonInCharge = Obj.NameOfPersonInCharge;
                    St_FundingAgencyObj.EmailOfPersonInCharge = Obj.EmailOfPersonInCharge;
                    St_FundingAgencyObj.ArabicName = Obj.ArabicName;
                    St_FundingAgencyObj.EnglishName = Obj.EnglishName;


                    return PartialView("Delete", St_FundingAgencyObj);
                }
                return PartialView("Delete", new St_FundingAgencyH());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }

        [HttpPost]
        public JsonResult DeleteFundingAgencyH(St_FundingAgencyHVM ObjDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjDelete.CompanyID = UserInfo.fCompanyId;
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
                _unitOfWork.St_FundingAgencyH.Delete(ObjDelete);
                _unitOfWork.Complete();

                Msg.Code = 1;
                Msg.Msg = Resources.Resource.DeletedSuccessfully;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Save()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            St_FundingAgencyHVM Obj = new St_FundingAgencyHVM
            {
                FundingAgencyID = _unitOfWork.St_FundingAgencyH.GetMaxSerial(UserInfo.fCompanyId)
            };
            return PartialView(Obj);
        }


        public JsonResult SaveSt_FundingAgencyH(St_FundingAgencyH ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjSave.FundingAgencyID = _unitOfWork.St_FundingAgencyH.GetMaxSerial(UserInfo.fCompanyId);
                ObjSave.InsDateTime = DateTime.Now;
                ObjSave.InsUserID = userId;
                ObjSave.CompanyID = UserInfo.fCompanyId;
                if (String.IsNullOrEmpty(ObjSave.EnglishName))
                    ObjSave.EnglishName = ObjSave.ArabicName;

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
                _unitOfWork.St_FundingAgencyH.Add(ObjSave);
                _unitOfWork.Complete();
                Msg.LastID = _unitOfWork.St_FundingAgencyH.GetMaxSerial(UserInfo.fCompanyId).ToString();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

        }



       


    }
}