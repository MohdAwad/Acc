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
    public class PaymentVoucherCashMultiAccountController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentVoucherCashMultiAccountController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        [Authorize(Roles = "CoOwner,ShowPaymentVoucherCashMultiAccount")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetPaymentVoucherCashMultiAccountFromTransactionKind(UserInfo.fCompanyId);
            int year = DateTime.Now.Year;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            DateTime Today = DateTime.Now;
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            int CurrentYear = Today.Year;
            if (CurrentYear == UserInfo.CurrYear)
            {
                FromDate = DateTime.Now;
                ToDate = DateTime.Now;
            }
            else if (CurrentYear < UserInfo.CurrYear || CurrentYear > UserInfo.CurrYear)
            {
                FromDate = new DateTime(UserInfo.CurrYear, 1, 1);
                ToDate = new DateTime(UserInfo.CurrYear, 1, 1);
            }
            var PaymentCashFilter = new PaymentCashFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(PaymentCashFilter);
        }
        [HttpPost]
        public JsonResult GetAllPaymentVoucherCashMultiAccount(PaymentCashFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaymentCashMultiAccount = _unitOfWork.NativeSql.GetAllPaymentCashMultiAccountFromHeader(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllPaymentCashMultiAccount == null)
                {
                    return Json(new List<PaymentCashFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllPaymentCashMultiAccount = AllPaymentCashMultiAccount.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllPaymentCashMultiAccount = AllPaymentCashMultiAccount.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllPaymentCashMultiAccount = AllPaymentCashMultiAccount.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.FundAccountNumber))
                {
                    AllPaymentCashMultiAccount = AllPaymentCashMultiAccount.Where(m => m.FundAccountNumber == Obj.FundAccountNumber).ToList();
                }
                return Json(AllPaymentCashMultiAccount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<PaymentCashFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,AddPaymentVoucherCashMultiAccount")]
        public ActionResult AddNew()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeadrObj = new Header();
            HeadrObj.VoucherDate = DateTime.Now;
            var TransactionCreditObj = new Transaction();
            var TransactionDebitObj = new List<Transaction>();
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetPaymentVoucherCashMultiAccountFromTransactionKind(UserInfo.fCompanyId);
            var TransFixedVM = new TransactionFixedVM
            {
                Header = HeadrObj,
                TransactionCredit = TransactionCreditObj,
                TransactionDebitList = TransactionDebitObj,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindID = 1,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = 1,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                CurrentYear = UserInfo.CurrYear

            };
            return View(TransFixedVM);
        }
        [ValidateInput(false)]
        public ActionResult GridViewTransActionPartial(string id, string id2, string id3,string id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            ViewBag.WorkWithCostCenter = Company.WorkWithCostCenter;

            if (!String.IsNullOrEmpty(id))
            {
                var TransObj = _unitOfWork.NativeSql.GetTransactionsDetailPaymentVoucherCashMultiDebit(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), int.Parse(id4));
                return PartialView("GridViewTransActionPartial", TransObj);
            }
            else
            {
                var TransObj = new List<TransActionModelVM>();
                return PartialView("GridViewTransActionPartial", TransObj);
            }


        }
        public JsonResult SavePaymentVoucherCashMultiAccount(TransactionFixedVM ObjToSave)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
                var SaveHeader = ObjToSave.Header;
                SaveHeader.InsDateTime = DateTime.Now;
                SaveHeader.InsUserID = userId;
                SaveHeader.CompanyTransactionKindNo = ObjToSave.CompanyTransactionKindID;
                SaveHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, ObjToSave.CompanyTransactionKindID);
                SaveHeader.CompanyID = UserInfo.fCompanyId;
                SaveHeader.TotalCredit = ObjToSave.Amount;
                SaveHeader.TotalDebit = ObjToSave.Amount;
                SaveHeader.SaleID = 0;
                double TotalDebitForeign = 0;
                if (SaveHeader.ConversionFactor == 0)
                {
                    SaveHeader.ConversionFactor = 1;
                };
                if (SaveHeader.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                }
                else
                {
                    TotalDebitForeign = SaveHeader.TotalDebit / SaveHeader.ConversionFactor;
                };
                SaveHeader.TotalDebitForeign = TotalDebitForeign;
                SaveHeader.TotalCreditForeign = TotalDebitForeign;

                SaveHeader.CompanyYear = UserInfo.CurrYear;
                SaveHeader.FCurrencyID = ObjToSave.CurrencyID;
                var ObjComapnyTransactionKind = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo);
                if (ObjComapnyTransactionKind.AutoSerial)
                {
                    SaveHeader.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, SaveHeader.TransactionKindNo, SaveHeader.CompanyYear).ToString();
                    SaveHeader.VHI = int.Parse(SaveHeader.VoucherNumber);
                }
                else
                {
                    string SerialNumber = "";
                    int Serial = ObjComapnyTransactionKind.Serial;
                    string Symbol = ObjComapnyTransactionKind.Symbol;
                    DateTime dDate = SaveHeader.VoucherDate;
                    string YearNo = dDate.ToString("yy");
                    string sMonth = dDate.ToString("MM");
                    int MonthNo = Int16.Parse(sMonth);
                    int LengthLastSerial = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo).ToString().Length;
                    Serial = Serial - LengthLastSerial;
                    for (int i = 0; i <= Serial; i++)
                    {
                        if (i < Serial)
                        {
                            SerialNumber = SerialNumber + "0";
                        }
                        else if (i == Serial)
                        {
                            SerialNumber = SerialNumber + _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo).ToString();
                        }
                    }
                    SaveHeader.VoucherNumber = Symbol + YearNo + sMonth + SerialNumber;
                    SaveHeader.VHI = _unitOfWork.CompanyTransactionKindMonthlySerial.GetMaxSerial(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo);
                    _unitOfWork.CompanyTransactionKindMonthlySerial.Update(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, MonthNo);
                }
                if (Company.DirectExportTheTransactions)
                {
                    SaveHeader.Exported = 1;
                }
                else
                {
                    SaveHeader.Exported = 0;
                }
                var TransactionCredit = new Transaction();
                TransactionCredit.CompanyID = UserInfo.fCompanyId;
                TransactionCredit.CompanyYear = SaveHeader.CompanyYear;
                TransactionCredit.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                TransactionCredit.TransactionKindNo = SaveHeader.TransactionKindNo;
                TransactionCredit.VoucherDate = SaveHeader.VoucherDate;
                TransactionCredit.VoucherNumber = SaveHeader.VoucherNumber;
                TransactionCredit.InsUserID = SaveHeader.InsUserID;
                TransactionCredit.VHI = SaveHeader.VHI;
                TransactionCredit.InsDateTime = DateTime.Now;
                TransactionCredit.AccountNumber = ObjToSave.FromAccountNumber;
                TransactionCredit.CostCenter = ObjToSave.FromCostCenter;
                TransactionCredit.Debit = 0;
                TransactionCredit.Credit = ObjToSave.Amount;
                if (SaveHeader.ConversionFactor == 1)
                {
                    TransactionCredit.CreditForeign = 0;
                }
                else
                {
                    TransactionCredit.CreditForeign = TransactionCredit.Credit / SaveHeader.ConversionFactor;
                };
                TransactionCredit.DebitForeign = 0;
                TransactionCredit.CreditDebitForeign = TransactionCredit.CreditForeign;
                TransactionCredit.Note = SaveHeader.Note;
                TransactionCredit.RowNumber = 1;
                var TransactionDebitList = ObjToSave.TransactionDebitList;
                var iRow = 1;
                foreach (var TransactionDebitSave in TransactionDebitList)
                {
                    try
                    {
                        iRow = iRow + 1;
                        TransactionDebitSave.CompanyID = UserInfo.fCompanyId;
                        TransactionDebitSave.CompanyYear = SaveHeader.CompanyYear;
                        TransactionDebitSave.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                        TransactionDebitSave.TransactionKindNo = SaveHeader.TransactionKindNo;
                        TransactionDebitSave.VoucherDate = SaveHeader.VoucherDate;
                        TransactionDebitSave.VoucherNumber = SaveHeader.VoucherNumber;
                        TransactionDebitSave.InsUserID = SaveHeader.InsUserID;
                        TransactionDebitSave.VHI = SaveHeader.VHI;
                        TransactionDebitSave.InsDateTime = DateTime.Now;
                        TransactionDebitSave.AccountNumber = TransactionDebitSave.AccountNumber;
                        TransactionDebitSave.CostCenter = TransactionDebitSave.CostCenter;
                        TransactionDebitSave.Debit = TransactionDebitSave.Debit;
                        TransactionDebitSave.Credit = 0;
;                        TransactionDebitSave.CreditForeign = 0;
                        if (SaveHeader.ConversionFactor == 1)
                        {
                            TransactionDebitSave.DebitForeign = 0;
                        }
                        else
                        {
                            TransactionDebitSave.DebitForeign = TransactionDebitSave.Debit / SaveHeader.ConversionFactor;
                        };
                        TransactionDebitSave.CreditDebitForeign = TransactionDebitSave.DebitForeign;
                        TransactionDebitSave.RowNumber = iRow;
                        TransactionDebitSave.Note = TransactionDebitSave.Note;
                        _unitOfWork.Transaction.Add(TransactionDebitSave);
                        _unitOfWork.Complete();
                    }
                    catch (Exception ex)
                    {
                        Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                        Msg.Code = 0;
                        return Json(Msg, JsonRequestBehavior.AllowGet);

                    }
                }
                SaveHeader.RowCount = iRow;
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
                _unitOfWork.Header.Add(SaveHeader);
                _unitOfWork.Transaction.Add(TransactionCredit);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;

                Msg.Year = SaveHeader.CompanyYear;
                Msg.FCompanyId = SaveHeader.CompanyID;
                Msg.VoucherNumber = SaveHeader.VoucherNumber.ToString();
                Msg.TransactionKindNo = SaveHeader.TransactionKindNo.ToString();
                Msg.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo.ToString();

                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,UpdatePaymentVoucherCashMultiAccount")]
        public ActionResult UpdatePaymentVoucherCashMultiAccount(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionKindObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CompanyTransactionKindID = HeaderObj.CompanyTransactionKindNo;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 1 , id4);
            var TransactionDebitListObj = new List<Transaction>();
            int iRowNumber = HeaderObj.RowCount;
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionCredit = TransactionCreditObj;
            TransFixedVM.CompanyTransactionKindID = CompanyTransactionKindID;
            if (Resources.Resource.CurLang == "Arb")
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionKindObj.ArabicName;
            }
            else
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionKindObj.EnglishName;
            }
            TransFixedVM.TransactionDebitList = TransactionDebitListObj;
            TransFixedVM.Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId);
            TransFixedVM.CurrencyID = HeaderObj.FCurrencyID;
            TransFixedVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, 3);
            TransFixedVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransFixedVM.AccountCreditName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.Amount = Math.Round(HeaderObj.TotalCredit, 3);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalCredit, 3);
            TransFixedVM.ForeignAmount = Math.Round(HeaderObj.TotalCreditForeign, 3);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalCreditForeign, 3);
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            return View(TransFixedVM);
        }
        [HttpPost]
        public JsonResult UpdatePaymentVoucherCashMultiAccount(TransactionFixedVM ObjToUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var UpdateHeader = ObjToUpdate.Header;
                UpdateHeader.InsDateTime = DateTime.Now;
                UpdateHeader.VoucherNumber = ObjToUpdate.Header.VoucherNumber;
                UpdateHeader.VoucherDate = ObjToUpdate.Header.VoucherDate;
                UpdateHeader.CompanyTransactionKindNo = ObjToUpdate.CompanyTransactionKindID;
                UpdateHeader.TransactionKindNo = _unitOfWork.CompanyTransactionKind.GetTransKindForCompanyTransactionKind(UserInfo.fCompanyId, ObjToUpdate.CompanyTransactionKindID);
                UpdateHeader.InsUserID = userId;
                UpdateHeader.CompanyID = UserInfo.fCompanyId;
                UpdateHeader.TotalDebit = ObjToUpdate.Amount;
                UpdateHeader.TotalCredit = ObjToUpdate.Amount;
                UpdateHeader.SaleID = ObjToUpdate.SaleManNo;
                double TotalCreditForeign = 0;
                if (UpdateHeader.ConversionFactor == 0)
                {
                    UpdateHeader.ConversionFactor = 1;
                };
                if (UpdateHeader.ConversionFactor == 1)
                {
                    TotalCreditForeign = 0;
                }
                else
                {
                    TotalCreditForeign = UpdateHeader.TotalCredit / UpdateHeader.ConversionFactor;
                };
                UpdateHeader.TotalCreditForeign = TotalCreditForeign;
                UpdateHeader.TotalDebitForeign = TotalCreditForeign;

                UpdateHeader.CompanyYear = ObjToUpdate.CompanyYear;
                UpdateHeader.FCurrencyID = ObjToUpdate.CurrencyID;
                var TransactionCredit = new Transaction();
                TransactionCredit.CompanyID = UserInfo.fCompanyId;
                TransactionCredit.CompanyYear = UpdateHeader.CompanyYear;
                TransactionCredit.CompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo;
                TransactionCredit.TransactionKindNo = UpdateHeader.TransactionKindNo;
                TransactionCredit.VoucherDate = UpdateHeader.VoucherDate;
                TransactionCredit.VoucherNumber = UpdateHeader.VoucherNumber;
                TransactionCredit.InsUserID = UpdateHeader.InsUserID;
                TransactionCredit.VHI = UpdateHeader.VHI;
                TransactionCredit.InsDateTime = DateTime.Now;
                TransactionCredit.AccountNumber = ObjToUpdate.FromAccountNumber;
                TransactionCredit.CostCenter = ObjToUpdate.FromCostCenter;
                TransactionCredit.Credit = ObjToUpdate.Amount;
                TransactionCredit.Debit = 0;
                if (UpdateHeader.ConversionFactor == 1)
                {
                    TransactionCredit.CreditForeign = 0;
                }
                else
                {
                    TransactionCredit.CreditForeign = TransactionCredit.Credit / UpdateHeader.ConversionFactor;
                };
                TransactionCredit.DebitForeign = 0;
                TransactionCredit.CreditDebitForeign = TransactionCredit.CreditForeign;
                TransactionCredit.Note = UpdateHeader.Note;
                TransactionCredit.RowNumber = 1;

                var TransactionDebitList = ObjToUpdate.TransactionDebitList;
                var iRow = 1;
                _unitOfWork.NativeSql.DeleteTransActionDebit(UpdateHeader.CompanyID, UpdateHeader.VoucherNumber,
                    UpdateHeader.CompanyTransactionKindNo, UpdateHeader.TransactionKindNo, UpdateHeader.CompanyYear);
                foreach (var TransactionDebitSave in TransactionDebitList)
                {
                    try
                    {
                        iRow = iRow + 1;
                        TransactionDebitSave.CompanyID = UserInfo.fCompanyId;
                        TransactionDebitSave.CompanyYear = UpdateHeader.CompanyYear;
                        TransactionDebitSave.CompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo;
                        TransactionDebitSave.TransactionKindNo = UpdateHeader.TransactionKindNo;
                        TransactionDebitSave.VoucherDate = UpdateHeader.VoucherDate;
                        TransactionDebitSave.VoucherNumber = UpdateHeader.VoucherNumber;
                        TransactionDebitSave.InsUserID = UpdateHeader.InsUserID;
                        TransactionDebitSave.VHI = UpdateHeader.VHI;
                        TransactionDebitSave.InsDateTime = DateTime.Now;
                        TransactionDebitSave.AccountNumber = TransactionDebitSave.AccountNumber;
                        TransactionDebitSave.CostCenter = TransactionDebitSave.CostCenter;
                        TransactionDebitSave.Credit = 0;
                        TransactionDebitSave.Debit = TransactionDebitSave.Debit;
                        TransactionDebitSave.CreditForeign = 0;
                        if (UpdateHeader.ConversionFactor == 1)
                        {
                            TransactionDebitSave.DebitForeign = 0;
                        }
                        else
                        {
                            TransactionDebitSave.DebitForeign = TransactionDebitSave.Debit / UpdateHeader.ConversionFactor;
                        };
                        TransactionDebitSave.CreditDebitForeign = TransactionDebitSave.DebitForeign;
                        TransactionDebitSave.RowNumber = iRow;
                        TransactionDebitSave.Note = TransactionDebitSave.Note;
                        _unitOfWork.Transaction.Add(TransactionDebitSave);
                        _unitOfWork.Complete();
                    }
                    catch (Exception ex)
                    {
                        Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                        Msg.Code = 0;
                        return Json(Msg, JsonRequestBehavior.AllowGet);

                    }
                }
                UpdateHeader.RowCount = iRow;

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
                _unitOfWork.Header.Update(UpdateHeader);
                _unitOfWork.Transaction.Update(TransactionCredit);
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.UpdatedSuccessfully;

                Msg.Year = UpdateHeader.CompanyYear;
                Msg.FCompanyId = UpdateHeader.CompanyID;
                Msg.VoucherNumber = UpdateHeader.VoucherNumber.ToString();
                Msg.TransactionKindNo = UpdateHeader.TransactionKindNo.ToString();
                Msg.CompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo.ToString();

                return Json(Msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Msg.Msg = Resources.Resource.SomthingWentWrong + " : " + ex.Message.ToString();
                Msg.Code = 0;
                return Json(Msg, JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,CopyPaymentVoucherCashMultiAccount")]
        public ActionResult CopyPaymentVoucherCashMultiAccount(string id, int id2, int id3, int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetPaymentVoucherCashMultiAccountFromTransactionKind(UserInfo.fCompanyId);
            var CompanyTransactionKindID = HeaderObj.CompanyTransactionKindNo;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 1 , id4);
            var TransactionDebitListObj = new List<Transaction>();
            int iRowNumber = HeaderObj.RowCount;
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionCredit = TransactionCreditObj;
            TransFixedVM.CompanyTransactionKind = CompanyTransactionKindObj;
            TransFixedVM.CompanyTransactionKindID = CompanyTransactionKindID;
            TransFixedVM.TransactionDebitList = TransactionDebitListObj;
            TransFixedVM.Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId);
            TransFixedVM.CurrencyID = HeaderObj.FCurrencyID;
            TransFixedVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, 3);
            TransFixedVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransFixedVM.AccountCreditName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.Amount = Math.Round(HeaderObj.TotalCredit, 3);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalCredit, 3);
            TransFixedVM.ForeignAmount = Math.Round(HeaderObj.TotalCreditForeign, 3);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalCreditForeign, 3);
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            return View(TransFixedVM);
        }
        [Authorize(Roles = "CoOwner,AttachPaymentVoucherCashMultiAccount")]
        public ActionResult ShowAttach(int id, string id2, string id3, string id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            TransActionFileVM Obj = new TransActionFileVM
            {
                Year = id,
                CompanyId = UserInfo.fCompanyId,
                VoucherNumber = id2,
                TransactionKindNo = id4,
                CompanyTransactionKindNo = id3

            };
            return View("ShowAttach", Obj);

        }
        public ActionResult DetailPaymentVoucherCashMultiAccount(string id, int id2, int id3, int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CurrencyObj = _unitOfWork.Currency.GetCurrencyByID(UserInfo.fCompanyId, HeaderObj.FCurrencyID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 1, id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionCredit = TransactionCreditObj;
            if (Resources.Resource.CurLang == "Arb")
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionObj.ArabicName;
                TransFixedVM.CurrencyName = CurrencyObj.ArabicName;
            }
            else
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionObj.EnglishName;
                TransFixedVM.CurrencyName = CurrencyObj.EnglishName;
            }
            TransFixedVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, 3);
            TransFixedVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransFixedVM.AccountCreditName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.Amount = Math.Round(HeaderObj.TotalCredit, 3);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalCredit, 3);
            TransFixedVM.ForeignAmount = Math.Round(HeaderObj.TotalCreditForeign, 3);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalCreditForeign, 3);
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            return View(TransFixedVM);
        }
        [HttpGet]
        public JsonResult GetTransactionDebit(string id, string id2, string id3 , int id4)
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var AllTransactionsDetailDebit = _unitOfWork.NativeSql.GetAllTransactionsDetailDebit(id, int.Parse(id2), UserInfo.fCompanyId, int.Parse(id3), id4);

            if (AllTransactionsDetailDebit == null)
            {
                return Json(new TransactionFixedVM(), JsonRequestBehavior.AllowGet);
            }
            return Json(AllTransactionsDetailDebit, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "CoOwner,DeletePaymentVoucherCashMultiAccount")]
        public ActionResult DeletePaymentVoucherCashMultiAccount(string id, int id2, int id3, int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3, id4);
            var CompanyTransactionObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CurrencyObj = _unitOfWork.Currency.GetCurrencyByID(UserInfo.fCompanyId, HeaderObj.FCurrencyID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 1, id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionCredit = TransactionCreditObj;
            if (Resources.Resource.CurLang == "Arb")
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionObj.ArabicName;
                TransFixedVM.CurrencyName = CurrencyObj.ArabicName;
            }
            else
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionObj.EnglishName;
                TransFixedVM.CurrencyName = CurrencyObj.EnglishName;
            }
            TransFixedVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, 3);
            TransFixedVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransFixedVM.AccountCreditName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.Amount = Math.Round(HeaderObj.TotalCredit, 3);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalCredit, 3);
            TransFixedVM.ForeignAmount = Math.Round(HeaderObj.TotalCreditForeign, 3);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalCreditForeign, 3);
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            return View(TransFixedVM);
        }
        [Authorize(Roles = "CoOwner,ExportPaymentVoucherCashMultiAccount")]
        public ActionResult ExportPaymentCashMultiAccount()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetPaymentVoucherCashMultiAccountFromTransactionKind(UserInfo.fCompanyId);
            int year = DateTime.Now.Year;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            DateTime Today = DateTime.Now;
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            int CurrentYear = Today.Year;
            if (CurrentYear == UserInfo.CurrYear)
            {
                FromDate = DateTime.Now;
                ToDate = DateTime.Now;
            }
            else if (CurrentYear < UserInfo.CurrYear || CurrentYear > UserInfo.CurrYear)
            {
                FromDate = new DateTime(UserInfo.CurrYear, 1, 1);
                ToDate = new DateTime(UserInfo.CurrYear, 1, 1);
            }
            var PaymentCashFilter = new PaymentCashFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(PaymentCashFilter);
        }
        [HttpPost]
        public JsonResult GetAllPaymentVoucherCashMultiAccountUnExport(PaymentCashFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaymentCash = _unitOfWork.NativeSql.GetAllPaymentCashMultiAccountFromHeaderUnExport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllPaymentCash == null)
                {
                    return Json(new List<PaymentCashFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllPaymentCash = AllPaymentCash.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllPaymentCash = AllPaymentCash.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllPaymentCash = AllPaymentCash.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllPaymentCash)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllPaymentCash, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<PaymentCashFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,UnExportPaymentVoucherCashMultiAccount")]
        public ActionResult UnExportPaymentCashMultiAccount()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetPaymentVoucherCashMultiAccountFromTransactionKind(UserInfo.fCompanyId);
            int year = DateTime.Now.Year;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            DateTime Today = DateTime.Now;
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            int CurrentYear = Today.Year;
            if (CurrentYear == UserInfo.CurrYear)
            {
                FromDate = DateTime.Now;
                ToDate = DateTime.Now;
            }
            else if (CurrentYear < UserInfo.CurrYear || CurrentYear > UserInfo.CurrYear)
            {
                FromDate = new DateTime(UserInfo.CurrYear, 1, 1);
                ToDate = new DateTime(UserInfo.CurrYear, 1, 1);
            }
            var PaymentCashFilter = new PaymentCashFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency
            };
            return View(PaymentCashFilter);
        }
        [HttpPost]
        public JsonResult GetAllPaymentVoucherCashMultiAccountExport(PaymentCashFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllPaymentCash = _unitOfWork.NativeSql.GetAllPaymentCashMultiAccountFromHeaderExport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllPaymentCash == null)
                {
                    return Json(new List<PaymentCashFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllPaymentCash = AllPaymentCash.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllPaymentCash = AllPaymentCash.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllPaymentCash = AllPaymentCash.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllPaymentCash)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllPaymentCash, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<PaymentCashFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult UpdateToExport(ExportAndUnExportVM ObjToUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                foreach (var UpdateHeader in ObjToUpdate.Header)
                {
                    UpdateHeader.Exported = 1;
                    UpdateHeader.CompanyID = UserInfo.fCompanyId;
                    _unitOfWork.Header.UpdateToExportAndUnExport(UpdateHeader);
                    _unitOfWork.Complete();
                }
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.ExportSuccessfully;
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
        public JsonResult UpdateToUnExport(ExportAndUnExportVM ObjToUpdate)
        {
            MsgUnit Msg = new MsgUnit();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                foreach (var UpdateHeader in ObjToUpdate.Header)
                {
                    UpdateHeader.Exported = 0;
                    UpdateHeader.CompanyID = UserInfo.fCompanyId;
                    _unitOfWork.Header.UpdateToExportAndUnExport(UpdateHeader);
                    _unitOfWork.Complete();
                }
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.UnExportSuccessfully;
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