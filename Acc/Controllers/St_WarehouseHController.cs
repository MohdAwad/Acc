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
    public class St_WarehouseHController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_WarehouseHController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var st_WarehouseHVM = new St_WarehouseHVM
            {

            };
            return View(st_WarehouseHVM);
        }
        public ActionResult Add()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            St_WarehouseHVM Obj = new St_WarehouseHVM
            {
            };
            return PartialView(Obj);
        }
        [HttpGet]
        public JsonResult GetAllSt_WarehouseH()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_WarehouseH = _unitOfWork.NativeSql.GetAllSt_WarehouseH(UserInfo.fCompanyId);
                if (AllSt_WarehouseH == null)
                {
                    return Json(new List<St_WarehouseH>(), JsonRequestBehavior.AllowGet);
                }
                int iRow = 0;
                foreach (var iRowCount in AllSt_WarehouseH)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllSt_WarehouseH, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_WarehouseHVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Save(St_WarehouseHVM ObjSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var ObjSaveWarehouseH = new St_WarehouseH();
                ObjSaveWarehouseH.InsDateTime = DateTime.Now;
                ObjSaveWarehouseH.InsUserID = userId;
                ObjSaveWarehouseH.CompanyID = UserInfo.fCompanyId;
                ObjSaveWarehouseH.StockCode = ObjSave.StockCode;
                ObjSaveWarehouseH.Telephone = ObjSave.Telephone;
                ObjSaveWarehouseH.Address = ObjSave.Address;
                ObjSaveWarehouseH.ArabicName = ObjSave.ArabicName;
                ObjSaveWarehouseH.ManufacturingWarehouse = ObjSave.ManufacturingWarehouse;
                if (String.IsNullOrEmpty(ObjSave.EnglishName))
                {
                    ObjSaveWarehouseH.EnglishName = ObjSaveWarehouseH.ArabicName;
                }
                else
                {
                    ObjSaveWarehouseH.EnglishName = ObjSave.EnglishName;
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
                var St_TransactionAllStockTransaction = _unitOfWork.NativeSql.GetSt_TransactionKindHAllStockTransaction(UserInfo.fCompanyId);
                if (St_TransactionAllStockTransaction.Count() == 0)
                {
                    St_TransactionAllStockTransaction = _unitOfWork.NativeSql.GetSt_TransactionKindH();
                    foreach (var SaveSt_TransactionAllStockTransaction in St_TransactionAllStockTransaction)
                    {
                        var St_CompanyTransationKindHObj = new St_CompanyTransactionKindH();
                        St_CompanyTransationKindHObj.CompanyID = UserInfo.fCompanyId;
                        St_CompanyTransationKindHObj.St_CompanyTransactionKindID = _unitOfWork.St_CompanyTransactionKindH.GetMaxSerial(UserInfo.fCompanyId);
                        St_CompanyTransationKindHObj.St_TransactionKindID = SaveSt_TransactionAllStockTransaction.St_TransactionKindID;
                        St_CompanyTransationKindHObj.StockCode = "*";
                        St_CompanyTransationKindHObj.AutoSerial = true;
                        St_CompanyTransationKindHObj.SymbolSerial = false;
                        St_CompanyTransationKindHObj.Symbol = "";
                        St_CompanyTransationKindHObj.Serial = 0;
                        St_CompanyTransationKindHObj.InsUserID = userId;
                        St_CompanyTransationKindHObj.InsDateTime = DateTime.Now;
                        _unitOfWork.St_CompanyTransactionKindH.Add(St_CompanyTransationKindHObj);
                        _unitOfWork.Complete();
                    }
                }
                var St_TransactionAllWithoutStockTransaction = _unitOfWork.NativeSql.GetSt_TransactionKindHWithoutAllStockTransaction();
                foreach (var SaveSt_TransactionWithoutAllStockTransaction in St_TransactionAllWithoutStockTransaction)
                {
                    var St_CompanyTransationKindHObj = new St_CompanyTransactionKindH();
                    St_CompanyTransationKindHObj.CompanyID = UserInfo.fCompanyId;
                    St_CompanyTransationKindHObj.St_CompanyTransactionKindID = _unitOfWork.St_CompanyTransactionKindH.GetMaxSerial(UserInfo.fCompanyId);
                    St_CompanyTransationKindHObj.St_TransactionKindID = SaveSt_TransactionWithoutAllStockTransaction.St_TransactionKindID;
                    St_CompanyTransationKindHObj.StockCode = ObjSaveWarehouseH.StockCode;
                    St_CompanyTransationKindHObj.AutoSerial = true;
                    St_CompanyTransationKindHObj.SymbolSerial = false;
                    St_CompanyTransationKindHObj.Symbol = "";
                    St_CompanyTransationKindHObj.Serial = 0;
                    St_CompanyTransationKindHObj.InsUserID = userId;
                    St_CompanyTransationKindHObj.InsDateTime = DateTime.Now;
                    _unitOfWork.St_CompanyTransactionKindH.Add(St_CompanyTransationKindHObj);
                    _unitOfWork.Complete();
                }
                var St_ItemWarehouse = _unitOfWork.NativeSql.GetSt_ItemWarehouseH(UserInfo.fCompanyId);
                foreach (var SaveSt_ItemWarehouse in St_ItemWarehouse)
                {
                    var St_ItemWarehouseHObj = new St_ItemWarehouseH();
                    St_ItemWarehouseHObj.CompanyID = UserInfo.fCompanyId;
                    St_ItemWarehouseHObj.ItemCode = SaveSt_ItemWarehouse.ItemCode;
                    St_ItemWarehouseHObj.StockCode = ObjSaveWarehouseH.StockCode;
                    St_ItemWarehouseHObj.RowNumber = SaveSt_ItemWarehouse.ItemWarehouseRowNumber + 1;
                    _unitOfWork.St_ItemCardH.AddItemWarehous(St_ItemWarehouseHObj);
                    _unitOfWork.Complete();
                }
                _unitOfWork.St_WarehouseH.Add(ObjSaveWarehouseH);
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
                    var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }

                    var Obj = _unitOfWork.St_WarehouseH.GetSt_WarehouseHByID(UserInfo.fCompanyId, id);
                    var St_WarehouseHObj = new St_WarehouseHVM { };
                    St_WarehouseHObj.StockCode = Obj.StockCode;
                    St_WarehouseHObj.ArabicName = Obj.ArabicName;
                    St_WarehouseHObj.EnglishName = Obj.EnglishName;
                    St_WarehouseHObj.Telephone = Obj.Telephone;
                    St_WarehouseHObj.Address = Obj.Address;
                    St_WarehouseHObj.ManufacturingWarehouse = Obj.ManufacturingWarehouse;
                    return PartialView("Update", St_WarehouseHObj);
                }
                return PartialView("Update", new St_WarehouseHVM());
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
                    var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                    if (UserInfo == null)
                    {
                        RedirectToAction("", "");
                    }

                    var Obj = _unitOfWork.St_WarehouseH.GetSt_WarehouseHByID(UserInfo.fCompanyId, id);
                    var St_WarehouseHObj = new St_WarehouseHVM { };
                    St_WarehouseHObj.StockCode = Obj.StockCode;
                    St_WarehouseHObj.ArabicName = Obj.ArabicName;
                    St_WarehouseHObj.EnglishName = Obj.EnglishName;
                    St_WarehouseHObj.Telephone = Obj.Telephone;
                    St_WarehouseHObj.Address = Obj.Address;
                    St_WarehouseHObj.ManufacturingWarehouse = Obj.ManufacturingWarehouse;
                    return PartialView("Delete", St_WarehouseHObj);
                }
                return PartialView("Delete", new St_WarehouseHVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [HttpPost]
        public JsonResult Update(St_WarehouseHVM ObjUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var ObjUpdateWarehouseH = new St_WarehouseH();
                ObjUpdateWarehouseH.InsDateTime = DateTime.Now;
                ObjUpdateWarehouseH.InsUserID = userId;
                ObjUpdateWarehouseH.CompanyID = UserInfo.fCompanyId;
                ObjUpdateWarehouseH.StockCode = ObjUpdate.StockCode;
                ObjUpdateWarehouseH.Telephone = ObjUpdate.Telephone;
                ObjUpdateWarehouseH.Address = ObjUpdate.Address;
                ObjUpdateWarehouseH.ArabicName = ObjUpdate.ArabicName;
                ObjUpdateWarehouseH.ManufacturingWarehouse = ObjUpdate.ManufacturingWarehouse;
                if (String.IsNullOrEmpty(ObjUpdate.EnglishName))
                {
                    ObjUpdateWarehouseH.EnglishName = ObjUpdateWarehouseH.ArabicName;
                }
                else
                {
                    ObjUpdateWarehouseH.EnglishName = ObjUpdate.EnglishName;
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
                _unitOfWork.St_WarehouseH.Update(ObjUpdateWarehouseH);
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
        public JsonResult Delete(St_WarehouseHVM ObjDelete)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);

                ObjDelete.CompanyID = UserInfo.fCompanyId;
                var ObjDeleteSt_WarehouseH = new St_WarehouseH();
                ObjDeleteSt_WarehouseH.CompanyID = UserInfo.fCompanyId;
                ObjDeleteSt_WarehouseH.StockCode = ObjDelete.StockCode;

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
                _unitOfWork.NativeSql.DeleteSt_CompanyTransactionKindH(ObjDeleteSt_WarehouseH.CompanyID, ObjDeleteSt_WarehouseH.StockCode);
                _unitOfWork.NativeSql.DeleteSt_ItemWarehouseHByStockCode(ObjDeleteSt_WarehouseH.CompanyID, ObjDeleteSt_WarehouseH.StockCode);
                _unitOfWork.St_WarehouseH.Delete(ObjDeleteSt_WarehouseH);
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
        public JsonResult CheckIfStockCodeExisting(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            string StockCode = _unitOfWork.St_WarehouseH.CheckIfStockCodeExisting(UserInfo.fCompanyId, id);
            return Json(StockCode, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CheckStcokBeforDelete(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != null || id != "")
            {
                string StockCode = _unitOfWork.NativeSql.CheckSt_WarehouseH(UserInfo.fCompanyId, id);
                if (StockCode == null || StockCode == "")
                {
                    StockCode = "";
                }
                return Json(StockCode, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}