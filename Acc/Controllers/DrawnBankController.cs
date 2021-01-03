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
    public class DrawnBankController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public DrawnBankController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        [Authorize(Roles = "CoOwner,ShowBankswithdrawnfrom")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            return View();
        }
        [Authorize(Roles = "CoOwner,AddBankswithdrawnfrom")]
        public ActionResult AddNew()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            DrawnBank Obj = new DrawnBank
            {
                BankID = _unitOfWork.DrawnBank.GetMaxSerial(UserInfo.fCompanyId),
                Active = true
            };
            return PartialView(Obj);
        }
        [HttpGet]
        public JsonResult GetAllDrawnBank()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllDrawnBank = _unitOfWork.NativeSql.GetDrawnBank(UserInfo.fCompanyId);
                if (AllDrawnBank == null)
                {
                    return Json(new List<DrawnBankVM>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllDrawnBank, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<DrawnBankVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,AddBankswithdrawnfrom")]
        public JsonResult SaveDrawnBank(DrawnBank ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjToSave.BankID = _unitOfWork.DrawnBank.GetMaxSerial(UserInfo.fCompanyId);
                ObjToSave.InsDateTime = DateTime.Now;
                ObjToSave.InsUserID = userId;
                ObjToSave.CompanyID = UserInfo.fCompanyId;
                if (String.IsNullOrEmpty(ObjToSave.EnglishName))
                    ObjToSave.EnglishName = ObjToSave.ArabicName;

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
                _unitOfWork.DrawnBank.Add(ObjToSave);
                _unitOfWork.Complete();
                Msg.LastID = _unitOfWork.DrawnBank.GetMaxSerial(UserInfo.fCompanyId).ToString();
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
        [Authorize(Roles = "CoOwner,UpdateBankswithdrawnfrom")]
        public ActionResult UpdateDrawnBank(int id)
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

                    var Obj = _unitOfWork.DrawnBank.GetDrawnBankByID(UserInfo.fCompanyId, id);


                    return PartialView("UpdateDrawnBank", Obj);
                }



                return PartialView("UpdateDrawnBank", new DrawnBank());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,UpdateBankswithdrawnfrom")]
        public JsonResult UpdateDrawnBank(DrawnBank ObjUpdate)
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
                _unitOfWork.DrawnBank.Update(ObjUpdate);
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
        [HttpGet]
        public JsonResult CheckDrawnBank(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);

            var DrawnBankInfo = _unitOfWork.NativeSql.CheckDrawnBank(UserInfo.fCompanyId, id);
            if (DrawnBankInfo == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(DrawnBankInfo, JsonRequestBehavior.AllowGet);
            }

        }
    }
}