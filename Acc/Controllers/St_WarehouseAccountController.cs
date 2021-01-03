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
    public class St_WarehouseAccountController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_WarehouseAccountController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SaveSt_WarehouseAccount()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetUserByID(userId);
                var CompanyInfo = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                if (UserInfo == null)
                {
                    RedirectToAction("", "");
                }
                var Obj = new St_WarehouseAccountVM();
                Obj.St_Warehouse = _unitOfWork.St_Warehouse.GetAllSt_Warehouse(UserInfo.fCompanyId);
                Obj.StockCode = _unitOfWork.St_Warehouse.GetFirstStock(UserInfo.fCompanyId);
                var ObjGet = _unitOfWork.St_WarehouseAccount.GetSt_WarehouseAccountByID(UserInfo.fCompanyId, Obj.StockCode);
                int RegisterCase = _unitOfWork.NativeSql.GetSt_RegisterValueByRegisterID(UserInfo.fCompanyId, 1);
                Obj.InventoryType = RegisterCase;
                if (ObjGet != null)
                {
                    Obj.SalesFundAccountNumber = ObjGet.SalesFundAccountNumber;
                    Obj.SalesAccountNumber = ObjGet.SalesAccountNumber;
                    Obj.CostOfGoodsAccountNumber = ObjGet.CostOfGoodsAccountNumber;
                    Obj.SalesRefundFundAccountNumber = ObjGet.SalesRefundFundAccountNumber;
                    Obj.SalesReturnAccountNumber = ObjGet.SalesReturnAccountNumber;
                    Obj.ReturnCostOfGoodsAccountNumber = ObjGet.ReturnCostOfGoodsAccountNumber;
                    Obj.PurchaseFundAccountNumber = ObjGet.PurchaseFundAccountNumber;
                    Obj.PurchaseAccountNumber = ObjGet.PurchaseAccountNumber;
                    Obj.PurchaseFundReturnAccountNumber = ObjGet.PurchaseFundReturnAccountNumber;
                    Obj.PurchaseReturnAccountNumber = ObjGet.PurchaseReturnAccountNumber;
                    Obj.SalesFundAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId,ObjGet.SalesFundAccountNumber);
                    Obj.SalesAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.SalesAccountNumber);
                    Obj.CostOfGoodsAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.CostOfGoodsAccountNumber);
                    Obj.SalesRefundFundAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.SalesRefundFundAccountNumber);
                    Obj.SalesReturnAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.SalesReturnAccountNumber);
                    Obj.ReturnCostOfGoodsAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnCostOfGoodsAccountNumber);
                    Obj.PurchaseFundAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchaseFundAccountNumber);
                    Obj.PurchaseAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchaseAccountNumber);
                    Obj.PurchaseFundReturnAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchaseFundReturnAccountNumber);
                    Obj.PurchaseReturnAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchaseReturnAccountNumber);
                }
                return View("SaveSt_WarehouseAccount", Obj);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [HttpPost]
        public JsonResult SaveSt_WarehouseAccount(St_WarehouseAccount ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                ObjToSave.InsDateTime = DateTime.Now;
                ObjToSave.InsUserID = userId;
                ObjToSave.CompanyID = UserInfo.fCompanyId;
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
                _unitOfWork.St_WarehouseAccount.Delete(ObjToSave);
                _unitOfWork.St_WarehouseAccount.Add(ObjToSave);
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
        [HttpGet]
        public JsonResult GetWarehouseAccountByStockCode(string id)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            if (id != "")
            {
                var ObjWarehouseAccount = _unitOfWork.NativeSql.GetSt_WarehouseAccountByStock(UserInfo.fCompanyId, id);
                return Json(ObjWarehouseAccount, JsonRequestBehavior.AllowGet);

            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}