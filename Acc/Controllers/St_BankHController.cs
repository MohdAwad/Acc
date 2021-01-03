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
    public class St_BankHController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_BankHController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            return View();
        }
        public JsonResult GetAllSt_BankH()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_BankH = _unitOfWork.NativeSql.GetAllBank(UserInfo.fCompanyId);
                if (AllSt_BankH == null)
                {
                    return Json(new List<St_BankH>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllSt_BankH, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_BankH>(), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Add()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            St_BankHVM Obj = new St_BankHVM
            {
                St_FundingAgencyH = _unitOfWork.St_FundingAgencyH.GetAllSt_FundingAgencyH(UserInfo.fCompanyId),
                BankID = _unitOfWork.St_BankH.GetMaxSerial(UserInfo.fCompanyId)
            };
            return PartialView(Obj);
        }
        public JsonResult SaveSt_BankH(St_BankHVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var StBankObj = new St_BankH();
                StBankObj.InsDateTime = DateTime.Now;
                StBankObj.InsUserID = userId;
                StBankObj.CompanyID = UserInfo.fCompanyId;
                StBankObj.FundingAgencyID = ObjToSave.FundingAgencyID;
                StBankObj.BankID = _unitOfWork.St_BankH.GetMaxSerial(UserInfo.fCompanyId);
                StBankObj.BankAccountNumber = ObjToSave.BankAccountNumber;
                if (String.IsNullOrEmpty(ObjToSave.EnglishName))
                    ObjToSave.EnglishName = ObjToSave.ArabicName;
                StBankObj.ArabicName = ObjToSave.ArabicName;
                StBankObj.EnglishName = ObjToSave.EnglishName;
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
                _unitOfWork.St_BankH.Add(StBankObj);
                _unitOfWork.Complete();
                Msg.LastID = _unitOfWork.St_BankH.GetMaxSerial(UserInfo.fCompanyId).ToString();
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
                    var Obj = _unitOfWork.St_BankH.GetAllSt_BankHByID(UserInfo.fCompanyId, id);
                    var St_BankObj = new St_BankHVM();
                    St_BankObj.St_FundingAgencyH = _unitOfWork.St_FundingAgencyH.GetAllSt_FundingAgencyH(UserInfo.fCompanyId);
                    St_BankObj.FundingAgencyID = Obj.FundingAgencyID;
                    St_BankObj.BankID = Obj.BankID;
                    St_BankObj.ArabicName = Obj.ArabicName;
                    St_BankObj.EnglishName = Obj.EnglishName;
                    St_BankObj.BankAccountNumber = Obj.BankAccountNumber;
                    St_BankObj.BankAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, St_BankObj.BankAccountNumber);
                    return PartialView("Update", St_BankObj);
                }
                return PartialView("Update", new St_BankHVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
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
                    var Obj = _unitOfWork.St_BankH.GetAllSt_BankHByID(UserInfo.fCompanyId, id);
                    var St_BankObj = new St_BankHVM();
                    St_BankObj.St_FundingAgencyH = _unitOfWork.St_FundingAgencyH.GetAllSt_FundingAgencyH(UserInfo.fCompanyId);
                    St_BankObj.FundingAgencyID = Obj.FundingAgencyID;
                    St_BankObj.BankID = Obj.BankID;
                    St_BankObj.ArabicName = Obj.ArabicName;
                    St_BankObj.EnglishName = Obj.EnglishName;
                    St_BankObj.BankAccountNumber = Obj.BankAccountNumber;
                    St_BankObj.BankAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, St_BankObj.BankAccountNumber);
                    return PartialView("Delete", St_BankObj);
                }
                return PartialView("Delete", new St_BankHVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [HttpPost]
        public JsonResult UpdateSt_BankH(St_BankHVM ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var St_BankHObj = new St_BankH();
                St_BankHObj.InsDateTime = DateTime.Now;
                St_BankHObj.InsUserID = userId;
                St_BankHObj.CompanyID = UserInfo.fCompanyId;
                St_BankHObj.FundingAgencyID = ObjUpdate.FundingAgencyID;
                St_BankHObj.BankID = ObjUpdate.BankID;
                if (String.IsNullOrEmpty(ObjUpdate.EnglishName))
                ObjUpdate.EnglishName = ObjUpdate.ArabicName;
                St_BankHObj.ArabicName = ObjUpdate.ArabicName;
                St_BankHObj.EnglishName = ObjUpdate.EnglishName;
                St_BankHObj.BankAccountNumber = ObjUpdate.BankAccountNumber;

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
                _unitOfWork.St_BankH.Update(St_BankHObj);
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
        [HttpPost]
        public JsonResult DeleteSt_BankH(St_BankHVM ObjDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjDelete.CompanyID = UserInfo.fCompanyId;
                var St_BankHObj = new St_BankH();
                St_BankHObj.CompanyID = UserInfo.fCompanyId;
                St_BankHObj.BankID = ObjDelete.BankID;
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
                _unitOfWork.St_BankH.Delete(St_BankHObj);
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
    }
}