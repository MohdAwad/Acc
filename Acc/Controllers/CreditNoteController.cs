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
    public class CreditNoteController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreditNoteController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
   
        [Authorize(Roles = "CoOwner,ShowCreditNote")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetCreditNoteFromTransactionKind(UserInfo.fCompanyId);
            int year = DateTime.Now.Year;
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
            var CreditNoteFilter = new CreditNoteFilterVM
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(CreditNoteFilter);
        }
        [HttpPost]
        public JsonResult GetAllCreditNote(CreditNoteFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllCreditNote = _unitOfWork.NativeSql.GetAllCreditNoteFromHeader(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllCreditNote == null)
                {
                    return Json(new List<CreditNoteFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllCreditNote = AllCreditNote.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllCreditNote = AllCreditNote.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllCreditNote = AllCreditNote.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.PaidAccountNumber))
                {
                    AllCreditNote = AllCreditNote.Where(m => m.PaidAccountNumber == Obj.PaidAccountNumber).ToList();
                }
                return Json(AllCreditNote, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<CreditNoteFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "CoOwner,AddCreditNote")]
        public ActionResult AddNew()
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeadrObj = new Header();
            HeadrObj.VoucherDate = DateTime.Now;
            var TransactionDebitObj = new Transaction();
            var TransactionCreditObj = new Transaction();
            var TransactionTaxObj = new Transaction();
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetCreditNoteFromTransactionKind(UserInfo.fCompanyId);
            var TransFixedVM = new TransactionFixedVM
            {
                Header = HeadrObj,
                TransactionDebit = TransactionDebitObj,
                TransactionCredit = TransactionCreditObj,
                TransactionTax = TransactionTaxObj,
                CompanyTransactionKind = CompanyTransactionKindObj,
                CompanyTransactionKindID = 1,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                CurrencyID = 1,
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                CurrentYear = UserInfo.CurrYear


            };
            return View(TransFixedVM);
        }
        [HttpPost]
        public JsonResult SaveCreditNote(TransactionFixedVM ObjToSave)
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
                SaveHeader.TotalCredit = ObjToSave.Amount + ObjToSave.Tax;
                SaveHeader.TotalDebit = ObjToSave.Amount + ObjToSave.Tax;
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
                    SaveHeader.VoucherNumber = _unitOfWork.Header.GetMaxVHByKind(UserInfo.fCompanyId, SaveHeader.CompanyTransactionKindNo, SaveHeader.TransactionKindNo , SaveHeader.CompanyYear).ToString();
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
                if (ObjToSave.Tax > 0)
                    SaveHeader.RowCount = 3;
                else
                    SaveHeader.RowCount = 2;
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
                TransactionCredit.Credit = ObjToSave.Amount + ObjToSave.Tax; 
                TransactionCredit.DebitForeign = 0;
                if (SaveHeader.ConversionFactor == 1)
                {
                    TransactionCredit.CreditForeign = 0;
                }
                else
                {
                    TransactionCredit.CreditForeign = TransactionCredit.Credit / SaveHeader.ConversionFactor;
                };
                TransactionCredit.CreditDebitForeign = TransactionCredit.CreditForeign;
                TransactionCredit.Note = SaveHeader.Note;
                TransactionCredit.RowNumber = 1;

                var TransactionDebit = new Transaction();
                TransactionDebit.CompanyID = UserInfo.fCompanyId;
                TransactionDebit.CompanyYear = SaveHeader.CompanyYear;
                TransactionDebit.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                TransactionDebit.TransactionKindNo = SaveHeader.TransactionKindNo;
                TransactionDebit.VoucherDate = SaveHeader.VoucherDate;
                TransactionDebit.VoucherNumber = SaveHeader.VoucherNumber;
                TransactionDebit.InsUserID = SaveHeader.InsUserID;
                TransactionDebit.VHI = SaveHeader.VHI;
                TransactionDebit.InsDateTime = DateTime.Now;
                TransactionDebit.AccountNumber = ObjToSave.ToAccountNumber;
                TransactionDebit.CostCenter = ObjToSave.ToCostCenter;
                TransactionDebit.Debit = ObjToSave.Amount;
                TransactionDebit.Credit = 0; 
                if (SaveHeader.ConversionFactor == 1)
                {
                    TransactionDebit.CreditForeign = 0;
                }
                else
                {
                    TransactionDebit.DebitForeign = TransactionDebit.Debit / SaveHeader.ConversionFactor;
                };
                TransactionDebit.CreditDebitForeign = TransactionDebit.DebitForeign;
                TransactionDebit.CreditDebitForeign = 0;
                TransactionDebit.Note = SaveHeader.Note;
                TransactionDebit.RowNumber = 2;

                var TransactionTax = new Transaction();
                if (ObjToSave.Tax > 0)
                {

                    TransactionTax.CompanyID = UserInfo.fCompanyId;
                    TransactionTax.CompanyYear = SaveHeader.CompanyYear;
                    TransactionTax.CompanyTransactionKindNo = SaveHeader.CompanyTransactionKindNo;
                    TransactionTax.TransactionKindNo = SaveHeader.TransactionKindNo;
                    TransactionTax.VoucherDate = SaveHeader.VoucherDate;
                    TransactionTax.VoucherNumber = SaveHeader.VoucherNumber;
                    TransactionTax.InsUserID = SaveHeader.InsUserID;
                    TransactionTax.VHI = SaveHeader.VHI;
                    TransactionTax.InsDateTime = DateTime.Now;
                    TransactionTax.AccountNumber = ObjToSave.TaxAccountNumber;
                    TransactionTax.CostCenter = ObjToSave.TaxCostCenter;
                    TransactionTax.Debit = ObjToSave.Tax;
                    TransactionTax.Credit = 0;
                    if (SaveHeader.ConversionFactor == 1)
                    {
                        TransactionTax.DebitForeign = 0;
                    }
                    else
                    {
                        TransactionTax.DebitForeign = TransactionTax.Debit / SaveHeader.ConversionFactor;
                    };
                    TransactionTax.CreditForeign = 0;
                    TransactionTax.CreditDebitForeign = TransactionTax.DebitForeign;
                    TransactionTax.Note = SaveHeader.Note;
                    TransactionTax.RowNumber = 3;
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
                _unitOfWork.Header.Add(SaveHeader);
                _unitOfWork.Transaction.Add(TransactionCredit);
                _unitOfWork.Transaction.Add(TransactionDebit);
                if (ObjToSave.Tax > 0)
                {
                    _unitOfWork.Transaction.Add(TransactionTax);
                }
                _unitOfWork.Complete();
                Msg.Code = 1;
                Msg.Msg = Resources.Resource.AddedSuccessfully;
                Msg.Year = SaveHeader.CompanyYear;
                Msg.VoucherNumber = SaveHeader.VoucherNumber;
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

        [Authorize(Roles = "CoOwner,AttachCreditNote")]
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
        [Authorize(Roles = "CoOwner,UpdateCreditNote")]
        public ActionResult UpdateCreditNote(string id, int id2,int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2,id3 , id4);
            var CompanyTransactionKindObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CompanyTransactionKindID = HeaderObj.CompanyTransactionKindNo;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 1 , id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 2 , id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransactionTaxObj = _unitOfWork.NativeSql.GetTransactionsDetailTax(id, UserInfo.fCompanyId, id2, 3, id4);
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionCreditObj;
            TransFixedVM.TransactionCredit = TransactionDebitObj;
            TransFixedVM.CompanyYear = TransactionDebitObj.CompanyYear;
            if (Resources.Resource.CurLang == "Arb")
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionKindObj.ArabicName;
            }
            else
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionKindObj.EnglishName;
            }
            TransFixedVM.CompanyTransactionKindID = CompanyTransactionKindID;
            TransFixedVM.Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId);
            TransFixedVM.CurrencyID = HeaderObj.FCurrencyID;
            TransFixedVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, Company.TheDecimalPointForTheForeignCurrency);
            TransFixedVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransFixedVM.AccountDebitName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            TransFixedVM.AccountCreditName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.Amount = Math.Round(TransactionCreditObj.Debit, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.ForeignAmount = Math.Round(TransactionCreditObj.DebitForeign, Company.TheDecimalPointForTheForeignCurrency);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalCreditForeign, Company.TheDecimalPointForTheForeignCurrency);
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            if (iRowNumber > 2)
            {
                TransFixedVM.TransactionTax = TransactionTaxObj;
                TransFixedVM.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionTaxObj.AccountNumber);
                TransFixedVM.TaxCostCenterName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionTaxObj.CostCenter);
                TransFixedVM.Tax = Math.Round(TransactionTaxObj.Debit, Company.TheDecimalPointForTheLocalCurrency);
                TransFixedVM.TaxForeign = Math.Round(TransactionTaxObj.DebitForeign, Company.TheDecimalPointForTheForeignCurrency);
            }
            return View(TransFixedVM);
        }
        [Authorize(Roles = "CoOwner,CopyCreditNote")]
        public ActionResult CopyCreditNote(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionKindObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CompanyTransactionKindID = HeaderObj.CompanyTransactionKindNo;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 1 , id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 2 , id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransactionTaxObj = _unitOfWork.NativeSql.GetTransactionsDetailTax(id, UserInfo.fCompanyId, id2, 3, id4);
            var CreditCompanyTransactionKindObj = _unitOfWork.NativeSql.GetCreditNoteFromTransactionKind(UserInfo.fCompanyId);
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionCreditObj;
            TransFixedVM.TransactionCredit = TransactionDebitObj;
            if (Resources.Resource.CurLang == "Arb")
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionKindObj.ArabicName;
            }
            else
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionKindObj.EnglishName;
            }
            TransFixedVM.CompanyTransactionKind = CreditCompanyTransactionKindObj;
            TransFixedVM.CompanyTransactionKindID = CompanyTransactionKindID;
            TransFixedVM.Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId);
            TransFixedVM.CurrencyID = HeaderObj.FCurrencyID;
            TransFixedVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransFixedVM.AccountDebitName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            TransFixedVM.AccountCreditName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.Amount = Math.Round(TransactionCreditObj.Debit, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.ForeignAmount = Math.Round(TransactionCreditObj.DebitForeign, Company.TheDecimalPointForTheForeignCurrency);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalCreditForeign, Company.TheDecimalPointForTheForeignCurrency);
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            if (iRowNumber > 2)
            {
                TransFixedVM.TransactionTax = TransactionTaxObj;
                TransFixedVM.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionTaxObj.AccountNumber);
                TransFixedVM.TaxCostCenterName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionTaxObj.CostCenter);
                TransFixedVM.Tax = Math.Round(TransactionTaxObj.Debit, Company.TheDecimalPointForTheLocalCurrency);
                TransFixedVM.TaxForeign = Math.Round(TransactionTaxObj.DebitForeign, Company.TheDecimalPointForTheForeignCurrency);
            }
            return View(TransFixedVM);
        }
        [HttpPost]
        public JsonResult UpdateCreditNote(TransactionFixedVM ObjToUpdate)
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
                UpdateHeader.TotalCredit = ObjToUpdate.Amount + ObjToUpdate.Tax;
                UpdateHeader.TotalDebit = ObjToUpdate.Amount + ObjToUpdate.Tax;
                double TotalDebitForeign = 0;
                if (UpdateHeader.ConversionFactor == 0)
                {
                    UpdateHeader.ConversionFactor = 1;
                };
                if (UpdateHeader.ConversionFactor == 1)
                {
                    TotalDebitForeign = 0;
                }
                else
                {
                    TotalDebitForeign = UpdateHeader.TotalDebit / UpdateHeader.ConversionFactor;
                };
                UpdateHeader.TotalDebitForeign = TotalDebitForeign;
                UpdateHeader.TotalCreditForeign = TotalDebitForeign;

                UpdateHeader.CompanyYear = ObjToUpdate.CompanyYear;
                UpdateHeader.FCurrencyID = ObjToUpdate.CurrencyID;
                if (ObjToUpdate.Tax > 0)
                    UpdateHeader.RowCount = 3;
                else
                    UpdateHeader.RowCount = 2;
                _unitOfWork.NativeSql.DeleteTransActionTrans(UpdateHeader.CompanyID, UpdateHeader.VoucherNumber,
                    UpdateHeader.CompanyTransactionKindNo, UpdateHeader.TransactionKindNo, UpdateHeader.CompanyYear);
                _unitOfWork.Complete();
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
                TransactionCredit.Debit = 0;
                TransactionCredit.Credit = ObjToUpdate.Amount + ObjToUpdate.Tax;
                TransactionCredit.CreditForeign = 0;
                TransactionCredit.Debit = 0;
                TransactionCredit.Credit = ObjToUpdate.Amount + ObjToUpdate.Tax;
                TransactionCredit.DebitForeign = 0;
                if (UpdateHeader.ConversionFactor == 1)
                {
                    TransactionCredit.CreditForeign = 0;
                }
                else
                {
                    TransactionCredit.CreditForeign = TransactionCredit.Credit / UpdateHeader.ConversionFactor;
                };
                TransactionCredit.CreditDebitForeign = TransactionCredit.CreditForeign;
                TransactionCredit.Note = UpdateHeader.Note;
                TransactionCredit.RowNumber = 1;
                var TransactionDebit = new Transaction();
                TransactionDebit.CompanyID = UserInfo.fCompanyId;
                TransactionDebit.CompanyYear = UpdateHeader.CompanyYear;
                TransactionDebit.CompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo;
                TransactionDebit.TransactionKindNo = UpdateHeader.TransactionKindNo;
                TransactionDebit.VoucherDate = UpdateHeader.VoucherDate;
                TransactionDebit.VoucherNumber = UpdateHeader.VoucherNumber;
                TransactionDebit.InsUserID = UpdateHeader.InsUserID;
                TransactionDebit.VHI = UpdateHeader.VHI;
                TransactionDebit.InsDateTime = DateTime.Now;
                TransactionDebit.AccountNumber = ObjToUpdate.ToAccountNumber;
                TransactionDebit.CostCenter = ObjToUpdate.ToCostCenter;
                TransactionDebit.Debit = ObjToUpdate.Amount;
                TransactionDebit.Credit = 0;
                if (UpdateHeader.ConversionFactor == 1)
                {
                    TransactionDebit.CreditForeign = 0;
                }
                else
                {
                    TransactionDebit.DebitForeign = TransactionDebit.Debit / UpdateHeader.ConversionFactor;
                };
                TransactionDebit.CreditDebitForeign = TransactionDebit.DebitForeign;
                TransactionDebit.Note = UpdateHeader.Note;
                TransactionDebit.RowNumber = 2;

                var TransactionTax = new Transaction();
                if (ObjToUpdate.Tax > 0)
                {
                    TransactionTax.CompanyID = UserInfo.fCompanyId;
                    TransactionTax.CompanyYear = UpdateHeader.CompanyYear;
                    TransactionTax.CompanyTransactionKindNo = UpdateHeader.CompanyTransactionKindNo;
                    TransactionTax.TransactionKindNo = UpdateHeader.TransactionKindNo;
                    TransactionTax.VoucherDate = UpdateHeader.VoucherDate;
                    TransactionTax.VoucherNumber = UpdateHeader.VoucherNumber;
                    TransactionTax.InsUserID = UpdateHeader.InsUserID;
                    TransactionTax.VHI = UpdateHeader.VHI;
                    TransactionTax.InsDateTime = DateTime.Now;
                    TransactionTax.AccountNumber = ObjToUpdate.TaxAccountNumber;
                    TransactionTax.CostCenter = ObjToUpdate.TaxCostCenter;
                    TransactionTax.Debit = ObjToUpdate.Tax;
                    TransactionTax.Credit = 0;
                    if (UpdateHeader.ConversionFactor == 1)
                    {
                        TransactionTax.DebitForeign = 0;
                    }
                    else
                    {
                        TransactionTax.DebitForeign = TransactionTax.Debit / UpdateHeader.ConversionFactor;
                    };
                    TransactionTax.CreditForeign = 0;
                    TransactionTax.CreditDebitForeign = TransactionTax.DebitForeign;
                    TransactionTax.Note = UpdateHeader.Note;
                    TransactionTax.RowNumber = 3;
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
                _unitOfWork.Header.Update(UpdateHeader);
                _unitOfWork.Transaction.Add(TransactionCredit);
                _unitOfWork.Transaction.Add(TransactionDebit);
                if (ObjToUpdate.Tax > 0)
                {
                    _unitOfWork.Transaction.Add(TransactionTax);
                }
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
        public ActionResult DetailCreditNote(string id, int id2,int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CurrencyObj = _unitOfWork.Currency.GetCurrencyByID(UserInfo.fCompanyId, HeaderObj.FCurrencyID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 1 , id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 2 , id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransactionTaxObj = _unitOfWork.NativeSql.GetTransactionsDetailTax(id, UserInfo.fCompanyId, id2, 3, id4);
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionCreditObj;
            TransFixedVM.TransactionCredit = TransactionDebitObj;
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
            TransFixedVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransFixedVM.AccountDebitName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            TransFixedVM.AccountCreditName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.Amount = Math.Round(TransactionCreditObj.Debit, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.ForeignAmount = Math.Round(TransactionCreditObj.DebitForeign, Company.TheDecimalPointForTheForeignCurrency);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalCreditForeign, Company.TheDecimalPointForTheForeignCurrency);
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            if (iRowNumber > 2)
            {
                TransFixedVM.TransactionTax = TransactionTaxObj;
                TransFixedVM.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionTaxObj.AccountNumber);
                TransFixedVM.TaxCostCenterName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionTaxObj.CostCenter);
                TransFixedVM.Tax = Math.Round(TransactionTaxObj.Debit, Company.TheDecimalPointForTheLocalCurrency);
                TransFixedVM.TaxForeign = Math.Round(TransactionTaxObj.DebitForeign, Company.TheDecimalPointForTheForeignCurrency);
            }
            return View(TransFixedVM);
        }
        [Authorize(Roles = "CoOwner,DeleteCreditNote")]
        public ActionResult DeleteCreditNote(string id, int id2,int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CurrencyObj = _unitOfWork.Currency.GetCurrencyByID(UserInfo.fCompanyId, HeaderObj.FCurrencyID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 1 , id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 2 , id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransactionTaxObj = _unitOfWork.NativeSql.GetTransactionsDetailTax(id, UserInfo.fCompanyId, id2, 3, id4);
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionCreditObj;
            TransFixedVM.TransactionCredit = TransactionDebitObj;
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
            TransFixedVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransFixedVM.AccountDebitName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            TransFixedVM.AccountCreditName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.Amount = Math.Round(TransactionCreditObj.Debit, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.ForeignAmount = Math.Round(TransactionCreditObj.DebitForeign, Company.TheDecimalPointForTheForeignCurrency);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalCreditForeign, Company.TheDecimalPointForTheForeignCurrency);
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            if (iRowNumber > 2)
            {
                TransFixedVM.TransactionTax = TransactionTaxObj;
                TransFixedVM.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionTaxObj.AccountNumber);
                TransFixedVM.TaxCostCenterName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionTaxObj.CostCenter);
                TransFixedVM.Tax = Math.Round(TransactionTaxObj.Debit, Company.TheDecimalPointForTheLocalCurrency);
                TransFixedVM.TaxForeign = Math.Round(TransactionTaxObj.DebitForeign, Company.TheDecimalPointForTheForeignCurrency);
            }
            return View(TransFixedVM);
        }
        [Authorize(Roles = "CoOwner,ExportCreditNote")]
        public ActionResult ExportCreditNote()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetCreditNoteFromTransactionKind(UserInfo.fCompanyId);
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
            var CreditNoteFilter = new CreditNoteFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
               TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
        };
            return View(CreditNoteFilter);
        }
        [HttpPost]
        public JsonResult GetAllCreditNoteUnExport(CreditNoteFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllCreditNote = _unitOfWork.NativeSql.GetAllCreditNoteFromHeaderUnExport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllCreditNote == null)
                {
                    return Json(new List<CreditNoteFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllCreditNote = AllCreditNote.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllCreditNote = AllCreditNote.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllCreditNote = AllCreditNote.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllCreditNote)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllCreditNote, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<CreditNoteFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,UnExportCreditNote")]
        public ActionResult UnExportCreditNote()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetCreditNoteFromTransactionKind(UserInfo.fCompanyId);
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
            var CreditNoteFilter = new CreditNoteFilterVM
            {
                FromDate = new DateTime(year, 1, 1),
                ToDate = new DateTime(year, 12, 31),
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(CreditNoteFilter);
        }
        [HttpPost]
        public JsonResult GetAllCreditNoteExport(CreditNoteFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllCreditNote = _unitOfWork.NativeSql.GetAllCreditNoteFromHeaderExport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllCreditNote == null)
                {
                    return Json(new List<CreditNoteFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllCreditNote = AllCreditNote.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllCreditNote = AllCreditNote.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllCreditNote = AllCreditNote.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllCreditNote)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllCreditNote, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<CreditNoteFilterVM>(), JsonRequestBehavior.AllowGet);
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