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
    public class St_BranchAccountHController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_BranchAccountHController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Obj = new St_BranchAccountHVM();
            Obj.St_BranchH = _unitOfWork.St_BranchH.GetAllSt_BranchH(UserInfo.fCompanyId);
            return View(Obj);
        }
        public JsonResult GetAllSt_BranchAccountH()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllSt_BranchAccountH = _unitOfWork.NativeSql.GetAllSt_BranchAccountH(UserInfo.fCompanyId);
                if (AllSt_BranchAccountH == null)
                {
                    return Json(new List<St_BranchAccountHVM>(), JsonRequestBehavior.AllowGet);
                }

                return Json(AllSt_BranchAccountH, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<St_BranchAccountHVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult SaveSt_BranchAccountH(string id)
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
                var Obj = new St_BranchAccountHVM();
                var ObjGet = _unitOfWork.St_BranchAccountH.GetSt_BranchAccountHByID(UserInfo.fCompanyId, id);
                var ObjGetBranchName = _unitOfWork.St_BranchH.GetSt_BranchrByID(UserInfo.fCompanyId, id);
                Obj.BranchCode = id;
                if (Resources.Resource.CurLang == "Arb")
                {
                    Obj.BranchName = ObjGetBranchName.ArabicName;
                }
                else {
                    Obj.BranchName = ObjGetBranchName.EnglishName;
                }
                Obj.CompanyTransactionKindReceiptVoucher = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
                Obj.CompanyTransactionKindPaymentVoucher = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
                Obj.CompanyTransactionKindReceiptChequeVoucher = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
                Obj.CompanyTransactionKindTransferFeesAndInstallationVoucher = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
                Obj.CompanyTransactionKindInstallmentSchedulingVoucher = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
                Obj.CompanyTransactionKindMaintenanceCardVoucher = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
                Obj.CompanyTransactionKindTransferVoucher = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
                Obj.CompanyTransactionKindReturningSoldItemVoucher = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
                Obj.CompanyTransactionKindClosingAFundVoucher = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
                Obj.CompanyTransactionKindChequeFundClosingVoucher = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
                Obj.CompanyTransactionKindClosingAFinancingFundVoucher = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
                Obj.CompanyTransactionKindFinancingVoucher = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
                Obj.CompanyTransactionKindInstallationVoucher = _unitOfWork.CompanyTransactionKind.GetAllCompanyTransactionKind(UserInfo.fCompanyId);
                if (ObjGet != null)
                {
                    Obj.FundAccountNumber = ObjGet.FundAccountNumber;
                    Obj.SalesAccountNumber = ObjGet.SalesAccountNumber;
                    Obj.MaintenanceCardAccountNumber = ObjGet.MaintenanceCardAccountNumber;
                    Obj.ReturnCardAccountNumber = ObjGet.ReturnCardAccountNumber;
                    Obj.TransferFeesAndInstallationAccountNumber = ObjGet.TransferFeesAndInstallationAccountNumber;
                    Obj.ChequeFundAccountNumber = ObjGet.ChequeFundAccountNumber;
                    Obj.RelocationAccountNumber = ObjGet.RelocationAccountNumber;
                    Obj.VisaAccountNumber = ObjGet.VisaAccountNumber;
                    Obj.MasterAccountNumber = ObjGet.MasterAccountNumber;
                    Obj.AmericanAccountNumber = ObjGet.AmericanAccountNumber;
                    Obj.ArabiCashAccountNumber = ObjGet.ArabiCashAccountNumber;
                    Obj.FundAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.FundAccountNumber);
                    Obj.SalesAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.SalesAccountNumber);
                    Obj.MaintenanceCardAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.MaintenanceCardAccountNumber);
                    Obj.ReturnCardAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ReturnCardAccountNumber);
                    Obj.TransferFeesAndInstallationAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.TransferFeesAndInstallationAccountNumber);
                    Obj.ChequeFundAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ChequeFundAccountNumber);
                    Obj.RelocationAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.RelocationAccountNumber);
                    Obj.VisaAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.VisaAccountNumber);
                    Obj.MasterAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.MasterAccountNumber);
                    Obj.AmericanAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.AmericanAccountNumber);
                    Obj.ArabiCashAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, ObjGet.ArabiCashAccountNumber);
                    Obj.ReceiptVoucherNo = ObjGet.ReceiptVoucherNo;
                    Obj.PaymentVoucherNo = ObjGet.PaymentVoucherNo;
                    Obj.ReceiptChequeVoucherNo = ObjGet.ReceiptChequeVoucherNo;
                    Obj.TransferFeesAndInstallationVoucherNo = ObjGet.TransferFeesAndInstallationVoucherNo;
                    Obj.InstallmentSchedulingVoucherNo = ObjGet.InstallmentSchedulingVoucherNo;
                    Obj.MaintenanceCardVoucherNo = ObjGet.MaintenanceCardVoucherNo;
                    Obj.TransferVoucherNo = ObjGet.TransferVoucherNo;
                    Obj.ReturningSoldItemVoucherNo = ObjGet.ReturningSoldItemVoucherNo;
                    Obj.ClosingAFundVoucherNo = ObjGet.ClosingAFundVoucherNo;
                    Obj.ChequeFundClosingVoucherNo = ObjGet.ChequeFundClosingVoucherNo;
                    Obj.ClosingAFinancingFundVoucherNo = ObjGet.ClosingAFinancingFundVoucherNo;
                    Obj.FinancingVoucherNo = ObjGet.FinancingVoucherNo;
                    Obj.InstallationVoucherNo = ObjGet.InstallationVoucherNo;

                }
                return View("SaveSt_BranchAccountH", Obj);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return View("Error");
            }
        }
        [HttpPost]
        public JsonResult SaveSt_BranchAccountH(St_BranchAccountHVM ObjToSave)
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
                var St_BranchAccountHObj = new St_BranchAccountH();
                St_BranchAccountHObj.CompanyID = UserInfo.fCompanyId;
                St_BranchAccountHObj.BranchCode = ObjToSave.BranchCode;
                St_BranchAccountHObj.FundAccountNumber = ObjToSave.FundAccountNumber;
                St_BranchAccountHObj.SalesAccountNumber = ObjToSave.SalesAccountNumber;
                St_BranchAccountHObj.MaintenanceCardAccountNumber = ObjToSave.MaintenanceCardAccountNumber;
                St_BranchAccountHObj.ReturnCardAccountNumber = ObjToSave.ReturnCardAccountNumber;
                St_BranchAccountHObj.TransferFeesAndInstallationAccountNumber = ObjToSave.TransferFeesAndInstallationAccountNumber;
                St_BranchAccountHObj.ChequeFundAccountNumber = ObjToSave.ChequeFundAccountNumber;
                St_BranchAccountHObj.RelocationAccountNumber = ObjToSave.RelocationAccountNumber;
                St_BranchAccountHObj.VisaAccountNumber = ObjToSave.VisaAccountNumber;
                St_BranchAccountHObj.MasterAccountNumber = ObjToSave.MasterAccountNumber;
                St_BranchAccountHObj.AmericanAccountNumber = ObjToSave.AmericanAccountNumber;
                St_BranchAccountHObj.ArabiCashAccountNumber = ObjToSave.ArabiCashAccountNumber;
                St_BranchAccountHObj.ReceiptVoucherNo = ObjToSave.ReceiptVoucherNo;
                St_BranchAccountHObj.PaymentVoucherNo = ObjToSave.PaymentVoucherNo;
                St_BranchAccountHObj.ReceiptChequeVoucherNo = ObjToSave.ReceiptChequeVoucherNo;
                St_BranchAccountHObj.TransferFeesAndInstallationVoucherNo = ObjToSave.TransferFeesAndInstallationVoucherNo;
                St_BranchAccountHObj.InstallmentSchedulingVoucherNo = ObjToSave.InstallmentSchedulingVoucherNo;
                St_BranchAccountHObj.MaintenanceCardVoucherNo = ObjToSave.MaintenanceCardVoucherNo;
                St_BranchAccountHObj.TransferVoucherNo = ObjToSave.TransferVoucherNo;
                St_BranchAccountHObj.ReturningSoldItemVoucherNo = ObjToSave.ReturningSoldItemVoucherNo;
                St_BranchAccountHObj.ClosingAFundVoucherNo = ObjToSave.ClosingAFundVoucherNo;
                St_BranchAccountHObj.ChequeFundClosingVoucherNo = ObjToSave.ChequeFundClosingVoucherNo;
                St_BranchAccountHObj.ClosingAFinancingFundVoucherNo = ObjToSave.ClosingAFinancingFundVoucherNo;
                St_BranchAccountHObj.FinancingVoucherNo = ObjToSave.FinancingVoucherNo;
                St_BranchAccountHObj.InstallationVoucherNo = ObjToSave.InstallationVoucherNo;
                St_BranchAccountHObj.InsDateTime = DateTime.Now;
                St_BranchAccountHObj.InsUserID = userId;
                _unitOfWork.NativeSql.DeleteSt_BranchExpenseAccountH(UserInfo.fCompanyId, ObjToSave.BranchCode);
                _unitOfWork.NativeSql.DeleteSt_BranchOtherExpenseAccountH(UserInfo.fCompanyId, ObjToSave.BranchCode);
                _unitOfWork.St_BranchAccountH.Delete(St_BranchAccountHObj);
                _unitOfWork.Complete();
                if (ObjToSave.St_BranchExpenseAccountH != null)
                {
                    foreach (var SaveSt_BranchExpenseAccountH in ObjToSave.St_BranchExpenseAccountH)
                    {
                        SaveSt_BranchExpenseAccountH.CompanyID = UserInfo.fCompanyId;
                        SaveSt_BranchExpenseAccountH.BranchCode = ObjToSave.BranchCode;
                        SaveSt_BranchExpenseAccountH.ExpenseAccountNumber = SaveSt_BranchExpenseAccountH.ExpenseAccountNumber;
                        SaveSt_BranchExpenseAccountH.RowNumber = SaveSt_BranchExpenseAccountH.RowNumber;
                        _unitOfWork.St_BranchAccountH.AddBranchExpenseAccountH(SaveSt_BranchExpenseAccountH);
                    }
                }
                if (ObjToSave.St_BranchOtherExpenseAccountH != null)
                {
                    foreach (var SaveSt_BranchOtherExpenseAccountH in ObjToSave.St_BranchOtherExpenseAccountH)
                    {
                        SaveSt_BranchOtherExpenseAccountH.CompanyID = UserInfo.fCompanyId;
                        SaveSt_BranchOtherExpenseAccountH.BranchCode = ObjToSave.BranchCode;
                        SaveSt_BranchOtherExpenseAccountH.OtherExpenseAccountNumber = SaveSt_BranchOtherExpenseAccountH.OtherExpenseAccountNumber;
                        SaveSt_BranchOtherExpenseAccountH.RowNumber = SaveSt_BranchOtherExpenseAccountH.RowNumber;
                        _unitOfWork.St_BranchAccountH.AddBranchOtherExpenseAccountH(SaveSt_BranchOtherExpenseAccountH);
                    }
                }
                _unitOfWork.St_BranchAccountH.Add(St_BranchAccountHObj);
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
        [ValidateInput(false)]
        public ActionResult GridViewExpenses(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var ExpensesObj = _unitOfWork.NativeSql.GetBranchExpensesAccountH(id, UserInfo.fCompanyId);
                return PartialView("GridViewExpenses", ExpensesObj);
            }
            else
            {
                var ExpensesObj = new List<St_BranchAccountHVM>();
                return PartialView("GridViewExpenses", ExpensesObj);
            }


        }
        [ValidateInput(false)]
        public ActionResult GridViewOtherExpenses(string id)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            if (!String.IsNullOrEmpty(id))
            {
                var OtherExpensesObj = _unitOfWork.NativeSql.GetBranchOtherExpensesAccountH(id, UserInfo.fCompanyId);
                return PartialView("GridViewOtherExpenses", OtherExpensesObj);
            }
            else
            {
                var OtherExpensesObj = new List<St_BranchAccountHVM>();
                return PartialView("GridViewOtherExpenses", OtherExpensesObj);
            }


        }
    }
}