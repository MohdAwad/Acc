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
    public class St_BranchHController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_BranchHController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            return View();
        }
        [HttpGet]
        public JsonResult GetAllSt_BranchH()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_Branch = _unitOfWork.NativeSql.GetAllSt_BranchH(UserInfo.fCompanyId);
                if (AllSt_Branch == null)
                {
                    return Json(new List<St_BranchHVM>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllSt_Branch, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_BranchHVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Add()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            St_BranchHVM Obj = new St_BranchHVM
            {
                St_WarehouseHBranchStock = _unitOfWork.St_WarehouseH.GetAllSt_WarehouseH(UserInfo.fCompanyId),
                St_WarehouseHMainStock = _unitOfWork.St_WarehouseH.GetAllSt_WarehouseH(UserInfo.fCompanyId)
            };
            return PartialView(Obj);
        }
        public JsonResult Save(St_BranchHVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var ObjSaveBranch = new St_BranchH();
                ObjSaveBranch.InsDateTime = DateTime.Now;
                ObjSaveBranch.InsUserID = userId;
                ObjSaveBranch.CompanyID = UserInfo.fCompanyId;
                ObjSaveBranch.BranchCode = ObjSave.BranchCode;
                ObjSaveBranch.BranchStockCode = ObjSave.BranchStockCode;
                ObjSaveBranch.MainStockCode = ObjSave.MainStockCode;
                ObjSaveBranch.Telephone = ObjSave.Telephone;
                ObjSaveBranch.Address = ObjSave.Address;
                ObjSaveBranch.ArabicName = ObjSave.ArabicName;
                if (String.IsNullOrEmpty(ObjSave.EnglishName))
                {
                    ObjSaveBranch.EnglishName = ObjSaveBranch.ArabicName;
                }
                else
                {
                    ObjSaveBranch.EnglishName = ObjSave.EnglishName;
                }
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
                _unitOfWork.St_BranchH.Add(ObjSaveBranch);
                _unitOfWork.Complete();
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
        public ActionResult Update(string id)
        {
            try
            {
                if (id != "")
                {
                    var userId = User.Identity.GetUserId();
                    var UserInfo = _unitOfWork.User.GetUserByID(userId);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }

                    var Obj = _unitOfWork.NativeSql.GetSt_BranchHByID(UserInfo.fCompanyId, id);
                    var St_BranchObj = new St_BranchHVM { };
                    St_BranchObj.St_WarehouseHMainStock = _unitOfWork.St_WarehouseH.GetAllSt_WarehouseH(UserInfo.fCompanyId);
                    St_BranchObj.St_WarehouseHBranchStock = _unitOfWork.St_WarehouseH.GetAllSt_WarehouseH(UserInfo.fCompanyId);
                    St_BranchObj.BranchCode = Obj.BranchCode;
                    St_BranchObj.ArabicName = Obj.ArabicName;
                    St_BranchObj.EnglishName = Obj.EnglishName;
                    St_BranchObj.Telephone = Obj.Telephone;
                    St_BranchObj.Address = Obj.Address;
                    St_BranchObj.MainStockCode = Obj.MainStockCode;
                    St_BranchObj.BranchStockCode = Obj.BranchStockCode;
                    return PartialView("Update", St_BranchObj);
                }
                return PartialView("Update", new St_BranchHVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        public ActionResult Delete(string id)
        {
            try
            {
                if (id != "")
                {
                    var userId = User.Identity.GetUserId();
                    var UserInfo = _unitOfWork.User.GetUserByID(userId);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }

                    var Obj = _unitOfWork.NativeSql.GetSt_BranchHByID(UserInfo.fCompanyId, id);
                    var St_BranchObj = new St_BranchHVM { };
                    St_BranchObj.BranchCode = Obj.BranchCode;
                    St_BranchObj.ArabicName = Obj.ArabicName;
                    St_BranchObj.EnglishName = Obj.EnglishName;
                    St_BranchObj.Telephone = Obj.Telephone;
                    St_BranchObj.Address = Obj.Address;
                    St_BranchObj.MainStockName = _unitOfWork.NativeSql.GetSt_WarehouseHName(UserInfo.fCompanyId, Obj.MainStockCode);
                    St_BranchObj.BranchStockName = _unitOfWork.NativeSql.GetSt_WarehouseHName(UserInfo.fCompanyId, Obj.BranchStockCode);
                    return PartialView("Delete", St_BranchObj);
                }
                return PartialView("Delete", new St_BranchHVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [HttpPost]
        public JsonResult Update(St_BranchHVM ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var ObjUpdateBranch = new St_BranchH();
                ObjUpdateBranch.InsDateTime = DateTime.Now;
                ObjUpdateBranch.InsUserID = userId;
                ObjUpdateBranch.CompanyID = UserInfo.fCompanyId;
                ObjUpdateBranch.BranchCode = ObjUpdate.BranchCode;
                ObjUpdateBranch.BranchStockCode = ObjUpdate.BranchStockCode;
                ObjUpdateBranch.MainStockCode = ObjUpdate.MainStockCode;
                ObjUpdateBranch.Telephone = ObjUpdate.Telephone;
                ObjUpdateBranch.Address = ObjUpdate.Address;
                ObjUpdateBranch.ArabicName = ObjUpdate.ArabicName;
                if (String.IsNullOrEmpty(ObjUpdate.EnglishName))
                {
                    ObjUpdateBranch.EnglishName = ObjUpdateBranch.ArabicName;
                }
                else
                {
                    ObjUpdateBranch.EnglishName = ObjUpdate.EnglishName;
                }
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
                _unitOfWork.St_BranchH.Update(ObjUpdateBranch);
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
        public JsonResult Delete(St_BranchHVM ObjDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                ObjDelete.CompanyID = UserInfo.fCompanyId;
                var ObjDeleteSt_Branch = new St_BranchH();
                ObjDeleteSt_Branch.CompanyID = UserInfo.fCompanyId;
                ObjDeleteSt_Branch.BranchCode = ObjDelete.BranchCode;
                var ObjDeleteSt_BranchAccount = new St_BranchAccountH();
                ObjDeleteSt_BranchAccount.CompanyID = UserInfo.fCompanyId;
                ObjDeleteSt_BranchAccount.BranchCode = ObjDelete.BranchCode;
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
                _unitOfWork.NativeSql.DeleteSt_BranchExpenseAccountH(ObjDeleteSt_Branch.CompanyID, ObjDeleteSt_Branch.BranchCode);
                _unitOfWork.NativeSql.DeleteSt_BranchOtherExpenseAccountH(ObjDeleteSt_Branch.CompanyID, ObjDeleteSt_Branch.BranchCode);
                _unitOfWork.St_BranchAccountH.Delete(ObjDeleteSt_BranchAccount);
                _unitOfWork.St_BranchH.Delete(ObjDeleteSt_Branch);
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
        public JsonResult CheckIfBranchCodeExisting(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            string BranchCode = _unitOfWork.St_BranchH.CheckIfBranchCodeExisting(UserInfo.fCompanyId, id);
            return Json(BranchCode, JsonRequestBehavior.AllowGet);
        }
    }
}