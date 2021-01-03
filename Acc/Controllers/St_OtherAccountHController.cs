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
    public class St_OtherAccountHController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_OtherAccountHController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SaveSt_OtherAccountH()
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
                var ObjGet = _unitOfWork.St_OtherAccountH.GetSt_OtherAccountHByID(UserInfo.fCompanyId);
                if (ObjGet != null)
                {
                    St_OtherAccountHVM Obj = new St_OtherAccountHVM
                    {
                        SalesTaxAccountNumber = ObjGet.SalesTaxAccountNumber,
                        PurchaseTaxAccountNumber = ObjGet.PurchaseTaxAccountNumber,
                        PurchaseAccountNumber = ObjGet.PurchaseAccountNumber,
                        ForeignPurchaseAccountNumber = ObjGet.ForeignPurchaseAccountNumber,
                        DirectAccountNumber = ObjGet.DirectAccountNumber,
                        DifferenceAccountNumber = ObjGet.DifferenceAccountNumber,
                        BillsOfExchangeAccountNumber = ObjGet.BillsOfExchangeAccountNumber,
                        SalesTaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.SalesTaxAccountNumber),
                        PurchaseTaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchaseTaxAccountNumber),
                        PurchaseAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchaseAccountNumber),
                        ForeignPurchaseAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ForeignPurchaseAccountNumber),
                        DirectAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.DirectAccountNumber),
                        DifferenceAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.DifferenceAccountNumber),
                        BillsOfExchangeAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.BillsOfExchangeAccountNumber)
                    };
                    return View("SaveSt_OtherAccountH", Obj);
                }
                return View("SaveSt_OtherAccountH", new St_OtherAccountHVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [HttpPost]
        public JsonResult SaveSt_OtherAccountH(St_OtherAccountH ObjToSave)
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
                _unitOfWork.St_OtherAccountH.Delete(ObjToSave);
                _unitOfWork.St_OtherAccountH.Add(ObjToSave);
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
    }
}