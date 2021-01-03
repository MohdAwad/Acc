using Acc.Helpers;
using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace Acc.Controllers
{
    [Authorize]
    public class SupplierBranchBankController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierBranchBankController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        // GET: SupplierBranchBank

        [Authorize(Roles = "CoOwner,ShowSupplierbankbranches")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            return View();
        }
        [HttpGet]
        public JsonResult GetAllSupplierBranchBank()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSupplierBranchBank = _unitOfWork.NativeSql.GetAllSupplierBranchBank(UserInfo.fCompanyId);
                if (AllSupplierBranchBank == null)
                {
                    return Json(new List<SupplierBranchBankVM>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllSupplierBranchBank, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<SupplierBranchBankVM>(), JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "CoOwner,AddSupplierbankbranches")]
        public ActionResult AddNew()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            SupplierBranchBankVM Obj = new SupplierBranchBankVM
            {
                SupplierBranchBankID = _unitOfWork.SupplierBranchBank.GetMaxSerial(UserInfo.fCompanyId),
                SupplierBank = _unitOfWork.SupplierBank.GetAllSupplierBank(UserInfo.fCompanyId),
                SupplierBankID = 1
            };
            return PartialView(Obj);
        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,AddSupplierbankbranches")]
        public JsonResult SaveSupplierBranchBank(SupplierBranchBankVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SupplierBranchBankObj = new SupplierBranchBank();
                SupplierBranchBankObj.InsDateTime = DateTime.Now;
                SupplierBranchBankObj.InsUserID = userId;
                SupplierBranchBankObj.CompanyID = UserInfo.fCompanyId;
                SupplierBranchBankObj.SupplierBranchBankID = ObjToSave.SupplierBranchBankID;
                SupplierBranchBankObj.SupplierBankID = ObjToSave.SupplierBankID;
                if (String.IsNullOrEmpty(ObjToSave.EnglishName))
                    ObjToSave.EnglishName = ObjToSave.ArabicName;
                SupplierBranchBankObj.ArabicName = ObjToSave.ArabicName;
                SupplierBranchBankObj.EnglishName = ObjToSave.EnglishName;

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
                _unitOfWork.SupplierBranchBank.Add(SupplierBranchBankObj);
                _unitOfWork.Complete();
                Msg.LastID = _unitOfWork.SupplierBranchBank.GetMaxSerial(UserInfo.fCompanyId).ToString();
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

        [Authorize(Roles = "CoOwner,UpdateSupplierbankbranches")]
        public ActionResult UpdateSupplierBranchBank(int id)
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

                    var Obj = _unitOfWork.NativeSql.GetSupplierBranchBankByID(UserInfo.fCompanyId, id);
                    var SupplierBranchBankObj = new SupplierBranchBankVM { };
                    SupplierBranchBankObj.SupplierBank = _unitOfWork.SupplierBank.GetAllSupplierBank(UserInfo.fCompanyId);
                    SupplierBranchBankObj.SupplierBranchBankID = Obj.SupplierBranchBankID;
                    SupplierBranchBankObj.SupplierBankID = Obj.SupplierBankID;
                    SupplierBranchBankObj.ArabicName = Obj.ArabicName;
                    SupplierBranchBankObj.EnglishName = Obj.EnglishName;


                    return PartialView("UpdateSupplierBranchBank", SupplierBranchBankObj);
                }



                return PartialView("UpdateSupplierBranchBank", new SupplierBranchBankVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }

        [Authorize(Roles = "CoOwner,DeleteSupplierbankbranches")]
        public ActionResult DeleteSupplierBranchBank(int id)
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

                    var Obj = _unitOfWork.NativeSql.GetSupplierBranchBankByID(UserInfo.fCompanyId, id);
                    var SupplierBranchBankObj = new SupplierBranchBankVM { };
                    SupplierBranchBankObj.SupplierBranchBankID = Obj.SupplierBranchBankID;
                    SupplierBranchBankObj.SupplierBankID = Obj.SupplierBankID;
                    SupplierBranchBankObj.ArabicName = Obj.ArabicName;
                    SupplierBranchBankObj.EnglishName = Obj.EnglishName;
                    SupplierBranchBankObj.SupplierBankName = _unitOfWork.NativeSql.GetSupplierBankName(UserInfo.fCompanyId, SupplierBranchBankObj.SupplierBankID);


                    return PartialView("DeleteSupplierBranchBank", SupplierBranchBankObj);
                }



                return PartialView("DeleteSupplierBranchBank", new SupplierBranchBank());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "CoOwner,UpdateSupplierbankbranches")]
        public JsonResult UpdateSupplierBranchBank(SupplierBranchBankVM ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var SupplierBranchBankObj = new SupplierBranchBank();
                SupplierBranchBankObj.InsDateTime = DateTime.Now;
                SupplierBranchBankObj.InsUserID = userId;
                SupplierBranchBankObj.CompanyID = UserInfo.fCompanyId;
                SupplierBranchBankObj.SupplierBranchBankID = ObjUpdate.SupplierBranchBankID;
                SupplierBranchBankObj.SupplierBankID = ObjUpdate.SupplierBankID;
                if (String.IsNullOrEmpty(ObjUpdate.EnglishName))
                    ObjUpdate.EnglishName = ObjUpdate.ArabicName;
                SupplierBranchBankObj.ArabicName = ObjUpdate.ArabicName;
                SupplierBranchBankObj.EnglishName = ObjUpdate.EnglishName;
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
                _unitOfWork.SupplierBranchBank.Update(SupplierBranchBankObj);
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
        [Authorize(Roles = "CoOwner,DeleteSupplierbankbranches")]
        public JsonResult DeleteSupplierBranchBank(SupplierBranchBankVM ObjDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                ObjDelete.CompanyID = UserInfo.fCompanyId;
                var SupplierBranchBankObj = new SupplierBranchBank();
                SupplierBranchBankObj.CompanyID = UserInfo.fCompanyId;
                SupplierBranchBankObj.SupplierBranchBankID = ObjDelete.SupplierBranchBankID;

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
                _unitOfWork.SupplierBranchBank.Delete(SupplierBranchBankObj);
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
        [HttpGet]
        public JsonResult CheckSupplierBankBeforDelete(int id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != 0)
            {
                int SupplierBranchBankID = _unitOfWork.NativeSql.CheckSupplierBranchBank(UserInfo.fCompanyId, id);

                return Json(SupplierBranchBankID, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}