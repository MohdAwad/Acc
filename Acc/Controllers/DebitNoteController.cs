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
    public class DebitNoteController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public DebitNoteController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        // GET: DebitNote

        [Authorize(Roles = "CoOwner,ShowDebitNote")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetDebitNoteFromTransactionKind(UserInfo.fCompanyId);
            var SaleObj = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
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
            var DebitNoteFilter = new DebitNoteFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                Sale = SaleObj,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(DebitNoteFilter);
        }
        [HttpPost]
        public JsonResult GetAllDebitNote(DebitNoteFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllDebitNote = _unitOfWork.NativeSql.GetAllDebitNoteFromHeader(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllDebitNote == null)
                {
                    return Json(new List<DebitNoteFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllDebitNote = AllDebitNote.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllDebitNote = AllDebitNote.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.SaleID != 0)
                {
                    AllDebitNote = AllDebitNote.Where(m => m.SaleID == Obj.SaleID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllDebitNote = AllDebitNote.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                if (!String.IsNullOrEmpty(Obj.PaidAccountNumber))
                {
                    AllDebitNote = AllDebitNote.Where(m => m.PaidAccountNumber == Obj.PaidAccountNumber).ToList();
                }
                return Json(AllDebitNote, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<DebitNoteFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "CoOwner,AddDebitNote")]
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
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetDebitNoteFromTransactionKind(UserInfo.fCompanyId);
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
                SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId),
                WorkWithCostCenter = Company.WorkWithCostCenter,
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency,
                CurrentYear = UserInfo.CurrYear

            };
            return View(TransFixedVM);
        }
        [HttpPost]
        public JsonResult SaveDebitNote(TransactionFixedVM ObjToSave)
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
                SaveHeader.SaleID = ObjToSave.SaleManNo;
                double TotalDebitForeign = 0;
                if (SaveHeader.ConversionFactor == 0) {
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
                if (ObjToSave.Tax > 0)
                    SaveHeader.RowCount = 3;
                else
                    SaveHeader.RowCount = 2;
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
                TransactionDebit.AccountNumber = ObjToSave.FromAccountNumber;
                TransactionDebit.CostCenter = ObjToSave.FromCostCenter;
                TransactionDebit.Debit = ObjToSave.Amount + ObjToSave.Tax;
                TransactionDebit.Credit = 0;
                if (SaveHeader.ConversionFactor == 1)
                {
                    TransactionDebit.DebitForeign = 0;
                }
                else
                {
                    TransactionDebit.DebitForeign = TransactionDebit.Debit / SaveHeader.ConversionFactor;
                };
                TransactionDebit.CreditForeign = 0;
                TransactionDebit.CreditDebitForeign = TransactionDebit.DebitForeign;
                TransactionDebit.Note = SaveHeader.Note;
                TransactionDebit.RowNumber = 1;

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
                TransactionCredit.AccountNumber = ObjToSave.ToAccountNumber;
                TransactionCredit.CostCenter = ObjToSave.ToCostCenter;
                TransactionCredit.Debit = 0;
                TransactionCredit.Credit = ObjToSave.Amount;
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
                TransactionCredit.RowNumber = 2;

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
                    TransactionTax.Debit = 0;
                    TransactionTax.Credit = ObjToSave.Tax;
                    TransactionTax.DebitForeign = 0;
                    if (SaveHeader.ConversionFactor == 1)
                    {
                        TransactionTax.CreditForeign = 0;
                    }
                    else
                    {
                        TransactionTax.CreditForeign = TransactionTax.Credit / SaveHeader.ConversionFactor;
                    };
                    TransactionTax.CreditDebitForeign = TransactionTax.CreditForeign;
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
                _unitOfWork.Transaction.Add(TransactionDebit);
                _unitOfWork.Transaction.Add(TransactionCredit);
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
        [Authorize(Roles = "CoOwner,AttachDebitNote")]
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
        [Authorize(Roles = "CoOwner,UpdateDebitNote")]
        public ActionResult UpdateDebitNote(string id, int id2,int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2,id3 , id4);
            var CompanyTransactionKindObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CompanyTransactionKindID = HeaderObj.CompanyTransactionKindNo;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 1 , id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 2 , id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransactionTaxObj = _unitOfWork.NativeSql.GetTransactionsDetailTax(id, UserInfo.fCompanyId, id2, 3, id4);
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionDebitObj;
            TransFixedVM.TransactionCredit = TransactionCreditObj;
            TransFixedVM.CompanyTransactionKindID = CompanyTransactionKindID;
            TransFixedVM.CompanyYear = HeaderObj.CompanyYear;
            if (HeaderObj.SaleID == 0)
            {
                TransFixedVM.SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
            }
            else
            {
                TransFixedVM.SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                TransFixedVM.SaleManNo = HeaderObj.SaleID;
            }
            if (Resources.Resource.CurLang == "Arb")
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionKindObj.ArabicName;
            }
            else
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionKindObj.EnglishName;
            }
            TransFixedVM.Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId);
            TransFixedVM.CurrencyID = HeaderObj.FCurrencyID;
            TransFixedVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            if (!String.IsNullOrEmpty(TransactionDebitObj.AccountNumber))
            {
                TransFixedVM.AccountDebitName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);

            }
            TransFixedVM.AccountCreditName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.Amount = Math.Round(TransactionCreditObj.Credit, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.ForeignAmount = Math.Round(TransactionCreditObj.CreditForeign, Company.TheDecimalPointForTheForeignCurrency);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign, 3);
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            if (iRowNumber > 2)
            {
                TransFixedVM.TransactionTax = TransactionTaxObj;
                TransFixedVM.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionTaxObj.AccountNumber);
                TransFixedVM.TaxCostCenterName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionTaxObj.CostCenter);
                TransFixedVM.Tax = Math.Round(TransactionTaxObj.Credit, Company.TheDecimalPointForTheLocalCurrency);
                TransFixedVM.TaxForeign = Math.Round(TransactionTaxObj.CreditForeign, Company.TheDecimalPointForTheForeignCurrency);
            }
            return View(TransFixedVM);
    }
        [Authorize(Roles = "CoOwner,CopyDebitNote")]
        public ActionResult CopyDebitNote(string id, int id2, int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);
            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2, id3 , id4);
            var CompanyTransactionKindObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CompanyTransactionKindID = HeaderObj.CompanyTransactionKindNo;
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 1 , id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 2 , id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransactionTaxObj = _unitOfWork.NativeSql.GetTransactionsDetailTax(id, UserInfo.fCompanyId, id2, 3, id4);
            var DebitCompanyTransactionKindObj = _unitOfWork.NativeSql.GetDebitNoteFromTransactionKind(UserInfo.fCompanyId);
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionDebitObj;
            TransFixedVM.TransactionCredit = TransactionCreditObj;
            TransFixedVM.CompanyTransactionKind = DebitCompanyTransactionKindObj;
            TransFixedVM.CompanyTransactionKindID = CompanyTransactionKindID;
            if (HeaderObj.SaleID == 0)
            {
                TransFixedVM.SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
            }
            else
            {
                TransFixedVM.SaleMan = _unitOfWork.Sale.GetAllSale(UserInfo.fCompanyId);
                TransFixedVM.SaleManNo = HeaderObj.SaleID;
            }
            if (Resources.Resource.CurLang == "Arb")
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionKindObj.ArabicName;
            }
            else
            {
                TransFixedVM.CompanyTransactionKindName = CompanyTransactionKindObj.EnglishName;
            }
            TransFixedVM.Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId);
            TransFixedVM.CurrencyID = HeaderObj.FCurrencyID;
            TransFixedVM.CurrencyNewValue = Math.Round(HeaderObj.ConversionFactor, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.WorkWithCostCenter = Company.WorkWithCostCenter;
            TransFixedVM.AccountDebitName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            TransFixedVM.AccountCreditName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.Amount = Math.Round(TransactionCreditObj.Credit, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.ForeignAmount = Math.Round(TransactionCreditObj.CreditForeign, Company.TheDecimalPointForTheForeignCurrency);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign, Company.TheDecimalPointForTheForeignCurrency);
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            if (iRowNumber > 2)
            {
                TransFixedVM.TransactionTax = TransactionTaxObj;
                TransFixedVM.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionTaxObj.AccountNumber);
                TransFixedVM.TaxCostCenterName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionTaxObj.CostCenter);
                TransFixedVM.Tax = Math.Round(TransactionTaxObj.Credit, Company.TheDecimalPointForTheLocalCurrency);
                TransFixedVM.TaxForeign = Math.Round(TransactionTaxObj.CreditForeign, Company.TheDecimalPointForTheForeignCurrency);
            }
            return View(TransFixedVM);
        }
        [HttpPost]
        public JsonResult UpdateDebitNote(TransactionFixedVM ObjToUpdate)
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
                UpdateHeader.SaleID = ObjToUpdate.SaleManNo;
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
                TransactionDebit.AccountNumber = ObjToUpdate.FromAccountNumber;
                TransactionDebit.CostCenter = ObjToUpdate.FromCostCenter;
                TransactionDebit.Debit = ObjToUpdate.Amount + ObjToUpdate.Tax;
                TransactionDebit.Credit = 0;
                if (UpdateHeader.ConversionFactor == 1)
                {
                    TransactionDebit.DebitForeign = 0;
                }
                else
                {
                    TransactionDebit.DebitForeign = TransactionDebit.Debit / UpdateHeader.ConversionFactor;
                };
                TransactionDebit.CreditForeign = 0;
                TransactionDebit.CreditDebitForeign = TransactionDebit.DebitForeign;
                TransactionDebit.Note = UpdateHeader.Note;
                TransactionDebit.RowNumber = 1;

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
                TransactionCredit.AccountNumber = ObjToUpdate.ToAccountNumber;
                TransactionCredit.CostCenter = ObjToUpdate.ToCostCenter;
                TransactionCredit.Debit = 0;
                TransactionCredit.Credit = ObjToUpdate.Amount;
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
                TransactionCredit.RowNumber = 2;

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
                    TransactionTax.Debit = 0;
                    TransactionTax.Credit = ObjToUpdate.Tax;
                    TransactionTax.DebitForeign = 0;
                    if (UpdateHeader.ConversionFactor == 1)
                    {
                        TransactionTax.CreditForeign = 0;
                    }
                    else
                    {
                        TransactionTax.CreditForeign = TransactionTax.Credit / UpdateHeader.ConversionFactor;
                    };
                    TransactionTax.CreditDebitForeign = TransactionTax.CreditForeign;
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
                _unitOfWork.Transaction.Add(TransactionDebit);
                _unitOfWork.Transaction.Add(TransactionCredit);
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
        public ActionResult DetailDebitNote(string id, int id2,int id3 , int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2,id3 , id4);
            var CompanyTransactionObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CurrencyObj = _unitOfWork.Currency.GetCurrencyByID(UserInfo.fCompanyId, HeaderObj.FCurrencyID);
            var SalesManObj = _unitOfWork.Sale.GetSaleByID(UserInfo.fCompanyId, HeaderObj.SaleID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 1, id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 2, id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransactionTaxObj = _unitOfWork.NativeSql.GetTransactionsDetailTax(id, UserInfo.fCompanyId, id2, 3, id4);
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionDebitObj;
            TransFixedVM.TransactionCredit = TransactionCreditObj;
            if (HeaderObj.SaleID == 0)
            {
              TransFixedVM.SaleManName = "";
            }
            else
            {
              TransFixedVM.SaleManName = SalesManObj.SalesName;
            }
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
            TransFixedVM.AccountDebitName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            TransFixedVM.AccountCreditName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.Amount = Math.Round(TransactionCreditObj.Credit, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.ForeignAmount = Math.Round(TransactionCreditObj.CreditForeign, Company.TheDecimalPointForTheForeignCurrency);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign, Company.TheDecimalPointForTheForeignCurrency);
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            if (iRowNumber > 2)
            {
                TransFixedVM.TransactionTax = TransactionTaxObj;
                TransFixedVM.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionTaxObj.AccountNumber);
                TransFixedVM.TaxCostCenterName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionTaxObj.CostCenter);
                TransFixedVM.Tax = Math.Round(TransactionTaxObj.Credit, Company.TheDecimalPointForTheLocalCurrency);
                TransFixedVM.TaxForeign = Math.Round(TransactionTaxObj.CreditForeign, Company.TheDecimalPointForTheForeignCurrency);
            }
            return View(TransFixedVM);
        }
        [Authorize(Roles = "CoOwner,DeleteDebitNote")]
        public ActionResult DeleteDebitNote(string id, int id2,int id3, int id4)
        {
            string UserID = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(UserID);

            var HeaderObj = _unitOfWork.Header.GetHeaderDataById(id, UserInfo.fCompanyId, id2,id3, id4);
            var CompanyTransactionObj = _unitOfWork.CompanyTransactionKind.GetCompanyTransactionKindByID(UserInfo.fCompanyId, HeaderObj.CompanyTransactionKindNo);
            var CurrencyObj = _unitOfWork.Currency.GetCurrencyByID(UserInfo.fCompanyId, HeaderObj.FCurrencyID);
            var SalesManObj = _unitOfWork.Sale.GetSaleByID(UserInfo.fCompanyId, HeaderObj.SaleID);
            var Company = _unitOfWork.Company.GetMyCompany(UserInfo.fCompanyId);
            var TransactionDebitObj = _unitOfWork.NativeSql.GetTransactionsDetailDebit(id, UserInfo.fCompanyId, id2, 1, id4);
            var TransactionCreditObj = _unitOfWork.NativeSql.GetTransactionsDetailCredit(id, UserInfo.fCompanyId, id2, 2, id4);
            int iRowNumber = HeaderObj.RowCount;
            var TransactionTaxObj = _unitOfWork.NativeSql.GetTransactionsDetailTax(id, UserInfo.fCompanyId, id2, 3, id4);
            var TransFixedVM = new TransactionFixedVM { };
            TransFixedVM.Header = HeaderObj;
            TransFixedVM.TransactionDebit = TransactionDebitObj;
            TransFixedVM.TransactionCredit = TransactionCreditObj;
            TransFixedVM.CompanyTransactionKindID = HeaderObj.CompanyTransactionKindNo;
            TransFixedVM.CurrencyID = HeaderObj.FCurrencyID;
            if (HeaderObj.SaleID == 0)
            {
                TransFixedVM.SaleManName = "";
            }
            else
            {
                TransFixedVM.SaleManName = SalesManObj.SalesName;
            }
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
            TransFixedVM.AccountDebitName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionDebitObj.AccountNumber);
            TransFixedVM.AccountCreditName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionCreditObj.AccountNumber);
            TransFixedVM.CostCenterDebitName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionDebitObj.CostCenter);
            TransFixedVM.CostCenterCreditName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionCreditObj.CostCenter);
            TransFixedVM.Amount = Math.Round(TransactionCreditObj.Credit, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.TotalAmount = Math.Round(HeaderObj.TotalDebit, Company.TheDecimalPointForTheLocalCurrency);
            TransFixedVM.ForeignAmount = Math.Round(TransactionCreditObj.CreditForeign, Company.TheDecimalPointForTheForeignCurrency);
            TransFixedVM.TotalForeign = Math.Round(HeaderObj.TotalDebitForeign, Company.TheDecimalPointForTheForeignCurrency);
            TransFixedVM.TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency;
            TransFixedVM.TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency;
            if (iRowNumber > 2)
            {
                TransFixedVM.TransactionTax = TransactionTaxObj;
                TransFixedVM.TaxAccountName = _unitOfWork.NativeSql.GetAccountName(UserInfo.fCompanyId, TransactionTaxObj.AccountNumber);
                TransFixedVM.TaxCostCenterName = _unitOfWork.NativeSql.GetCostCenterName(UserInfo.fCompanyId, TransactionTaxObj.CostCenter);
                TransFixedVM.Tax = Math.Round(TransactionTaxObj.Credit, Company.TheDecimalPointForTheLocalCurrency);
                TransFixedVM.TaxForeign = Math.Round(TransactionTaxObj.CreditForeign, Company.TheDecimalPointForTheForeignCurrency);
            }
            return View(TransFixedVM);
        }
        [Authorize(Roles = "CoOwner,ExportDebitNote")]
        public ActionResult ExportDebitNote()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetDebitNoteFromTransactionKind(UserInfo.fCompanyId);
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
            var DebitNoteFilter = new DebitNoteFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(DebitNoteFilter);
        }
        [HttpPost]
        public JsonResult GetAllDebitNoteUnExport(DebitNoteFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllDebitNote = _unitOfWork.NativeSql.GetAllDebitNoteFromHeaderUnExport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllDebitNote == null)
                {
                    return Json(new List<DebitNoteFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllDebitNote = AllDebitNote.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllDebitNote = AllDebitNote.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllDebitNote = AllDebitNote.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllDebitNote)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllDebitNote, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<DebitNoteFilterVM>(), JsonRequestBehavior.AllowGet);
            }

        }
        [Authorize(Roles = "CoOwner,UnExportDebitNote")]
        public ActionResult UnExportDebitNote()
        {
            var userId = User.Identity.GetUserId();
            var UserInfo = _unitOfWork.User.GetMyInfo(userId);
            var CompanyTransactionKindObj = _unitOfWork.NativeSql.GetDebitNoteFromTransactionKind(UserInfo.fCompanyId);
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
            var DebitNoteFilter = new DebitNoteFilterVM
            {
                FromDate = FromDate,
                ToDate = ToDate,
                CompanyTransactionKind = CompanyTransactionKindObj,
                Currency = _unitOfWork.Currency.GetAllCurrency(UserInfo.fCompanyId),
                TheDecimalPointForTheLocalCurrency = Company.TheDecimalPointForTheLocalCurrency,
                TheDecimalPointForTheForeignCurrency = Company.TheDecimalPointForTheForeignCurrency
            };
            return View(DebitNoteFilter);
        }
        [HttpPost]
        public JsonResult GetAllDebitNoteExport(DebitNoteFilterVM Obj)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var UserInfo = _unitOfWork.User.GetMyInfo(userId);
                var AllDebitNote = _unitOfWork.NativeSql.GetAllDebitNoteFromHeaderExport(UserInfo.fCompanyId, Obj.FromDate, Obj.ToDate);
                if (AllDebitNote == null)
                {
                    return Json(new List<DebitNoteFilterVM>(), JsonRequestBehavior.AllowGet);
                }
                if (!String.IsNullOrEmpty(Obj.VoucherNumber))
                {
                    AllDebitNote = AllDebitNote.Where(m => m.VoucherNumber == Obj.VoucherNumber).ToList();
                }
                if (Obj.CurrencyID != 0)
                {
                    AllDebitNote = AllDebitNote.Where(m => m.CurrencyID == Obj.CurrencyID).ToList();
                }
                if (Obj.CompanyTransactionKindNo != 0)
                {
                    AllDebitNote = AllDebitNote.Where(m => m.CompanyTransactionKindNo == Obj.CompanyTransactionKindNo).ToList();
                }
                int iRow = 0;
                foreach (var iRowCount in AllDebitNote)
                {
                    iRowCount.iRowTable = iRow;
                    iRow = iRow + 1;
                }
                return Json(AllDebitNote, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<DebitNoteFilterVM>(), JsonRequestBehavior.AllowGet);
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