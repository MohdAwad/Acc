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
    
    public class DefinitionOtherAccountController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public DefinitionOtherAccountController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        [Authorize(Roles = "CoOwner,ShowOtheraccount")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "CoOwner,AddOtheraccount")]
        public ActionResult SaveDefinitionOtherAccount()
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
                var ObjGet = _unitOfWork.DefinitionOtherAccount.GetDefinitionOtherAccountByID(UserInfo.fCompanyId);
                if (ObjGet != null)
                {
                    DefinitionOtherAccountVM Obj = new DefinitionOtherAccountVM
                    {
                        DefinitionOtherAccount = new OtherAccount
                        {
                            ChequeFundAccountNumber = ObjGet.ChequeFundAccountNumber,
                            CashFundsAccountNumber = ObjGet.CashFundsAccountNumber,
                            ExpensesAccountNumber = ObjGet.ExpensesAccountNumber,
                            PaidExpensesAccountNumber = ObjGet.PaidExpensesAccountNumber,
                            RevenuesAccountNumber = ObjGet.RevenuesAccountNumber,
                            RevenuesReceivedAccountNumber = ObjGet.RevenuesReceivedAccountNumber,
                            SalesTaxAccountNumber = ObjGet.SalesTaxAccountNumber,
                            PurchasesTaxAccountNumber = ObjGet.PurchasesTaxAccountNumber,
                            ReturnSalesTaxAccountNumber = ObjGet.ReturnSalesTaxAccountNumber,
                            ReturnPurchasesTaxAccountNumber = ObjGet.ReturnPurchasesTaxAccountNumber,
                            CustomerAccountNumber = ObjGet.CustomerAccountNumber,
                            SupplierAccountNumber = ObjGet.SupplierAccountNumber
                        },
                        ChequeFundAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ChequeFundAccountNumber),
                        CashFundsAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.CashFundsAccountNumber),
                        ExpensesAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ExpensesAccountNumber),
                        PaidExpensesAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PaidExpensesAccountNumber),
                        RevenuesAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.RevenuesAccountNumber),
                        RevenuesReceivedAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.RevenuesReceivedAccountNumber),
                        SalesTaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.SalesTaxAccountNumber),
                        PurchasesTaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.PurchasesTaxAccountNumber),
                        ReturnSalesTaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnSalesTaxAccountNumber),
                        ReturnPurchasesTaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnPurchasesTaxAccountNumber),
                        CustomerAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.CustomerAccountNumber),
                        SupplierAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.SupplierAccountNumber)
                    };
                    return View("SaveDefinitionOtherAccount", Obj);
                }
                return View("SaveDefinitionOtherAccount", new DefinitionOtherAccountVM());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [HttpPost]
        [Authorize(Roles = "CoOwner,AddOtheraccount")]
        public JsonResult SaveDefinitionOtherAccount(OtherAccount ObjToSave)
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
                _unitOfWork.DefinitionOtherAccount.Delete(ObjToSave);
                _unitOfWork.DefinitionOtherAccount.Add(ObjToSave);
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